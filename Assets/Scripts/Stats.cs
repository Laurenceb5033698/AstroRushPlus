using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

/// <summary>
/// Holds all the stats relevant to a ship.
/// Each stat can be modified by upgrades.
/// </summary>
public class Stats : MonoBehaviour
{
    public StatBlock block = new StatBlock();

    //Internal Current values for stats that have current and max. Get using properties ShipHealth, ShipShield, ShipFuel
    //generally speaking these are important values.
    [SerializeField] private float Health;
    [SerializeField] private float Shield;
    [SerializeField] private float Fuel;
    [SerializeField] private int Bombs;

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

        //set current values from each respective max
        Health = Get(StatType.sHealth);
        Shield = Get(StatType.sShield);
        Fuel = Get(StatType.sFuel);
        Bombs = Mathf.FloorToInt( Get(StatType.mAmmo));
    }

    private void Start()
    {
    }

    private void Update()
    {
        decreaseEmp();

    }

    public float Get(StatType _type)
    {
        return block.Get(_type).Get();
    }

    //  Functions
    public void TakeDamage(float val)
    {
        if (!GodMode)
        {
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
        }
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
