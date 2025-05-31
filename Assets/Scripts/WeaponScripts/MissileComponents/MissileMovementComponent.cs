using UnityEngine;
using Projectiles;
using System.Collections.Generic;

public class MissileMovementComponent : ProjectileMotor
{
    //missile movement differs from regular projectiles movement.
    //missiles move by acceleration in a direction, which means speed and velocity change
    //missile's mSpeed stat governs its top speed, and mAccel governs the change in speed in the forward direction. 

    Vector3 m_Velocity;

    public override void Setup(string _tag, BulletStats _mStats)
    {
        base.Setup(_tag, _mStats);

        //set forwards velocity to maximum.
        m_Velocity = this.transform.forward * bulletStats.speed;
    }

    public override void Motor(out CollisionData _collideData)
    {
        //max travel distance is regular speed*time
        float travelDistance = CalcSpeed() * Time.deltaTime;
        //do collision check, return type of collision and set travel distance if reduced.
        
        DoSphereCast(travelDistance, out _collideData);

        //resolve movement
        if (_collideData.type == CollisionMode.BOUNCE)
        {
            //set bullet position to correct position at point of hit.
            transform.position += m_Velocity.normalized * _collideData.hitDistance;
            //transform.position = _hitInfo.point + _hitInfo.normal * m_Stats.size;

            //new forward direction is reflected based on hit normal. only refelcts in the plane of the playfield.
            Vector3 horizontalReflection = Vector3.Reflect(transform.forward, _collideData.hitNormal);
            horizontalReflection.y = 0;
            transform.rotation = Quaternion.LookRotation(horizontalReflection, Vector3.up);
        }
        else
        {
            //simple movement for others
            transform.position += m_Velocity.normalized * _collideData.hitDistance;
        }

        //if hits, return with objects set.
        //_hitObjects = collideData.hitObjects;
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
        _collideData = new CollisionData(CollisionMode.NONE, Vector3.zero, _maxDistance, new List<HitData>());

        RaycastHit[] hitInfoArray = Physics.SphereCastAll(transform.position, bulletStats.size / 2, transform.forward, _maxDistance, layerMask, QueryTriggerInteraction.Collide);
        foreach (RaycastHit hitInfo in hitInfoArray)
        {
            Collider c = hitInfo.collider;
            if (c == this.GetComponent<Collider>())
                continue;

            if (c.CompareTag(ownertag) || c == lastHitCollider || m_PiercedTargets.Contains(c))
            {
                //donot allow self-hits
                //donot allow multi-hits
                //donot allow pierce targets to be hit more than once (issue with fast moving targets, or when many targets inside large projectile)
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

            //try riccochet
            if (bulletStats.riccochet > 0)
            {
                BulletBounced(c);
                //stop considering further hits.
                _collideData.type = CollisionMode.BOUNCE;
                _collideData.hitNormal = hitInfo.normal;
                _collideData.hitDistance = hitInfo.distance;
                _collideData.hitObjects.Add(new HitData(damageable, hitInfo.point));
                break;
            }
            else
            {
                //no riccochets left, try piercing
                if (bulletStats.penetration > 0)
                {
                    //we hit an object
                    BulletPierced(c);
                    _collideData.type = CollisionMode.PIERCE;
                    _collideData.hitObjects.Add(new HitData(damageable, hitInfo.point));

                    //can potentially pierce multiple objects in one step.
                    continue;
                }
                else
                {
                    //regular hit, queue projectile dying
                    bulletStats.lifetime = -1;
                    _collideData.type = CollisionMode.NORMAL;
                    _collideData.hitNormal = hitInfo.normal;
                    _collideData.hitDistance = hitInfo.distance;
                    _collideData.hitObjects.Add(new HitData(damageable, hitInfo.point));

                    break;
                }
            }
        }
    }

    protected override float CalcSpeed()
    {
        float speed = GetComponent<Missile>().m_Stats.speed;
        float acceleration = GetComponent<Missile>().m_Stats.acceleration;
        //adds accel to velocity in forwards direction.
        Vector3 delta = transform.forward * acceleration;
        m_Velocity += delta;
        //caps velocity.magnitude to mSpeed.
        if(m_Velocity.magnitude > speed)
        {
            m_Velocity.Normalize();
            m_Velocity *= speed;
        }

        return m_Velocity.magnitude;
    }
}
