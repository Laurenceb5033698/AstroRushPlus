using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all the stats relevant to a ship.
/// Each stat can be modified by upgrades.
/// </summary>
public class Stats : MonoBehaviour
{
    private List<Stat> statList;
    //ship stats
    public Stat sHealth = new Stat(100);
    public Stat sShield = new Stat(100);
    public Stat sSpecial = new Stat(100);   //replaces boost, can now be uesd to augment ability intensity
    public Stat sSpeed = new Stat(50);     //replaces thrust, can now upgrade base speed
    public Stat sFuel = new Stat(100);         //replaces fuel, can now increase amount of fuel for ability
    public Stat sTurnrate = new Stat(350);
    public Stat sHealthRegen = new Stat(0);
    public Stat sShieldRegen = new Stat(0);

    //Weapon stats
    public Stat gAttack = new Stat(10);
    public Stat gAttackspeed = new Stat(1);
    public Stat gProjectileAmount = new Stat(1);
    public Stat gSpreadAngle = new Stat(0);
    public Stat gReloadTime = new Stat(0);
    public Stat gReloadAmmo = new Stat(0);
    public Stat gRampAmount = new Stat(0);
    public Stat gRampTime = new Stat(0);
    public Stat gBurnoutTime = new Stat(0);
    public Stat gChargeTime = new Stat(0);
    public Stat gBurstAmount = new Stat(0);
    public Stat gAoeDamage = new Stat(0);
    public Stat gAoeSize = new Stat(0);
    public Stat gDotDamage= new Stat(0);
    public Stat gDotDuration= new Stat(0);

    //bullet stats
    public Stat bSpeed = new Stat(20);
    public Stat bAcceleration = new Stat(0);
    public Stat bRange = new Stat(0);
    public Stat bLifetime = new Stat(5);
    public Stat bMagentPower = new Stat(0);
    public Stat bPenetrationAmount = new Stat(0);
    public Stat bRicochetAmount = new Stat(0);
    public Stat bSize = new Stat(1);
    public Stat bFalloff= new Stat(0);


    //missile weapon stats
    public Stat mAttack = new Stat(500);
    public Stat mAmmo = new Stat(3);
    public Stat mBurst = new Stat(0);
    public Stat mProjectileAmount = new Stat(1);
    public Stat mSpreadAngle = new Stat(0);
    public Stat mAoeDamage = new Stat(0);
    public Stat mAoeSize = new Stat(0);
    public Stat mDotDamage = new Stat(0);
    public Stat mDotDuration = new Stat(0);

    //missile bullet stats
    public Stat mSpeed = new Stat(20);
    public Stat mAcceleration = new Stat(0);
    public Stat mRange = new Stat(100);
    public Stat mLifetime = new Stat(10);
    public Stat mMagnetPower = new Stat(0);
    public Stat mSize = new Stat(1);


    //misc stats
    [SerializeField]
    private float RotationSpeed = 350f; //used by AICore

    //Status effects
    [SerializeField]
    private bool GodMode = false;
    //private bool inCombat = false;

    [SerializeField] private bool EmpDisabled = false;
    private float empTime = 0.0f;   //time in seconds

    //  Unity Functions
    private void Awake()
    {
        //once declared and initialised, can group stats.
        //add all stats to statlist. allows performing functions over all stats easily.
        statList = new List<Stat>(){
            sHealth, sShield, sSpecial, sSpeed, sFuel, sTurnrate, sHealthRegen, sShieldRegen,
            gAttack,gAttackspeed,gProjectileAmount,gSpreadAngle,gReloadAmmo,gReloadTime,gRampAmount,gRampTime,gBurnoutTime,gChargeTime,gBurstAmount,gAoeDamage,gAoeSize,gDotDamage,gDotDuration,
            bSpeed, bAcceleration, bRange, bLifetime, bMagentPower,bPenetrationAmount,bRicochetAmount,bSize,bFalloff,
            mAttack,mAmmo,mBurst,mProjectileAmount,mSpreadAngle,mAoeDamage,mAoeSize,mDotDamage,mDotDuration,
            mSpeed,mAcceleration,mRange,mLifetime,mMagnetPower,mSize
        };
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
        foreach (Stat stat in statList)
        {
            stat.Recalculate();
        }
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
                if (sShield.Value - val > 0)   //and the damage taken is lower than sheild health
                    ShipShield = -val;   //do damage to shield
                else
                {//otherwise split damage between shield and health
                    ShipHealth = (sShield.Value - val);   //remaining damage is delt to health; (shield-val) here will always be negative
                    ShipShield = -sShield.Value;        //and shield is set to 0
                }
            else
                ShipHealth = -val;   //shield is at 0, so deal damage directly to health
        }
    }

    public float ShipHealth
    {
        get { return sHealth.Value; }
        set //can be set using powerups
        {
            if (value > 0)
            {
                sHealth.Value = (sHealth.Value + value > sHealth.Max) ? sHealth.Max : sHealth.Value + value;
            }
            else if (value < 0)
            {
                sHealth.Value = (sHealth.Value + value < 0) ? 0 : sHealth.Value + value;
            }
        }
    }

    public float ShipShield
    {
        get { return sShield.Value; }
        set //can be set using powerups
        {
            if (value > 0)
            {
                sShield.Value = (sShield.Value + value > sShield.Max) ? sShield.Max : sShield.Value + value;
            }
            else if (value < 0)
            {
                sShield.Value = (sShield.Value + value < 0) ? 0 : sShield.Value + value;
            }
        }
    }

    public bool IsAlive()
    {
        return (sHealth.Value > 0);
    }

    public float GetHealth()
    {
        return sHealth.Value;
    }
    public void SetHealth()
    {
        ShipHealth = sHealth.Max;
    }
    public float GetShieldMax()
    {
        return sShield.Max;
    }


    //  Speed & movement
    //-----------------------------------------------------------------------------------------
    public float ShipFuel
    {
        get { return sFuel.Value; }
        set
        {
            if (value > 0)
            {
                sFuel.Value = (sFuel.Value + value > 100.0f) ? sFuel.Max : sFuel.Value + value;
            }
            else if (value < 0)
            {
                sFuel.Value = (sFuel.Value + value < 0.0f) ? 0 : sFuel.Value + value;
            }
        }
    }
    public float GetMainThrust()
    {
        return sSpeed.Value;
    }
    public float GetRotSpeed()
    {
        return sTurnrate.Value;
    }

    //  Abillity / boost power
    public float GetSpecial()
    {
        return sSpecial.Value;
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
        sShield.Value = sShield.Max;
    }
    //OBSELETE
    public bool GetShieldPUState()
    {
        return false;
    }
    public void SetFuel()
    {
        sFuel.Value = sFuel.Max;
    }

}
