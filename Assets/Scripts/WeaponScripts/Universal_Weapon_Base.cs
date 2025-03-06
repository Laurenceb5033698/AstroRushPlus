using UnityEngine;

/// <summary>
/// Handles system settings and structural methods for Universal_Weapon.
/// </summary>
public abstract class Universal_Weapon_Base : MonoBehaviour
{
    //system variables
    public AudioSource m_AudioSource;

    public Stats ShipStats;
    public GameObject m_AimingIndicator;

    //local flags
    bool m_DidShoot = false;
    bool m_CoolingOff = false;
    bool m_Reloading = false;
    bool m_Charging = false;
    bool m_ReloadBuff = false;

    //local variables
    float m_AttackInterval = 0; //internal cooldown for attackspeed
    float m_BurnoutCurrent = 0;
    float m_ReloadCurrent = 0;
    float m_ChargeCurrent = 0;
    const float m_MaxCharge = 100;
    float m_RampCurrent = 0;

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }
    //for visual or audio updating
    void Update()
    {
        
    }
    //for gameplay updating
    private void FixedUpdate()
    {
        
    }

    abstract protected void Shoot();
    abstract protected void SpawProjectilesImpl();
    virtual protected void SpawnProjectiles()
    {
        SpawProjectilesImpl();
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


    //weapon mechanics
    protected float CalcAttackspeed()
    {
        //attackspeed can change per shot each frame.
        float initialAttackspeed = ShipStats.gAttackspeed.Value;
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
        //increase m_ChargeCurrent by 20* attackspeed_modifier (where attackspeed_modifier = 1+x%)
        //charge amount causes damage, size, proj-speed to increase by %
        //charge of 0 = -50% modifier
        //charge of 100 = +50% modifier

        //+100% attackspeed modifier reduces time to charge by 50% (ie twice as fast)
    }

    protected void RampUp()
    {
        //ramp increases while firing continuously. increases +1 per bullet shot.
        //adds to current ramp. is maxed when equal to stat maxRamp
        //modifies attackspeed by % per ramp stack.
        //trying to fire when fully ramped adds to burnout time.
        if(m_RampCurrent < ShipStats.gRampAmount.Max)
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
}
