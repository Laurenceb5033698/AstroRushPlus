using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //Ships stats which can now be modified via upgrades etc
    public Stat Health = new Stat();
    public Stat Shield = new Stat();
    public Stat Attack = new Stat();    //Attack is new, should update arsenal
    public Stat Special = new Stat();   //replaces boost, can now be uesd to augment ability intensity
    public Stat Speed = new Stat();     //replaces thrust, can now upgrade base speed
    public Stat Fuel = new Stat();      //replaces fuel, can now increase amount of fuel for ability

    [SerializeField]
    private float RotationSpeed = 350f; //used by AICore

    [SerializeField]
    private bool GodMode = false;
    //private bool inCombat = false;
    private bool shieldPowerUp = false;


    [SerializeField] private bool EmpDisabled = false;
    private float empTime = 0.0f;   //time in seconds


    //  Unity Functions
    private void Awake()
    {
        //  Recalculate stat max & values from values set in editor
        RecalculateStats();
    }

    private void Start()
    {
    }

    private void Update()
    {
        decreaseEmp();
    }

    public void RecalculateStats()
    {
        Health.Recalculate();
        Shield.Recalculate();
        Attack.Recalculate();
        Special.Recalculate();
        Speed.Recalculate();
        Fuel.Recalculate();
    }

    //  Functions
    //  Health
    //-----------------------------------------------------------------------------------------
    public void TakeDamage(int val)
    {
        if (!GodMode)
        {
            //inCombat = true;
            if (ShipShield > 0) //if we have shields
                if (Shield.Value - val > 0)   //and the damage taken is lower than sheild health
                    ShipShield = -val;   //do damage to shield
                else
                {//otherwise split damage between shield and health
                    ShipHealth = (Shield.Value - val);   //remaining damage is delt to health; (shield-val) here will always be negative
                    ShipShield = -Shield.Value;        //and shield is set to 0
                }
            else
                ShipHealth = -val;   //shield is at 0, so deal damage directly to health
        }
    }

    public int ShipHealth
    {
        get { return Health.Value; }
        set //can be set using powerups
        {
            if (value > 0)
            {
                Health.Value = (Health.Value + value > Health.Max) ? Health.Max : Health.Value + value;
            }
            else if (value < 0)
            {
                Health.Value = (Health.Value + value < 0) ? 0 : Health.Value + value;
            }
        }
    }

    public int ShipShield
    {
        get { return Shield.Value; }
        set //can be set using powerups
        {
            if (value > 0)
            {
                Shield.Value = (Shield.Value + value > Shield.Max) ? Shield.Max : Shield.Value + value;
            }
            else if (value < 0)
            {
                Shield.Value = (Shield.Value + value < 0) ? 0 : Shield.Value + value;
            }
        }
    }

    public bool IsAlive()
    {
        return (Health.Value > 0);
    }

    public float GetHealth()
    {
        return Health.Value;
    }
    public void SetHealth()
    {
        ShipHealth = Health.Max;
    }
    public float GetShieldMax()
    {
        return Shield.Max;
    }


    //  Speed & movement
    //-----------------------------------------------------------------------------------------
    public int ShipFuel
    {
        get { return Fuel.Value; }
        set
        {
            if (value > 0)
            {
                Fuel.Value = (Fuel.Value + value > 100.0f) ? Fuel.Max : Fuel.Value + value;
            }
            else if (value < 0)
            {
                Fuel.Value = (Fuel.Value + value < 0.0f) ? 0 : Fuel.Value + value;
            }
        }
    }
    public float GetMainThrust()
    {
        return Speed.Value;
    }
    public float GetRotSpeed()
    {
        return RotationSpeed;
    }

    //  Abillity / boost power
    public float GetSpecial()
    {
        return Special.Value;
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
    public void ActivateShieldPU()
    {
        if (shieldPowerUp)
        {
            Shield.Value = Shield.Max;
            shieldPowerUp = false;
        }
    }
    public void SetShieldPU()
    {
        ActivateShieldPU(); // if have one already, use it before getting the next one
        shieldPowerUp = true;
    }
    public bool GetShieldPUState()
    {
        return shieldPowerUp;
    }
    public void SetFuel()
    {
        Fuel.Value = Fuel.Max;
    }

}
