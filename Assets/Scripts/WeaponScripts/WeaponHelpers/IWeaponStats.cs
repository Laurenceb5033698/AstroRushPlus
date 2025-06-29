using UnityEngine;


/// <summary>
/// Interface for getting stats for use in weapons classes.
/// can be overriden for missile stats.
/// </summary>
public class IWeaponStats
{
    protected Stats ShipStats;

    //ctor
    public IWeaponStats (Stats _s)
    {
        ShipStats = _s;
    }

    virtual public float AttackSpeed { get { return ShipStats.Get(StatType.gAttackspeed); } }
    virtual public float BurstAmount {  get { return ShipStats.Get(StatType.gBurstAmount); } }
    virtual public float ProjectileAmount { get { return ShipStats.Get(StatType.gProjectileAmount); } }
    virtual public float SpreadAngle { get { return ShipStats.Get(StatType.gSpreadAngle); } }
    virtual public float ChargeTime { get { return ShipStats.Get(StatType.gChargeTime); } }
    virtual public float RampTime { get { return ShipStats.Get(StatType.gRampTime); } }
    virtual public float RampAmount { get { return ShipStats.Get(StatType.gRampAmount); } }
    virtual public float BurnoutTime { get { return ShipStats.Get(StatType.gBurnoutTime); } }
    virtual public float ReloadAmmo { get { return ShipStats.Get(StatType.gReloadAmmo); } }
    virtual public float ReloadTime { get { return ShipStats.Get(StatType.gReloadTime); } }
}
