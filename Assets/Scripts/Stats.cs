using UnityEngine;

/// <summary>
/// Holds all the stats relevant to a ship.
/// Each stat can be modified by upgrades.
/// </summary>
public class Stats : MonoBehaviour
{
    public StatBlock block = new StatBlock();

    public Buffs m_Buffs;
    private StatBuff m_PassiveShieldRegenBuff;

    //Internal Current values for stats that have current and max. Get using properties ShipHealth, ShipShield, ShipFuel
    //generally speaking these are important values.
    [SerializeField] private float Health;
    [SerializeField] private float Shield;
    [SerializeField] private float Fuel;
    [SerializeField] private int Bombs;

    //regen timers
    [Range(0f, 10f)]
    [SerializeField] private float m_ShieldRegenDelay = 4.0f;
    private float m_TimeSinceDamage;
    bool m_shieldBuffApplied = false;

    private float m_RegenHealthTimer;
    private float m_RegenShieldTimer;

    //Status effects
    [SerializeField]
    private bool GodMode = false;
    //private bool inCombat = false;

    [SerializeField] private bool EmpDisabled = false;
    private float empTime = 0.0f;   //time in seconds

    //  Unity Functions
    private void Awake()
    {
        //required to use inspector values
        block.RecalculateStats();
        m_Buffs = GetComponent<Buffs>();
        if(m_Buffs == null )
            m_Buffs = gameObject.AddComponent<Buffs>();
        CreatePassiveShieldRegenBuff();
    }

    /// <summary>
    /// called by upgrade manager after base stats are loaded.
    /// needed to include loadout and ship base stats in starting health.
    /// </summary>
    public void PostLoaded()
    {
        //set current values from each respective max
        Health = Get(StatType.sHealth);
        Shield = Get(StatType.sShield);
        Fuel = Get(StatType.sFuel);
        Bombs = Mathf.FloorToInt(Get(StatType.mAmmo));
    }


    private void Start()
    {
    }

    private void Update()
    {
        decreaseEmp();
        UpdateRegeneration();
    }

    public float Get(StatType _type)
    {
        return block.Get(_type).Get();
    }

    public enum OnDamageReturn
    {
        Killed,
        Damaged,
        None
    }
    //  Functions
    //returns result of taking damage.
    public OnDamageReturn TakeDamage(float val)
    {
        //already dead, return none.
        if (!IsAlive())
        {
            return OnDamageReturn.None;
        }
        OnDamageReturn ret = OnDamageReturn.None;
        if (!GodMode)
        {
            m_TimeSinceDamage = Time.time + m_ShieldRegenDelay;
            //inCombat = true;
            if (ShipShield > 0) //if we have shields
                if (Shield - val > 0)   //and the damage taken is lower than sheild health
                    ShipShield = -val;   //do damage to shield
                else
                {//otherwise split damage between shield and health
                    ShipHealth = (Shield - val);   //remaining damage is delt to health; (shield-val) here will always be negative
                    ShipShield = -Shield;        //and shield is set to 0
                }
            else
                ShipHealth = -val;   //shield is at 0, so deal damage directly to health
            ret = OnDamageReturn.Damaged;
        }
        return IsAlive() ? ret : OnDamageReturn.Killed;
    }

    //Properties
    //-----------------------------------------------------------------------------------------
    //  Health

    public float ShipHealth
    {
        get { return Health; }
        set //can be set using powerups
        {
            if (value > 0)
            {
                Health = (Health + value > Get(StatType.sHealth)) ? Get(StatType.sHealth) : Health + value;
            }
            else if (value < 0)
            {
                Health = (Health + value < 0) ? 0 : Health + value;
            }
        }
    }
    public float ShipShield
    {
        get { return Shield; }
        set //can be set using powerups
        {
            if (value > 0)
            {
                Shield = (Shield + value > Get(StatType.sShield)) ? Get(StatType.sShield) : Shield + value;
            }
            else if (value < 0)
            {
                Shield = (Shield + value < 0) ? 0 : Shield + value;
            }
        }
    }

    public bool IsAlive()
    {
        return (Health > 0);
    }
    public bool IsShielded()
    {
        return (Shield > 0);
    }

    public float GetHealth()
    {
        return Health;
    }
    public void SetHealth()
    {
        ShipHealth = Get(StatType.sHealth);
    }
    public float GetShieldMax()
    {
        return Get(StatType.sShield);
    }


    private void UpdateRegeneration()
    {
        //while in main gamestate, ship can heal and recharge shield.
        if (ServicesManager.Instance.GameStateService.GameState == GameState.MAINGAME)
        {
            ManageCombatShieldRegenBuff();
            //each process regenerates 1 tick of regen for its respective stat per second.
            RegenerateHealth();
            RegenerateShield();
        }
    }

