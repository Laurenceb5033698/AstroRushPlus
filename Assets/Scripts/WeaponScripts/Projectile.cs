using UnityEngine;
using System.Collections.Generic;
using Projectiles;
using TMPro;

public class Projectile : MonoBehaviour
{
    protected ProjectileMotor motor;
    //private Collider lastHitCollider = null;
    //private List<Collider> m_PiercedTargets;

    public string ownertag;
    public GameObject psImpactPrefab;

    public BulletStats m_Stats;

    //values for range and falloff
    float m_cumulativePath = 0.0f;
    float m_falloffCurrent = 0.0f;
    float m_falloffPercent = 1.0f;
    float m_sizeModifier = 1.0f;

    bool m_ProjectileInteraction = false;
    protected virtual void Awake()
    {
        motor = GetComponent<ProjectileMotor>();
        if (!motor)
            motor = gameObject.AddComponent<ProjectileMotor>();
    }
    void Start () 
    {
        //m_PiercedTargets = new List<Collider>();

    }
    virtual public void SetupValues(string _ownerTag, Stats _setupStats)
    {
        ownertag = _ownerTag;
        m_Stats = new BulletStats();
        m_Stats.SetupValues(_setupStats, false);
        motor.Setup(_ownerTag, m_Stats);
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

    public virtual void applyImpulse(Rigidbody body)
    {
        //Vector3 direction = transform.position - body.transform.position;
        body.AddForce(transform.forward * ((m_Stats.damage / 2)+(m_Stats.speed / (2+body.mass))), ForceMode.Impulse);
    }

    protected virtual void Update () 
    {
	}

    protected virtual void FixedUpdate()
    {
        m_Stats.lifetime -= Time.deltaTime;
        if (m_Stats.lifetime < 0)
            DestroySelf();

        if(!motor)
        {
            Debug.Log("Error! Projectile: Motor is null.");
            return;
        }

        CollisionData collideData;
        motor.Motor(out collideData);

        HandleHits(collideData);

        //moved to ProjectileMotor Component

        ////max travel distance is regular speed*time
        //float travelDistance = CalcSpeed() * Time.deltaTime;
        ////do collision check, return type of collision and set travel distance if reduced.
        //CollisionData collideData;
        //DoSphereCast(travelDistance, out collideData);

        ////resolve movement
        //if (collideData.type == CollisionMode.BOUNCE)
        //{
        //    //set bullet position to correct position at point of hit.
        //    transform.position += transform.forward * collideData.hitDistance;
        //    //transform.position = _hitInfo.point + _hitInfo.normal * m_Stats.size;

        //    //new forward direction is reflected based on hit normal. only refelcts in the plane of the playfield.
        //    Vector3 horizontalReflection = Vector3.Reflect(transform.forward, collideData.hitNormal);
        //    horizontalReflection.y = 0;
        //    transform.rotation = Quaternion.LookRotation(horizontalReflection, Vector3.up);
        //}
        //else
        //{
        //    //simple movement for others
        //    transform.position += transform.forward * collideData.hitDistance;
        //}

        //check these in fixed update to reduce lag on higher framerates.
        CheckRange(collideData.hitDistance);
        BulletFalloff();
    }

    private void HandleHits(CollisionData _data)
    {
        foreach (HitData hit in _data.hitObjects)
        {
            hit.damageable.TakeDamage(transform.position, CalcDamage());
            SpawnHitVisuals(hit.hitPos);
            applyImpulse(hit.damageable.GetRigidbody());
        }
    }

    protected virtual void DestroySelf()
    {
        //SpawnHitVisuals();
        Destroy(transform.gameObject);
    }

    protected virtual void SpawnHitVisuals(Vector3 _spawnPos = new Vector3())
    {
        if (_spawnPos == Vector3.zero)
            _spawnPos = transform.position;
        Instantiate(psImpactPrefab, _spawnPos, transform.rotation);

    }

    /// <summary>
    /// performs spherecast to check collisions or triggers.
    /// Instead of using collider component for bullet hitting things. use sphere cast along path travelled.
    /// </summary>
    /// <param name="_maxDistance">The length of the raycast for this step.</param>
    //private void DoSphereCast(float _maxDistance, out CollisionData _collideData)
    //{
    //    //player, enemies, and asteroids all on default layer
    //    int layerMask = LayerMask.GetMask("Default");

    //    //if a bullet needs to detect a bullet, allow trigger on other projectiles.
    //    if (m_ProjectileInteraction)
    //        layerMask += LayerMask.GetMask("Projectiles");

    //    //return value starts as none. it is changed if any other collision type happens
    //    _collideData = new CollisionData(CollisionMode.NONE, Vector3.zero, _maxDistance);
        
    //    RaycastHit[] hitInfoArray = Physics.SphereCastAll(transform.position, m_Stats.size/2, transform.forward, _maxDistance, layerMask, QueryTriggerInteraction.Collide);
    //    foreach (RaycastHit hitInfo in hitInfoArray)
    //    {
    //        Collider c = hitInfo.collider;
    //        if (c == this.GetComponent<Collider>())
    //            continue;

    //        if (c.CompareTag(ownertag) || c == lastHitCollider || m_PiercedTargets.Contains(c))
    //        {
    //            //donot allow self-hits
    //            //donot allow multi-hits
    //            //donot allow pierce targets to be hit more than once (issue with fast moving targets, or when many targets inside large projectile)
    //            continue;
    //        }
                
    //        if (m_ProjectileInteraction)
    //        {
    //            if (c.CompareTag("bullet"))
    //            {
    //                //was a bullet
    //                //trigger bullet interaction
    //            }
    //            //donot riccochet, donot reduce pen count.
    //            //other interaction handled in bullet trigger
    //            continue;
    //        }
    //        //now try regular hit interaction
    //        Damageable damageable = c.GetComponent<Damageable>();
    //        if (!damageable)
    //            continue;

    //        damageable.TakeDamage(transform.position, CalcDamage());
    //        applyImpulse(damageable.GetRigidbody());
    //        //try riccochet
    //        if (m_Stats.riccochet > 0)
    //        {
    //            BulletBounced(c, hitInfo.point);
    //            //stop considering further hits.
    //            _collideData.type = CollisionMode.BOUNCE;
    //            _collideData.hitNormal = hitInfo.normal;
    //            _collideData.hitDistance = hitInfo.distance;

    //            break;
    //        }
    //        else
    //        {
    //            //no riccochets left, try piercing
    //            if(m_Stats.penetration > 0)
    //            {
    //                //we hit an object
    //                BulletPierced(c, hitInfo.point);
    //                _collideData.type = CollisionMode.PIERCE;
    //                //can potentially pierce multiple objects in one step.
    //                continue;
    //            }
    //            else
    //            {
    //                //regular hit, queue projectile dying
    //                m_Stats.lifetime = -1;
    //                _collideData.type = CollisionMode.NORMAL;
    //                _collideData.hitNormal = hitInfo.normal;
    //                _collideData.hitDistance = hitInfo.distance;

    //                break;
    //            }
    //        }
    //    }
    //}

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
    //private void BulletBounced(Collider _c, Vector3 _hitPos)
    //{
    //    m_Stats.riccochet--;
    //    SpawnHitVisuals(_hitPos);
    //    lastHitCollider = _c;
    //}

    /// <summary>
    /// bullet hit a solid object and penetrates
    /// </summary>
    //private void BulletPierced(Collider _c, Vector3 _hitPos)
    //{
    //    m_Stats.penetration--;
    //    m_PiercedTargets.Add(_c);
    //    SpawnHitVisuals(_hitPos);
    //    lastHitCollider = _c;
    //}

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
