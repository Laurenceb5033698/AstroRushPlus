using UnityEngine;
using System.Collections;

class AoeMissile : Missile
{
    public float radius;

    // Use this for initialization
    void Start()
    {
        radius = 10f;
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        lifetime = 10f;
        target = findTarget();
    }

    // Update is called once per frame
    protected override void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f)
        {
            DestroySelf();
        }
        if (target)
        {
            direction = (target.transform.position - transform.position).normalized;

            rb.AddForce(direction * 1500 * Time.deltaTime, ForceMode.Force);
            if (Vector3.Dot(transform.forward, direction) < 0.2f)
                rb.AddForce(direction * 200 * Time.deltaTime, ForceMode.VelocityChange);
            if (rb.velocity.magnitude > 80)
                rb.velocity = rb.velocity.normalized * 80;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), 300 * Time.deltaTime);

        }
        else
        {
            rb.AddForce(transform.forward * 1500 * Time.deltaTime, ForceMode.Force);
        }

    }

    protected override void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.GetComponent<Projectile>() == null) && (collision.gameObject.GetComponent<PickupItem>() == null))
        {           
            DestroySelf();
        }
    }
    protected override void DestroySelf()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider victim in inRange)
        {
            float dist = Vector3.Distance(victim.transform.position, transform.position);


            if (victim.gameObject.tag == "Asteroid")
            {
                victim.gameObject.GetComponent<Asteroid>().TakeDamage(damage - 2 * dist);
                applyImpulse(victim.GetComponent<Rigidbody>());
            }
            else
            {
                if (victim.gameObject.tag == "EnemyShip")
                {
                    victim.GetComponentInParent<AICore>().TakeDamage(transform.position, damage - (2f * dist));
                    applyImpulse(victim.GetComponentInParent<Rigidbody>());
                }
            }
            //deal lower damage to player ship
            //destroy shards
            //also destroy projectiles? ->maybe save that effect for emp


        }
        Instantiate(psImpactPrefab, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
    protected override void applyImpulse(Rigidbody body)
    {
        Vector3 direction = body.transform.position - transform.position;
        direction.Normalize();
        body.AddForce(direction * ((damage / 2) + (speed / (2 + body.mass))), ForceMode.Impulse);
    }
}