    //Regenerate functions apply regen stat onto health in given timeframe.
    //they do not control the amount of regen applied.
    private void RegenerateHealth()
    {
        if (m_RegenHealthTimer > Time.time)
            return;
        if (!IsAlive() || Health >= Get(StatType.sHealth))
            return;

        float regenValue = Get(StatType.sHealthRegen);
        if (regenValue <= 0.0f)
            return;

        //applies 1 tick of regen each second.
        ShipHealth = regenValue;
        m_RegenHealthTimer = Time.time + 1;
    }

    private void RegenerateShield()
    {
        if (m_RegenShieldTimer > Time.time)
            return;
        if (!IsAlive() || Shield >= Get(StatType.sShield))
            return;

        float regenValue = Get(StatType.sShieldRegen);
        if (regenValue <= 0.0f)
            return;

        //applies 1 tick of regen each second.
        ShipShield = regenValue;
        m_RegenShieldTimer = Time.time + 1;
    }

    private void ManageCombatShieldRegenBuff()
    {
        if (m_PassiveShieldRegenBuff.expired())
        {
            //when expired, buff is automatically removed by buffs update.
            m_shieldBuffApplied = false;
        }

        if (m_TimeSinceDamage <= Time.time)
        {
            //apply new passive shield regen buff
            if (!m_shieldBuffApplied)
            {
                //reset timer, and apply.
                m_PassiveShieldRegenBuff.SetTimer(20);
                m_Buffs.AddExistingBuff(m_PassiveShieldRegenBuff);
                m_shieldBuffApplied = true;
            }
        }
        else
        {
            //been hit, or waiting: remove buff if applied
            if (m_shieldBuffApplied)
            {
                //remove buff
                m_Buffs.RemoveExistingBuff(m_PassiveShieldRegenBuff);
                m_PassiveShieldRegenBuff.SetTimer(0);
                m_shieldBuffApplied = false;
            }
        }        
    }

    private void CreatePassiveShieldRegenBuff()
    {
        //shield buff, 20s duration, 5 regen/s
        m_PassiveShieldRegenBuff = new StatBuff(StatType.sShieldRegen, this, BuffType.FlatStat, 20.0f, 5.0f);
    }

    //  Speed & movement
    //-----------------------------------------------------------------------------------------
    public float ShipFuel
    {
        get { return Fuel; }
        set
        {
            if (value > 0)
            {
                Fuel = (Fuel + value > 100.0f) ? Get(StatType.sFuel) : Fuel + value;
            }
            else if (value < 0)
            {
                Fuel = (Fuel + value < 0.0f) ? 0 : Fuel + value;
            }
        }
    }
    public float GetMainThrust()
    {
        return Get(StatType.sSpeed);
    }
    public float GetRotSpeed()
    {
        return Get(StatType.sTurnrate);
    }

    //  Abillity / boost power
    public float GetSpecial()
    {
        return Get(StatType.sSpecial);
    }
    //  Missile
    //-----------------------------------------------------------------------------------------
    public int OrdinanceAmmo 
    { 
        get { return Bombs; }
        set 
        {
            int maxBombs = Mathf.FloorToInt(Get(StatType.mAmmo));
            Bombs = Mathf.Clamp( Bombs + value, 0, maxBombs);
        }
    }
    public bool HasAmmo()
    {
        return Bombs > 0;
    }
    //  Emp
    //-----------------------------------------------------------------------------------------
    public void SetDisable(float value)
    {
        if (value > 0.0f)
        {   //recieve +ve value: set empTime and EmpDisabled to TRUE;
            empTime = value;
            EmpDisabled = true;
        }
        else
        {   //receive 0 || -ve val: reset emps to false;
            empTime = 0.0f;
            EmpDisabled = false;
        }
    }
    public bool GetDisabled()
    {
        return EmpDisabled;
    }
    private void decreaseEmp()
    {
        if (EmpDisabled)
        {
            empTime -= Time.deltaTime;
            if (empTime <= 0.0f)
                EmpDisabled = false;
        }
    }


    // Pickup Functions
    //-----------------------------------------------------------------------------------------
    public void SetShieldPU()
    {
        Shield = Get(StatType.sShield);
    }
    //OBSELETE
    public bool GetShieldPUState()
    {
        return false;
    }
    public void SetFuel()
    {
        Fuel = Get(StatType.sFuel);
    }



    // Editor Utility
    //------------------------------------------------------------------------------------------
    //public List<Stat> EditorGetStatList()
    //{
        
    //    return block.statList;
    //}
}
