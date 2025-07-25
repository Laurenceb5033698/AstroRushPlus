using NUnit.Framework;
using Unity.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

/// <summary>
/// Handles system settings and structural methods for Universal_Weapon.
/// </summary>
[RequireComponent(typeof(ProjectileSpawner))]
public abstract class Universal_Weapon_Base : MonoBehaviour
{
    //system variables
    public AudioSource m_AudioSource;
    public IndicatorVFXController m_IndicatorVFXController;
    public ProjectileSpawner m_ProjectileSpawner;

    //public delegate void ProjectileSetupDelegate(GameObject _spawned);
    public ProjectileSpawner.ProjectileSetupDelegate SetupDelegate;
    public IWeaponStats InterfaceStats;

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

    //local amounts
    protected int m_Ammo = 0;

    //local variables
    protected float m_AttackInterval = 0; //internal cooldown for attackspeed
    protected float m_BurnoutCurrent = 0;
    protected float m_ReloadCurrent = 0;
    protected float m_ChargeCurrent = 0;
    protected const float m_MaxCharge = 100;
    protected float m_RampCurrent = 0;

    [SerializeField] protected float m_BurstDelay = 0.2f;

    private void Awake()
    {
        Startup();
        SetupDelegate += SetupBullet;
    }

    void Start()
    {
        //try get indicator vfx controller.
        if(m_AimingIndicatorHolder)
            m_IndicatorVFXController = m_AimingIndicatorHolder.GetComponent<IndicatorVFXController>();
    }
    /// <summary>
    /// Adds required components in start. Override to add specific components
    /// </summary>
    protected virtual void Startup()
    {   //adds required spawner component
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if(m_ProjectileSpawner == null)
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner>();

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
            float attackInterval = 1 / (ShipStats.Get(StatType.gAttackspeed) < 0.1f ? 0.1f : ShipStats.Get(StatType.gAttackspeed));
            m_AttackInterval = Time.time + attackInterval + CalcBurstInterval();
        }
    }

    virtual protected void SpawnProjectiles()
    {
        //handle burst
        int burst = Mathf.FloorToInt( ShipStats.Get(StatType.gBurstAmount));
        if (burst == 0)
        {
            //single burst, same as normal shooting
            Burst();
        }
        else
        {
            //burst stat >0; start burst
            float timeBetween = m_BurstDelay / ShipStats.Get(StatType.gAttackspeed);
            BurstAsync(timeBetween, burst + 1);
        }
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

        _bullet.GetComponent<Projectile>().SetupValues(tag, ShipStats);
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
    /// <summary>
    /// processes reload. use isReloading for state.
    /// </summary>
    protected void ReloadAmmo()
    {
        if (m_Ammo <= 0)
        {
            m_Reloading = true;
            m_ReloadCurrent -= Time.deltaTime;
            if (m_ReloadCurrent <= 0)
            {
                m_Ammo = (int)ShipStats.Get(StatType.gReloadAmmo);
                m_ReloadCurrent = ShipStats.Get(StatType.gReloadTime);
                m_Reloading = false;
            }
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

    protected void Burst()
    {
        Vector3 shootPosition = m_AimingIndicator.transform.position;
        Vector3 toIndicator = (shootPosition - m_AimingIndicatorHolder.transform.position).normalized;
        Quaternion aimDirection = Quaternion.LookRotation(toIndicator, Vector3.up);

        //Spawn Projectiles
        //List<GameObject> bullets;
        m_ProjectileSpawner.Spawn(m_BulletPrefab, shootPosition, aimDirection, SetupDelegate);

        //foreach (GameObject bullet in bullets)
        //{
        //    SetupBullet(bullet);
        //}
    }

    protected void BurstAsync(float _betweenBurstTime, int _numBursts)
    {
        Vector3 shootPosition = m_AimingIndicator.transform.position;
        Vector3 toIndicator = (shootPosition - m_AimingIndicatorHolder.transform.position).normalized;
        Quaternion aimDirection = Quaternion.LookRotation(toIndicator, Vector3.up);

        StartCoroutine( m_ProjectileSpawner.SpawnAsync(_betweenBurstTime, _numBursts, m_BulletPrefab, shootPosition, aimDirection, SetupDelegate));

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
    public virtual void Setup(GameObject _Ship)
    {
        m_Ship = _Ship;
        ShipStats = m_Ship.GetComponent<Stats>();
        InterfaceStats = new IWeaponStats(ShipStats);

    }

    //UTILs for composites
    /// <summary>
    /// returns stat value for given stat.
    /// </summary>
    /// <param name="_type"></param>
    public float GetStat(StatType _type)
    {
        return ShipStats.Get(_type);
    }

    protected virtual float CalcBurstInterval()
    {
        float interval = 0;
        int amount = Mathf.FloorToInt(InterfaceStats.BurstAmount);

        if(amount > 0)
        {
            float attacksPerSec = amount/ InterfaceStats.AttackSpeed;
            interval = amount * attacksPerSec * m_BurstDelay;

        }

        return interval;
    }
}
