using UnityEngine;

public class UWeapon_Railgun : Universal_Weapon_Base
{
    //railgun charges up then releases at max charge.
    //currently no support for overcharge, however releasing early fires an under-powered shot
    bool m_PreviousFiring;
    float m_chargePower = 1f;

    public override void Update()
    {
        if (m_PreviousFiring != m_Firing)
        {
            if (m_PreviousFiring)
            {
                LowPowerShoot();
            }
            m_PreviousFiring = m_Firing;
        }

        base.Update();
    }

    //#######################
    //Shooting mechanics

    private void LowPowerShoot()
    {
        //weapon was previously firing, but stopped
        m_chargePower = m_ChargeCurrent / GetChargeMax();
        //if within attackspeed
        if (Time.time > m_AttackInterval)
        {
            SpawnProjectiles();
            m_DidShoot = true;
            postShoot();
        }
        m_chargePower = 1;
    }

    protected override void preShoot(Vector3 _aimDir)
    {
        ChargeUp();

        base.preShoot(_aimDir);
    }

    protected override void postShoot()
    {
        if(m_DidShoot)
        {
            //reset charge amount.
            m_ChargeCurrent = 0;
        }
        base.postShoot();
    }


    /// <summary>
    /// While not defualt, railgun supports spread shot.
    /// </summary>
    /// <param name="_shootPosition"></param>
    /// <param name="_aimDirection"></param>
    protected override void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(ShipStats.Get(StatType.gProjectileAmount));
        float spreadAngle = ShipStats.Get(StatType.gSpreadAngle);

        //angle between each bullet. S = A/N-1, where N>1
        float separation = 0.0f;

        if (numProjectiles < 1)
            numProjectiles = 1;

        if (numProjectiles == 1)
            spreadAngle = 0;
        else
            separation = spreadAngle / (numProjectiles - 1);



        //create rotation about up vector for start direction.
        Quaternion direction = Quaternion.LookRotation(_shootPosition - transform.position, Vector3.up);
        Quaternion StartRotation = direction * Quaternion.AngleAxis(spreadAngle / -2, Vector3.up);

        GameObject bullet;
        for (int i = 0; i < numProjectiles; i++)
        {
            //create unit rotation for this index.
            Quaternion unitRotation = Quaternion.AngleAxis(separation * i, Vector3.up);
            //quaternions rotate by multiplying. Rotates startdirection by step amount.
            Quaternion bulletDirection = unitRotation * StartRotation;
            bullet = Instantiate<GameObject>(m_BulletPrefab, _shootPosition, bulletDirection);
            
            SetupBullet(bullet);
        }
    }


    //#######################
    //Weapon mechanics

    protected override void SetupBullet(GameObject _bullet)
    {
        //bullet stats modified by chargepower, if shot early, chargepower < 1; otherwise = 1
        string tag = ShipStats.gameObject.tag;
        float dmg = ShipStats.Get(StatType.gAttack) * m_chargePower;
        float spd = ShipStats.Get(StatType.bSpeed) * m_chargePower;
        //float dmg = ShipStats.Get(StatType.gAttack);

        _bullet.GetComponent<Projectile>().SetupValues((int)dmg, spd, tag);
    }


    protected override bool ShootConditions()
    {
        if (m_ChargeCurrent >= ShipStats.Get(StatType.gChargeTime))
        {
            //if fully charged
            return true;
        }

        //if not fully charged cannot auto shoot.
        //if shoot is released, check against attackspeed, if can fire from attackspeed then can fire underpowered shot
        return false;
    }

}
