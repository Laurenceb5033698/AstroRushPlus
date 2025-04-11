using Unity.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;

/// <summary>
/// Handles system settings and structural methods for Universal_Weapon.
/// </summary>
public abstract class Universal_Weapon_Base : MonoBehaviour
{
    //system variables
    public AudioSource m_AudioSource;
    public IndicatorVFXController m_IndicatorVFXController;

    public GameObject m_Ship;
    public Stats ShipStats;
    public GameObject m_AimingIndicator;
    public GameObject m_AimingIndicatorHolder;

    public GameObject m_BulletPrefab;

    //local flags
    protected bool m_Firing = false;
    protected bool m_DidShoot = false;
    protected bool m_PreviousDidShoot = false;
    protected bool m_CoolingOff = false;
    protected bool m_Reloading = false;
    protected bool m_Charging = false;
    protected bool m_ReloadBuff = false;

    //local variables
    protected float m_AttackInterval = 0; //internal cooldown for attackspeed
    protected float m_BurnoutCurrent = 0;
    protected float m_ReloadCurrent = 0;
    protected float m_ChargeCurrent = 0;
    protected const float m_MaxCharge = 100;
    protected float m_RampCurrent = 0;

    private void Awake()
    {
    }

    void Start()
    {
        //try get indicator vfx controller.
        m_IndicatorVFXController = m_AimingIndicatorHolder.GetComponent<IndicatorVFXController>();
    }

    //for visual or audio updating
    virtual public void Update()
    {
        if(m_PreviousDidShoot != m_DidShoot)
        {
            //weapon shot, or stopped shooting last frame.
            m_PreviousDidShoot = m_DidShoot;
            //update vfx state.
            if(m_IndicatorVFXController)
                m_IndicatorVFXController.ShootVFX(m_DidShoot);
        }

        //becuase FireWeapon is called from update, we reset didshoot in update too.
        //didshoot only true on frames where a bullet was sucessfully shot.
        m_DidShoot = false;

        //while attempting to fire, is set true in Preshoot(), then unset here, after work has been done.
        //can use last frames' state of isFiring for visuals.
        m_Firing = false;
    }

    //for gameplay updating
    virtual public void FixedUpdate()
    {
        //most of these handled by their respective weapons
        //reduce attack interval
        //cooldown
        //reload
        //charge

        if(m_IndicatorVFXController)
            m_IndicatorVFXController.ChargeSpeed(GetChargeMax());


    }

    //#######################
    //Shooting mechanics

    /// <summary>
    /// Runs when controller is trying to fire in a direction.
    /// Try every frame to shoot weapon.
    /// Direction is governed by weapon indicator.
    /// </summary>
    virtual public void Shoot(Vector3 _aimDir)
    {
        preShoot(_aimDir);
        ShootImpl();
        postShoot();
    }
    virtual protected void preShoot(Vector3 _aimDir)
    {
        //turn indicator
        m_AimingIndicatorHolder.transform.rotation = Quaternion.LookRotation(_aimDir, Vector3.up);

        m_Firing = true;
        //m_DidShoot = false;
    }
    virtual protected void ShootImpl()
    {
        if (ShootConditions())
        {
            SpawnProjectiles();
            m_DidShoot = true;
        }
    }
    virtual protected void postShoot()
    {
        if (m_DidShoot)
        {
            //sanitise attackspeed. minimum attackspeed = 1 shot every 10s lol
            float attackInterval = ShipStats.Get(StatType.gAttackspeed) < 0.1f ? 0.1f : ShipStats.Get(StatType.gAttackspeed);
            m_AttackInterval = Time.time + (1 / attackInterval);
        }
    }

