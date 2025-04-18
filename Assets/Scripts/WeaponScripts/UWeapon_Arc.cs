using UnityEngine;

public class UWeapon_Arc : Universal_Weapon_Base
{
    //copied railgun :( some changes, no undercharged shots

    //arc charges up then releases at max charge.
    //currently no support for overcharge,
    bool m_PreviousFiring;

    public override void Update()
    {
        if (m_PreviousFiring != m_Firing)
        {
            if (m_PreviousFiring)
            {
                m_IndicatorVFXController.PlayChargeVFX(false);
                m_ChargeCurrent = 0;
            }
            if (m_Firing)
            {

            }
            m_PreviousFiring = m_Firing;
        }

        base.Update();
    }

    //#######################
    //Shooting mechanics


    protected override void preShoot(Vector3 _aimDir)
    {
        if (m_ChargeCurrent == 0)
        {
            m_IndicatorVFXController.PlayChargeVFX(true);
        }
        ChargeUp();

        base.preShoot(_aimDir);
    }

    protected override void postShoot()
    {
        if (m_DidShoot)
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
        //if num Projectiles >1, add multiple certain stats by sata*(0.9^N)
        string tag = ShipStats.gameObject.tag;
        float dmg = ShipStats.Get(StatType.gAttack);
        float spd = ShipStats.Get(StatType.bSpeed);
        //float size = ShipStats.Get(StatType.bSize);

        _bullet.GetComponent<Projectile>().SetupValues((int)dmg, spd, tag);
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
