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
                m_IndicatorVFXController.PlayChargeVFX(false);
                LowPowerShoot();
                m_ChargeCurrent = 0;
            }
            if(m_Firing)
            {

            }
            m_PreviousFiring = m_Firing;
        }

        base.Update();
    }
    protected override void Startup()
    {
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
        {
            //no spawner found, add ours.
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Spread>();
        }
        else
        {   //spawner found, but what type?
            //do not want base type. i want derived type
            if (m_ProjectileSpawner is ProjectileSpawner_Spread)
            {
                //happy
            }
            else
            {
                //not happy
                Destroy(m_ProjectileSpawner);
                m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Spread>();
            }
        }
    }
    //#######################
    //Shooting mechanics

    private void LowPowerShoot()
    {
        //weapon was previously firing, but stopped
        m_chargePower = m_ChargeCurrent / GetChargeMax();
        //if within attackspeed
        if (m_chargePower > 0.5f && Time.time > m_AttackInterval)
        {
            SpawnProjectiles();
            m_DidShoot = true;
            postShoot();
        }
        m_chargePower = 1;
    }

    protected override void preShoot(Vector3 _aimDir)
    {
        if(m_ChargeCurrent == 0)
        {
            m_IndicatorVFXController.PlayChargeVFX(true);
        }
        ChargeUp();

        base.preShoot(_aimDir);
    }

    protected override void postShoot()
    {
        if(m_DidShoot)
        {
            m_IndicatorVFXController.PlayChargeVFX(false);
            //reset charge amount.
            m_ChargeCurrent = 0;
        }
        base.postShoot();
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

        _bullet.GetComponent<Projectile>().SetupValues(tag, ShipStats);
    }


    protected override bool ShootConditions()
    {
        if (m_ChargeCurrent >= GetChargeMax())
        {
            //if fully charged
            return true;
        }

        //if not fully charged cannot auto shoot.
        //if shoot is released, check against attackspeed, if can fire from attackspeed then can fire underpowered shot
        return false;
    }

}
