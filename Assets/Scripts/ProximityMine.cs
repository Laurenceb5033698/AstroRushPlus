using UnityEngine;
using System.Collections;

public class ProximityMine : Projectile {

    protected Rigidbody rb;
    //[SerializeField] private GameObject indicator;
    //no movement 
    //can hold an "effect"

    //detonates at set distance
    public float DetonationDistance = 4.0f;
    public float BlastRadius = 20f;

    void Start()
    {
        //indicator
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        lifetime = 20f;
        GetComponent<SphereCollider>().radius = DetonationDistance;

    }
    
    protected override void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f)
        {
            DestroySelf();
        }
        //  spin    
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.forward, transform.right), 10.0f);

        //  slow down


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
        Collider[] inRange = Physics.OverlapSphere(transform.position, BlastRadius);

        foreach (Collider victim in inRange)
        {
            //float dist = Vector3.Distance(victim.transform.position, transform.position);


            if (victim.gameObject.tag == "Asteroid")
            {
                victim.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                applyImpulse(victim.GetComponent<Rigidbody>());
            }
            else
            {
                if (victim.gameObject.tag == "EnemyShip")
                {
                    victim.GetComponentInParent<NewBasicAI>().TakeDamage(transform.position, damage);
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