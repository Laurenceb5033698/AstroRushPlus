using UnityEngine;
using System.Collections;

public class Missile : Projectile 
{
    //private float detonateAt;
    protected Rigidbody rb;

    protected GameObject target;
    protected Vector3 direction;

    void Start () 
	{
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        target = findTarget();
    }
    override public void SetupValues(string _ownerTag, Stats _setupStats)
    {
        ownertag = _ownerTag;
        m_Stats = new BulletStats(_setupStats, rb);
    }

    protected override void Update () 
	{
        m_Stats.lifetime -= Time.deltaTime;
		if (m_Stats.lifetime < 0) 
		{
			DestroySelf ();
		}

        if (target)
        {
            direction = (target.transform.position - transform.position).normalized;

            rb.AddForce(direction * 3000 * Time.deltaTime, ForceMode.Force);
            if (Vector3.Dot(transform.forward, direction) < 0.2f)
                rb.AddForce(direction * 100 * Time.deltaTime, ForceMode.VelocityChange);
            if (rb.linearVelocity.magnitude > 100)
                rb.linearVelocity = rb.linearVelocity.normalized * 100;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), 300 * Time.deltaTime);

        }
        else
        {
            rb.AddForce(transform.forward * 3000 * Time.deltaTime, ForceMode.Force);
        }

	}
    protected override void OnTriggerEnter(Collider _collision)
    {
        if ((_collision.gameObject.GetComponent<Projectile>() == null) &&(_collision.gameObject.GetComponent<PickupItem>() == null))
        {
            bool hit = false;
            Damageable otherDamageable = _collision.GetComponent<Damageable>();
            if (otherDamageable)
            {
                otherDamageable.TakeDamage(transform.position, CalcDamage());
                applyImpulse(otherDamageable.GetRigidbody());
                hit = (otherDamageable is not Shard_Damageable);//shards are damaged, but does not count to a hit
            }
            
            if(hit)
                DestroySelf();
        }
    }

    protected GameObject findTarget() //Looks for certain objects nearby 
     {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Collider[] targetColliders = Physics.OverlapSphere(transform.position, 90f); 
        foreach (Collider col in targetColliders) 
        {
            //Debug.Log(col.gameObject.name); 
            if (col.gameObject.GetComponentInParent<AICore>() != null)
            {
                Vector3 directionToTarget = col.gameObject.transform.position - transform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = col.gameObject;
                }
            }
        }
        return bestTarget;
     }

    override protected float CalcDamage()
    {
        return m_Stats.damage;
    }


    protected override void DestroySelf()
	{
        SpawnHitVisuals();
        //Instantiate (psImpactPrefab, transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
