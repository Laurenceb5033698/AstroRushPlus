using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Rendering;
using System.Security.Cryptography;
using UnityEngine.Analytics;
using UnityEditor.PackageManager;

[System.Serializable]
public struct BulletStats
{
    public float damage;
    public float speed;
    public float acceleration;
    public float range;
    public float lifetime;
    public float magnet;
    public int   penetration;
    public int   riccochet;
    public float size;
    public float falloff;
    //other bools might be needed here.

    //ctor from stats.
    public BulletStats(Stats _stats)
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
        falloff = Mathf.Max(1,_stats.Get(StatType.bFalloff));
    }
    public BulletStats(Stats _stats, Rigidbody _rb)
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
    }
}

public class Projectile : MonoBehaviour
{

    enum CollisionMode {
        NONE,
        PIERCE,
        NORMAL,
        BOUNCE
    }
    struct CollisionData {
        public CollisionMode type;
        public Vector3 hitNormal;
        public float hitDistance;
        public CollisionData (CollisionMode _m, Vector3 _n, float _d)
        {
            type = _m;
            hitNormal = _n;
            hitDistance = _d;
        }
    }

    private Collider lastHitCollider = null;

    public string ownertag;
    public GameObject psImpactPrefab;

    public BulletStats m_Stats;

    //values for range and falloff
    Vector3 m_PreviousPos;
    float m_cumulativePath = 0.0f;
    float m_falloffCurrent = 0.0f;
    float m_falloffPercent = 1.0f;
    float m_sizeModifier = 1.0f;

    bool m_ProjectileInteraction = false;

    void Start () {
        //begin recording path for range.
        m_PreviousPos = transform.position;
	}
    virtual public void SetupValues(string _ownerTag, Stats _setupStats)
    {
        ownertag = _ownerTag;
        m_Stats = new BulletStats(_setupStats);
    }

    //protected virtual void OnTriggerEnter(Collider _other)
    //{

    //    if ((_other != null) && (_other.gameObject.tag != ownertag) && (!_other.gameObject.CompareTag("bullet")))
    //    {//successful collision that wasnt with shooter
    //        //Debug.Log("other Entity: " + other.gameObject.tag);
    //        bool hit = false;
    //        Damageable otherDamagable = _other.GetComponent<Damageable>();
    //        if (otherDamagable)
    //        {
    //            otherDamagable.TakeDamage(transform.position, CalcDamage());
    //            applyImpulse(otherDamagable.GetRigidbody());
    //            hit = true;
    //        }

    //        //case: other is not damageable but can be hit anyway?
    //        //Debug.Log("Unknown entity. " + other.gameObject.tag);
            
    //        if (hit)
    //        {
    //            //reduce riccochet first
    //            if(m_Stats.riccochet > 0)
    //            { 
    //                m_Stats.riccochet--;
    //                SpawnHitVisuals();  //hit visuals for each rccochet
                    
    //            }
    //            else
    //            {
    //                if(m_Stats.penetration > 0)
    //                {
    //                    m_Stats.penetration--;
    //                    SpawnHitVisuals();
    //                }
    //                else
    //                {
    //                    //out of riccochet and pen, queue destroy.
    //                    m_Stats.lifetime = -1;
    //                }
    //            }
    //        }
    //    }
    //}

    protected virtual void applyImpulse(Rigidbody body)
    {
        //Vector3 direction = transform.position - body.transform.position;
        body.AddForce(transform.forward * ((m_Stats.damage / 2)+(m_Stats.speed / (2+body.mass))), ForceMode.Impulse);
    }

    // Update is called once per frame
    protected virtual void Update () {

        
        
	}

    private void FixedUpdate()
    {
        m_Stats.lifetime -= Time.deltaTime;
        if (m_Stats.lifetime < 0)
            DestroySelf();

        //max travel distance is regular speed*time
        float travelDistance = CalcSpeed() * Time.deltaTime;
        //do collision check, return type of collision and set travel distance if reduced.
        CollisionData collideData;
        DoSphereCast(travelDistance, out collideData);

        //resolve movement
        if (collideData.type == CollisionMode.BOUNCE)
        {
            //set bullet position to correct position at point of hit.
            transform.position += transform.forward * collideData.hitDistance;
            //transform.position = _hitInfo.point + _hitInfo.normal * m_Stats.size;

            //new forward direction is reflected based on hit normal. only refelcts in the plane of the playfield.
            Vector3 horizontalReflection = Vector3.Reflect(transform.forward, collideData.hitNormal);
            horizontalReflection.y = 0;
            transform.rotation = Quaternion.LookRotation(horizontalReflection, Vector3.up);
        }
        else
        {
            //simple movement for others
            transform.position += transform.forward * collideData.hitDistance;
        }

        //check these in fixed update to reduce lag on higher framerates.
        CheckRange(collideData.hitDistance);
        BulletFalloff();
    }

