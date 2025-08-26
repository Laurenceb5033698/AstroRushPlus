using UnityEngine;
using System.Collections;

public class ProximityMine : Projectile {

    protected Rigidbody rb;
    [SerializeField] private Transform indicator;
    //no movement 
    //can hold an "effect"
    [SerializeField] private GameObject AoeEffectPrefab;

    //detonates at set distance
    public float DetonationDistance = 4.0f;
    public float BlastRadius = 20f;
    private bool Triggered = false;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        //lifetime = 20f;

        //collision size stuff
        indicator.localScale.Set(DetonationDistance, 0, DetonationDistance);
        GetComponent<SphereCollider>().radius = DetonationDistance;

    }
    
    protected override void Update()
    {
        m_Stats.lifetime -= Time.deltaTime;
        if (m_Stats.lifetime <= 0.0f)
        {
            Triggered = true;
        }
        if (Triggered) DestroySelf();
        //  spin    
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.forward, transform.right), 10.0f);

        //  slow down


    }

    protected void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.tag != ownertag) && (collision.gameObject.GetComponent<Projectile>() == null)&& (collision.gameObject.GetComponent<PickupItem>() == null))
        {
            Triggered = true;
        }
    }
    protected override void DestroySelf()
    {
        if (AoeEffectPrefab == null)
        {

            Collider[] inRange = Physics.OverlapSphere(transform.position, BlastRadius);

            foreach (Collider victim in inRange)
            {
                //float dist = Vector3.Distance(victim.transform.position, transform.position);


                if (victim.gameObject.tag == "Asteroid")
                {
                    victim.gameObject.GetComponent<Asteroid>().TakeDamage(OwnerEventSource, transform.position, m_Stats.damage);
                    applyImpulse(victim.GetComponent<Rigidbody>());
                }
                else
                {
                    if (victim.gameObject.tag == "EnemyShip")
                    {
                        victim.GetComponentInParent<AICore>().TakeDamage(OwnerEventSource, transform.position, m_Stats.damage);
                        applyImpulse(victim.GetComponentInParent<Rigidbody>());
                    }
                }
                //deal lower damage to player ship
                //destroy shards
                //also destroy projectiles? ->maybe save that effect for emp


            }
        }
        else
        {
            GameObject instance = Instantiate(AoeEffectPrefab, transform.position, transform.rotation);
            instance.GetComponent<AoeEffect>().SetupValues(OwnerEventSource, m_Stats.damage, ownertag);
        }

        Instantiate(psImpactPrefab, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }

    public override void applyImpulse(Rigidbody body)
    {
        Vector3 direction = body.transform.position - transform.position;
        direction.Normalize();
        body.AddForce(direction * ((m_Stats.damage / 2) + (m_Stats.speed / (2 + body.mass))), ForceMode.Impulse);
    }
}