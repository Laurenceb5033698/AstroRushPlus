using UnityEngine;
using System.Collections;
using Unity.Mathematics;

class AoeMissile : Missile
{
    public float radius;

    // Use this for initialization
    void Start()
    {
        radius = 10f;
        rb = transform.GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        //lifetime = 10f;
        target = findTarget();
    }

    // Update is called once per frame
    protected override void Update()
    {
        m_Stats.lifetime -= Time.deltaTime;
        if (m_Stats.lifetime < 0)
        {
            DestroySelf();
        }
        if (target)
        {
            direction = (target.transform.position - transform.position).normalized;

            //rb.AddForce(direction * 1500 * Time.deltaTime, ForceMode.Force);
            //if (Vector3.Dot(transform.forward, direction) < 0.2f)
            //    rb.AddForce(direction * 200 * Time.deltaTime, ForceMode.VelocityChange);
            //if (rb.linearVelocity.magnitude > 80)
            //    rb.linearVelocity = rb.linearVelocity.normalized * 80;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), 300 * Time.deltaTime);

        }
        else
        {
            //rb.AddForce(transform.forward * 1500 * Time.deltaTime, ForceMode.Force);
        }

    }

    //protected override void OnTriggerEnter(Collider _collision)
    //{
    //    if ((_collision.gameObject.GetComponent<Projectile>() == null) && (_collision.gameObject.GetComponent<PickupItem>() == null))
    //    {           
    //        DestroySelf();
    //    }
    //}
    protected override void DestroySelf()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider victim in inRange)
        {
            if (victim.CompareTag(ownertag))
            {
                //do not damage owner.
                continue;
            }

            float dist = Vector3.Distance(victim.transform.position, transform.position);
            Damageable otherDamageable = victim.GetComponent<Damageable>();
            if (otherDamageable)
            {
                float damageOverDistance = (m_Stats.damage - 2 * dist);
                otherDamageable.TakeDamage(transform.position, damageOverDistance);
                applyImpulse(otherDamageable.GetRigidbody());
            }

            //deal lower damage to player ship
            //destroy shards
            //also destroy projectiles? ->maybe save that effect for emp


        }
        SpawnHitVisuals();
        Destroy(transform.gameObject);
    }
    public override void applyImpulse(Rigidbody _body)
    {
        Vector3 direction = _body.transform.position - transform.position;
        direction.Normalize();
        _body.AddForce(direction * ((m_Stats.damage / 2) + (m_Stats.speed / (2 + _body.mass))), ForceMode.Impulse);
    }


}

