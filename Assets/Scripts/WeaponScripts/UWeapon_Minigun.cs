using UnityEngine;

public class UWeapon_Minigun : Universal_Weapon_Base
{
    [SerializeField] float m_CooldownPeriod = 2.0f;

    protected override void Startup()
    {
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
        {
            //no spawner found, add ours.
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Minigun>();
        }
        else
        {   //spawner found, but what type?
            //do not want base type. i want derived type
            if (m_ProjectileSpawner is ProjectileSpawner_Minigun)
            {
                //happy
            }
            else
            {
                //not happy
                Destroy(m_ProjectileSpawner);
                m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Minigun>();
            }
        }
    }
    public override void Update()
    {
        //handle minigun cooling
        //when overheated, reduce burnouttime.
        if (m_CoolingOff)
        {
            m_BurnoutCurrent -= Time.deltaTime;
            //fully burning out resets ramp in PostShoot()
        }
        else
        {
            //not overheated, but we were firing this/last frame
            if (m_Firing)
            {
                //if fully ramped up, add to burnout time
                if (m_RampCurrent >= ShipStats.Get(StatType.gRampAmount))
                    m_BurnoutCurrent += Time.deltaTime;
            }
            else 
            {    //if you can shoot, but didnt
                 //burnout and ramp reduce
                 if(m_BurnoutCurrent > 0)
                    m_BurnoutCurrent -= Time.deltaTime;
                 if (m_RampCurrent > 0) //reduce ramp to 0 over 5 seconds(ramp time)
                     m_RampCurrent -= (Time.deltaTime *(ShipStats.Get(StatType.gRampAmount)/ ShipStats.Get(StatType.gRampTime)));
            }
        }
        
        //when current burnout drops below 0, reset to 0 and finish cooling off.
        if(m_BurnoutCurrent < 0)
        {
            m_BurnoutCurrent = 0;
            m_CoolingOff = false;
        }


        //call base update for visuals
        base.Update();
    }

    protected override void ShootImpl()
    {
        (m_ProjectileSpawner as ProjectileSpawner_Minigun).SetValues(m_BurnoutCurrent);
        base.ShootImpl();
    }

    protected override void postShoot()
    {
        if( m_DidShoot)
        {
            float rampAmount = ShipStats.Get(StatType.gRampAmount);
            if (m_RampCurrent < rampAmount)
            {
                //increase rampCurrent by 1
                m_RampCurrent++;
            }
            //else
            //{
            //    //maxxed out ramp current, add to burnout
            //    m_BurnoutCurrent += Time.deltaTime;
            //}

            //if burnoutcurrent exceeds max burnouttime (from stats), then overheat weapon and begin cooldown.
            if(m_BurnoutCurrent >= ShipStats.Get(StatType.gBurnoutTime))
            {
                m_CoolingOff = true;
                //fully reset current ramp. set burnout to cooldown period.
                m_BurnoutCurrent = m_CooldownPeriod;
                m_RampCurrent = 0;
            }

            //ramp mult increases attackspeed by a factor of 0.5*m_RampCurrent, where m_RampCurrent is the number of shots fired
            float rampMult = (1 + (m_RampCurrent * 0.5f));  //20 shots  = 10x attackspd
            float attackInterval = ShipStats.Get(StatType.gAttackspeed) * rampMult;
            m_AttackInterval = Time.time + (1 / attackInterval);
        }
    }

    protected override bool ShootConditions()
    {
        //cannot shoot while cooling off
        if(!m_CoolingOff && m_AttackInterval < Time.time)
        {
            return true;
        }
        return false;
    }

}
