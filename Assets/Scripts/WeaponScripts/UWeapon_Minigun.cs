using UnityEngine;

public class UWeapon_Minigun : Universal_Weapon_Base
{
    [SerializeField] float m_CooldownPeriod = 2.0f;

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

    protected override void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(ShipStats.Get(StatType.gProjectileAmount));
        float spreadAngle = ShipStats.Get(StatType.gSpreadAngle) / 10;

        //unknown if minigun can have multiple projectiles

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

        //minigun ramps up, then begins to get innacurate as it overheats.
        GameObject bullet;
        for ( int i = 0; i< numProjectiles; i++)
        {
            //if firing multiple projectiles, each projectile gets its own inaccuracy.
            Quaternion QuatInaccuracy = Quaternion.AngleAxis(Random.Range(-m_BurnoutCurrent, m_BurnoutCurrent), Vector3.up);

            //if spread shot, unit rotation splits projectiles into streams
            Quaternion unitRotation = Quaternion.AngleAxis(separation * i, Vector3.up);
            //quaternions rotate by multiplying. Rotates startdirection by step amount.
            Quaternion bulletDirection = unitRotation * StartRotation * QuatInaccuracy;



            bullet = Instantiate<GameObject>(m_BulletPrefab, _shootPosition, bulletDirection);
            SetupBullet(bullet);
        }

    }

}
