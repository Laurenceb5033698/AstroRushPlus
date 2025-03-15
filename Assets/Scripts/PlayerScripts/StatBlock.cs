using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// encapsulates all stats used for ships for use in shipStats and upgrade stats.
/// </summary>
[System.Serializable]
public class StatBlock
{
    [SerializeField]
    private List<Stat> statList;
    //ship stats                        // suggested defaults for new ship. stats start uninitiallised for upgrade modules.
    public Stat sHealth = new Stat();    //100
    public Stat sShield = new Stat();    //100
    public Stat sSpecial = new Stat();   //100   //replaces boost, can now be uesd to augment ability intensity
    public Stat sSpeed = new Stat();      //50     //replaces thrust, can now upgrade base speed
    public Stat sFuel = new Stat();      //100         //replaces fuel, can now increase amount of fuel for ability
    public Stat sTurnrate = new Stat();  //1500
    public Stat sHealthRegen = new Stat(); //0
    public Stat sShieldRegen = new Stat(); //0

    //Weapon stats
    public Stat gAttack = new Stat();     //10
    public Stat gAttackspeed = new Stat(); //1     //number of shots fired per second
    public Stat gProjectileAmount = new Stat();    //1
    public Stat gSpreadAngle = new Stat(); //30
    public Stat gReloadTime = new Stat();  //0
    public Stat gReloadAmmo = new Stat();  //0
    public Stat gRampAmount = new Stat();  //0
    public Stat gRampTime = new Stat();    //0
    public Stat gBurnoutTime = new Stat(); //0
    public Stat gChargeTime = new Stat();  //0
    public Stat gBurstAmount = new Stat(); //0
    public Stat gAoeDamage = new Stat();   //0
    public Stat gAoeSize = new Stat();     //0
    public Stat gDotDamage = new Stat();    //0
    public Stat gDotDuration = new Stat();  //0

    //bullet stats
    public Stat bSpeed = new Stat();          //80
    public Stat bAcceleration = new Stat();    //0
    public Stat bRange = new Stat();           //0
    public Stat bLifetime = new Stat();        //5
    public Stat bMagentPower = new Stat();     //0
    public Stat bPenetrationAmount = new Stat();   //0
    public Stat bRicochetAmount = new Stat();  //0
    public Stat bSize = new Stat();            //0
    public Stat bFalloff = new Stat();          //0


    //missile weapon stats
    public Stat mAttack = new Stat();        //500
    public Stat mAmmo = new Stat();            //3
    public Stat mBurst = new Stat();           //0
    public Stat mProjectileAmount = new Stat();    //1
    public Stat mSpreadAngle = new Stat();     //0
    public Stat mAoeDamage = new Stat();       //0
    public Stat mAoeSize = new Stat();         //0
    public Stat mDotDamage = new Stat();       //0
    public Stat mDotDuration = new Stat();     //0

    //missile bullet stats
    public Stat mSpeed = new Stat();          //50
    public Stat mAcceleration = new Stat();    //0
    public Stat mRange = new Stat();         //100
    public Stat mLifetime = new Stat();       //10
    public Stat mMagnetPower = new Stat();     //0
    public Stat mSize = new Stat();            //0

    //ctor
    public StatBlock()
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
}