    protected virtual void DestroySelf()
    {
        SpawnHitVisuals();
        Destroy(transform.gameObject);
    }

    protected virtual void SpawnHitVisuals()
    {
        Instantiate(psImpactPrefab, transform.position, transform.rotation);

    }

    /// <summary>
    /// performs spherecast to check collisions or triggers.
    /// Instead of using collider component for bullet hitting things. use sphere cast along path travelled.
    /// </summary>
    /// <param name="_maxDistance">The length of the raycast for this step.</param>
    private void DoSphereCast(float _maxDistance, out CollisionData _collideData)
    {
        //player, enemies, and asteroids all on default layer
        int layerMask = LayerMask.GetMask("Default");

        //if a bullet needs to detect a bullet, allow trigger on other projectiles.
        if (m_ProjectileInteraction)
            layerMask += LayerMask.GetMask("Projectiles");

        //return value starts as none. it is changed if any other collision type happens
        _collideData = new CollisionData(CollisionMode.NONE, Vector3.zero, _maxDistance);
        
        RaycastHit[] hitInfoArray = Physics.SphereCastAll(transform.position, m_Stats.size/2, transform.forward, _maxDistance, layerMask, QueryTriggerInteraction.Collide);
        foreach (RaycastHit hitInfo in hitInfoArray)
        {
            Collider c = hitInfo.collider;
            if (c.CompareTag(ownertag) || c == lastHitCollider)
            {
                //donot allow self-hits
                //donot allow multi-hits
                continue;
            }
                
            if (m_ProjectileInteraction)
            {
                if (c.CompareTag("bullet"))
                {
                    //was a bullet
                    //trigger bullet interaction
                }
                //donot riccochet, donot reduce pen count.
                //other interaction handled in bullet trigger
                continue;
            }
            //now try regular hit interaction
            Damageable damageable = c.GetComponent<Damageable>();
            if (!damageable)
                continue;

            damageable.TakeDamage(transform.position, CalcDamage());
            applyImpulse(damageable.GetRigidbody());
            //try riccochet
            if (m_Stats.riccochet > 0)
            {
                BulletBounced(c);
                //stop considering further hits.
                _collideData.type = CollisionMode.BOUNCE;
                _collideData.hitNormal = hitInfo.normal;
                _collideData.hitDistance = hitInfo.distance;

                break;
            }
            else
            {
                //no riccochets left, try piercing
                if(m_Stats.penetration > 0)
                {
                    //we hit an object
                    BulletPierced(c);
                    _collideData.type = CollisionMode.BOUNCE;
                    //can potentially pierce multiple objects in one step.
                    continue;
                }
                else
                {
                    //regular hit, queue projectile dying
                    m_Stats.lifetime = -1;
                    _collideData.type = CollisionMode.BOUNCE;
                    _collideData.hitNormal = hitInfo.normal;
                    _collideData.hitDistance = hitInfo.distance;

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Bullets have an effective range.
    /// It can travel upto that range as fast as it likes, then must falloff.
    /// Range can be reached independantly from lifetime.
    /// </summary>
    /// <param name="_segmentDistance"></param>
    private void CheckRange(float _segmentDistance)
    {
        m_cumulativePath += _segmentDistance;
        if(m_cumulativePath > m_Stats.range)
        {
            //add current overshoot to falloff
            m_falloffCurrent += _segmentDistance;
            //percent scales from 1 -> 0 as falloffcurrent increases.
            m_falloffPercent = Mathf.Clamp((1 - ( m_falloffCurrent / m_Stats.falloff)), 0, 1);
        }
    }
        
    private void BulletFalloff()
    {
        if (m_falloffCurrent >= (m_Stats.falloff - 0.01f))
        {
            //current falloff is over max falloff. queue deletion.
            m_Stats.lifetime = -1;
            return;
        }
        //when bullet falloff increases, it reduces bullet size, speed and damage
        float uniformScale = m_Stats.size* (m_falloffPercent);
        transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
    }


    /// <summary>
    /// bullet hit a solid object and needs to bounce.
    /// </summary>
    private void BulletBounced(Collider _c)
    {
        m_Stats.riccochet--;
        SpawnHitVisuals();
        lastHitCollider = _c;
    }

    /// <summary>
    /// bullet hit a solid object and penetrates
    /// </summary>
    private void BulletPierced(Collider _c)
    {
        m_Stats.penetration--;
        SpawnHitVisuals();
        lastHitCollider = _c;
    }

    virtual protected float CalcDamage()
    {
        //falloff percent = 1 when falloffcurrent = 0
        float damageModified = m_Stats.damage * m_falloffPercent;
        return damageModified;
    }

    virtual protected float CalcSpeed()
    {
        //falloff percent = 1 when falloffcurrent = 0
        float speedModified = m_Stats.speed;
        return speedModified;
    }

}
