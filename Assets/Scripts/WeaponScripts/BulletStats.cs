using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BulletStats
{
    //stat values used in projectile mechanics
    public float damage;
    public float speed;
    public float acceleration;
    public float range;
    public float lifetime;
    public float magnet;
    public int penetration;
    public int riccochet;
    public float size;
    public float falloff;
    //missile specific
    public float aoeRadius;
    public float aoeDamage;
    //other bools might be needed here.


    public void SetupValues(Stats _stats, bool _isMissile) 
    {
        if (!_isMissile)
        {
            SetupBullet(_stats);
        }
        else
        {
            SetupMissile(_stats);
        }
    }

    //ctor from stats.
    public void SetupBullet(Stats _stats)
    {
        damage = _stats.Get(StatType.gAttack);
        speed = _stats.Get(StatType.bSpeed);
        acceleration = _stats.Get(StatType.bAcceleration);
        range = _stats.Get(StatType.bRange);
        lifetime = _stats.Get(StatType.bLifetime);
        magnet = _stats.Get(StatType.bMagentPower);
        penetration = (int)_stats.Get(StatType.bPenetrationAmount);
        riccochet = (int)_stats.Get(StatType.bRicochetAmount);
        size = _stats.Get(StatType.bSize);
        falloff = Mathf.Max(1, _stats.Get(StatType.bFalloff));
    }
    public void SetupMissile(Stats _stats)
    {   //missile type projectile
        damage = _stats.Get(StatType.mAttack);
        speed = _stats.Get(StatType.mSpeed);
        acceleration = _stats.Get(StatType.mAcceleration);
        range = _stats.Get(StatType.mRange);
        lifetime = _stats.Get(StatType.mLifetime);
        magnet = _stats.Get(StatType.mMagnetPower);
        penetration = 0;
        riccochet = 0;
        size = _stats.Get(StatType.mSize);
        falloff = 1;
        aoeRadius = _stats.Get(StatType.mAoeSize);
        aoeDamage = _stats.Get(StatType.mAoeDamage);
    }
}
