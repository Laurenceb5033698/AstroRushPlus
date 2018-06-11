using UnityEngine;
using System.Collections;

public class Missile : Projectile 
{
    //private float detonateAt;
    protected Rigidbody rb;

    protected GameObject target;
    protected Vector3 direction;

    // Use this for initialization
    void Start () 
	{
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        lifetime = Time.time + 5f;
        target = findTarget();
    }
	
	// Update is called once per frame
	protected override void Update () 
	{
		if (Time.time > lifetime) 
		{
			DestroySelf ();
		}
        if (target)
        {
            direction = (target.transform.position - transform.position).normalized;

            rb.AddForce(direction * 3000 * Time.deltaTime, ForceMode.Force);
            if (Vector3.Dot(transform.forward, direction) < 0.2f)
                rb.AddForce(direction * 100 * Time.deltaTime, ForceMode.VelocityChange);
            if (rb.velocity.magnitude > 100)
                rb.velocity = rb.velocity.normalized * 100;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), 300 * Time.deltaTime);

        }
        else
        {
            rb.AddForce(transform.forward * 3000 * Time.deltaTime, ForceMode.Force);
        }

	}
    protected override void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.GetComponent<Projectile>() == null) &&(collision.gameObject.GetComponent<PickupItem>() == null))
        {
            if (collision.gameObject.tag == "Asteroid") {
                collision.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                applyImpulse(collision.GetComponent<Rigidbody>());
            }
            else if (collision.gameObject.tag == "EnemyShip") {
                collision.gameObject.GetComponentInParent<NewBasicAI>().TakeDamage(transform.position, damage);
                applyImpulse(collision.GetComponentInParent<Rigidbody>());
            }
            
            DestroySelf();
        }
    }

    void OnCollisionEnter(Collision collision)
	{
        
    }
    protected GameObject findTarget() //Looks for certain objects nearby 
     {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Collider[] targetColliders = Physics.OverlapSphere(transform.position, 90f); 
        foreach (Collider col in targetColliders) 
        {
            //Debug.Log(col.gameObject.name); 
            if (col.gameObject.GetComponentInParent<NewBasicAI>() != null)
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


	protected override void DestroySelf()
	{
        
        Instantiate (psImpactPrefab, transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
