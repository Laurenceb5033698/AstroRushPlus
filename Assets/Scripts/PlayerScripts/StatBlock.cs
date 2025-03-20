using System;
using System.Collections.Generic;
using UnityEngine;

//must mirror stat types found in statblock
public enum StatType
{
    sHealth, sShield, sSpecial, sSpeed, sFuel, sTurnrate, sHealthRegen, sShieldRegen,
    gAttack, gAttackspeed, gProjectileAmount, gSpreadAngle, gReloadAmmo, gReloadTime, gRampAmount, gRampTime, gBurnoutTime, gChargeTime, gBurstAmount, gAoeDamage, gAoeSize, gDotDamage, gDotDuration,
    bSpeed, bAcceleration, bRange, bLifetime, bMagentPower, bPenetrationAmount, bRicochetAmount, bSize, bFalloff,
    mAttack, mAmmo, mBurst, mProjectileAmount, mSpreadAngle, mAoeDamage, mAoeSize, mDotDamage, mDotDuration,
    mSpeed, mAcceleration, mRange, mLifetime, mMagnetPower, mSize,
    NUM
};
/// <summary>
/// encapsulates all stats used for ships for use in shipStats and upgrade stats.
/// </summary>
[System.Serializable]
public class StatBlock
{
    [SerializeField]
    public List<Stat> statList;
    //ship stats                        // suggested defaults for new ship. stats start uninitiallised for upgrade modules.
    //public Stat sHealth = new Stat(StatType.sHealth);    //100
    //public Stat sShield = new Stat(StatType.sShield);    //100
    //public Stat sSpecial = new Stat(StatType.sSpecial);   //100   //replaces boost, can now be uesd to augment ability intensity
    //public Stat sSpeed = new Stat(StatType.sSpeed);      //50     //replaces thrust, can now upgrade base speed
    //public Stat sFuel = new Stat(StatType.sFuel);      //100         //replaces fuel, can now increase amount of fuel for ability
    //public Stat sTurnrate = new Stat(StatType.sTurnrate);  //1500
    //public Stat sHealthRegen = new Stat(StatType.sHealthRegen); //0
    //public Stat sShieldRegen = new Stat(StatType.sShieldRegen); //0

    ////Weapon stats
    //public Stat gAttack = new Stat(StatType.gAttack);     //10
    //public Stat gAttackspeed = new Stat(StatType.gAttackspeed); //1     //number of shots fired per second
    //public Stat gProjectileAmount = new Stat(StatType.gProjectileAmount);    //1
    //public Stat gSpreadAngle = new Stat(StatType.gSpreadAngle); //30
    //public Stat gReloadTime = new Stat(StatType.gReloadTime);  //0
    //public Stat gReloadAmmo = new Stat(StatType.gReloadAmmo);  //0
    //public Stat gRampAmount = new Stat(StatType.gRampAmount);  //0
    //public Stat gRampTime = new Stat(StatType.gRampTime);    //0
    //public Stat gBurnoutTime = new Stat(StatType.gBurnoutTime); //0
    //public Stat gChargeTime = new Stat(StatType.gChargeTime);  //0
    //public Stat gBurstAmount = new Stat(StatType.gBurstAmount); //0
    //public Stat gAoeDamage = new Stat(StatType.gAoeDamage);   //0
    //public Stat gAoeSize = new Stat(StatType.gAoeSize);     //0
    //public Stat gDotDamage = new Stat(StatType.gDotDamage);    //0
    //public Stat gDotDuration = new Stat(StatType.gDotDuration);  //0

    ////bullet stats
    //public Stat bSpeed = new Stat(StatType.bSpeed);          //80
    //public Stat bAcceleration = new Stat(StatType.bAcceleration);    //0
    //public Stat bRange = new Stat(StatType.bRange);           //0
    //public Stat bLifetime = new Stat(StatType.bLifetime);        //5
    //public Stat bMagentPower = new Stat(StatType.bMagentPower);     //0
    //public Stat bPenetrationAmount = new Stat(StatType.bPenetrationAmount);   //0
    //public Stat bRicochetAmount = new Stat(StatType.bRicochetAmount);  //0
    //public Stat bSize = new Stat(StatType.bSize);            //0
    //public Stat bFalloff = new Stat(StatType.bFalloff);          //0


    ////missile weapon stats
    //public Stat mAttack = new Stat(StatType.mAttack);        //500
    //public Stat mAmmo = new Stat(StatType.mAmmo);            //3
    //public Stat mBurst = new Stat(StatType.mBurst);           //0
    //public Stat mProjectileAmount = new Stat(StatType.mProjectileAmount);    //1
    //public Stat mSpreadAngle = new Stat(StatType.mSpreadAngle);     //0
    //public Stat mAoeDamage = new Stat(StatType.mAoeDamage);       //0
    //public Stat mAoeSize = new Stat(StatType.mAoeSize);         //0
    //public Stat mDotDamage = new Stat(StatType.mDotDamage);       //0
    //public Stat mDotDuration = new Stat(StatType.mDotDuration);     //0

    ////missile bullet stats
    //public Stat mSpeed = new Stat(StatType.mSpeed);          //50
    //public Stat mAcceleration = new Stat(StatType.mAcceleration);    //0
    //public Stat mRange = new Stat(StatType.mRange);         //100
    //public Stat mLifetime = new Stat(StatType.mLifetime);       //10
    //public Stat mMagnetPower = new Stat(StatType.mMagnetPower);     //0
    //public Stat mSize = new Stat(StatType.mSize);            //0

    //ctor
    //public StatBlock()
    //{
    //    //once declared and initialised, can group stats.
    //    //add all stats to statlist. allows performing functions over all stats easily.
    //    statList = new List<Stat>(){
    //        sHealth, sShield, sSpecial, sSpeed, sFuel, sTurnrate, sHealthRegen, sShieldRegen,
    //        gAttack,gAttackspeed,gProjectileAmount,gSpreadAngle,gReloadAmmo,gReloadTime,gRampAmount,gRampTime,gBurnoutTime,gChargeTime,gBurstAmount,gAoeDamage,gAoeSize,gDotDamage,gDotDuration,
    //        bSpeed, bAcceleration, bRange, bLifetime, bMagentPower,bPenetrationAmount,bRicochetAmount,bSize,bFalloff,
    //        mAttack,mAmmo,mBurst,mProjectileAmount,mSpreadAngle,mAoeDamage,mAoeSize,mDotDamage,mDotDuration,
    //        mSpeed,mAcceleration,mRange,mLifetime,mMagnetPower,mSize
    //    };
    //}

    public StatBlock()
    {
        //initialises all stats for Statblock.
        //i think this happens before inspector values are loaded.
        statList = new List<Stat>();
        for (int i = 0; i < (int)StatType.NUM; i++)
        {
            statList.Add(new Stat((StatType)i));
        }
    }
    
    public Stat Get(StatType _type)
    {
        return statList[(int)_type];
    }

    /// <summary>
    /// Causes all stats to calculate max. required for initialising block max values during Start.
    /// </summary>
    public void RecalculateStats()
    {
        foreach (Stat stat in statList)
        {
            stat.Recalculate();
        }
    }

    public void ProcessStats(Func<List<Stat>,bool> func)
    {
        if (func(statList)) 
        {
            Debug.Log("Upgrade applied successfully.");
        }
        else
        {
            Debug.Log("Upgrade failed.");
        }
    }
}