    //when shoot is successful, does all spawning of projectiles
    abstract protected void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection);
    virtual protected void SpawnProjectiles()
    {
        Vector3 shootPosition = m_AimingIndicator.transform.position;
        Vector3 toIndicator = shootPosition - m_AimingIndicatorHolder.transform.position;
        toIndicator.Normalize();
        Quaternion aimDirection = Quaternion.LookRotation(toIndicator, Vector3.up);
        SpawnProjectilesImpl(shootPosition, aimDirection);
        DoneSpawning();
    }
    protected void DoneSpawning()
    {
        m_AudioSource.Play();
    }

    public void VolumeChanged(float _volume)
    {
        m_AudioSource.volume = _volume;
    }




    //#######################
    //Weapon mechanics

    virtual protected void SetupBullet(GameObject _bullet)
    {
        string tag = ShipStats.gameObject.tag;
        float dmg = ShipStats.Get(StatType.gAttack);
        float spd = ShipStats.Get(StatType.bSpeed);
        //float dmg = ShipStats.Get(StatType.gAttack);

        _bullet.GetComponent<Projectile>().SetupValues((int)dmg,spd,tag);
    }


    /// <summary>
    /// test if weapon can fire.
    /// </summary>
    /// <returns></returns>
    virtual protected bool ShootConditions()
    {
        if(Time.time > m_AttackInterval)
        {
            return true;
        }

        return false;
    }


    protected float CalcAttackspeed()
    {
        //attackspeed can change per shot each frame.
        float initialAttackspeed = ShipStats.Get(StatType.gAttackspeed);
        //conditional or temp increases
        float ramp = (m_RampCurrent * 0.05f);    //each stack adds 5%
        float reloadBuff = (m_ReloadBuff ? 0.5f : 0.0f);    //if applied adds 50%
        //

        float finalAttackspeed = initialAttackspeed + ramp + reloadBuff;

        //do validation

        return finalAttackspeed;
    }
    protected void ReloadAmmo()
    {
        if (m_ReloadCurrent > 0)
        {
            m_Reloading = true;
            m_ReloadCurrent -= Time.deltaTime;
        }
        else
        {
            m_Reloading = false;
        }
    }

    protected void CooldownBurnout()
    {
        //if burntout, start cooling off
        if (m_CoolingOff)
        {
            m_BurnoutCurrent -= Time.deltaTime;
        }
        //once fully cooled, stop cooling off
        if(m_BurnoutCurrent <=0)
        {
            m_CoolingOff = false;
        }
    }

    protected void ChargeUp()
    {
        if (m_ChargeCurrent < GetChargeMax())
            m_ChargeCurrent += Time.deltaTime;
    }

    protected float GetChargeMax()
    {
        //+100% attackspeed modifier reduces time to charge by 50% (ie twice as fast)

        //attackspeed = shots per sec
        //attackspeed = 1
        //default charge = 2s

        //charge max = dcharge/attack
        //eg
        //  attspd = 5 : chrgmx = 2/5 = 0.4s
        //  attspd = 0.2 : chrgmx = 2/0.2 = 2*5 = 10s
        float attackspeed = Mathf.Max(0.01f, ShipStats.Get(StatType.gAttackspeed)); //do not allow <= 0 
        float defaultChargeMax = ShipStats.Get(StatType.gChargeTime);

        return defaultChargeMax / attackspeed;
    }

    protected void RampUp()
    {
        //ramp increases while firing continuously. increases +1 per bullet shot.
        //adds to current ramp. is maxed when equal to stat maxRamp
        //modifies attackspeed by % per ramp stack.
        //trying to fire when fully ramped adds to burnout time.
        if(m_RampCurrent < ShipStats.Get(StatType.gRampAmount))
        {
            m_RampCurrent += 1;
        }
        else
        {
            m_BurnoutCurrent += Time.deltaTime;
        }
    }

    //check flags
    protected bool isReloading()
    {
        return m_Reloading;
    }
    protected bool isCoolingdown()
    {
        return m_CoolingOff;
    }
    protected bool isCharging()
    {
        return m_Charging;
    }


    //SETUP
    public void Setup(GameObject _Ship)
    {
        m_Ship = _Ship;
        ShipStats = m_Ship.GetComponent<Stats>();
    }
}
