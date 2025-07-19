using System.Threading;
using UnityEngine;

public class IMissileStats : IWeaponStats
{
    //ctor 
    public IMissileStats (Stats _s) : base(_s) { }

    override public float Attack { get { return ShipStats.Get(StatType.mAttack); } }
    override public float AttackSpeed { get { return 1; } }
    override public float BurstAmount { get { return ShipStats.Get(StatType.mBurst); } }
    override public float ProjectileAmount { get { return ShipStats.Get(StatType.mProjectileAmount); } }
    override public float SpreadAngle { get { return ShipStats.Get(StatType.mSpreadAngle); } }
    override public float ChargeTime { get { return 0; } }
    override public float RampTime { get { return 0; } }
    override public float RampAmount { get { return 0; } }
    override public float BurnoutTime { get { return 0; } }
    override public float ReloadAmmo { get { return 0; } }
    override public float ReloadTime { get { return 0; } }
    override public float AoeDamage { get { return ShipStats.Get(StatType.mAoeDamage); } }
    override public float AoeSize { get { return ShipStats.Get(StatType.mAoeSize); } }
}
