using UnityEngine;
using System.Collections;

public class Missile : Projectile 
{
    //private float detonateAt;
    private Rigidbody rb;

	public GameObject exp;
    private GameObject target;
    private Vector3 direction;

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
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() == null)
        {
            if (collision.gameObject.tag == "Asteroid") collision.gameObject.GetComponentInParent<Asteroid>().TakeDamage(damage);
            else if (collision.gameObject.tag == "EnemyShip") collision.gameObject.GetComponentInParent<NewBasicAI>().TakeDamage(damage);
            else if (collision.gameObject.tag == "GeneratorShield") collision.gameObject.GetComponentInParent<Generator>().TakeDamage(damage/1.5f);
            DestroySelf();
        }
    }

    void OnCollisionEnter(Collision collision)
	{
        
    }
    private GameObject findTarget() //Looks for certain objects nearby 
     { 
         Collider[] targetColliders = Physics.OverlapSphere(transform.position, 90f); 
         foreach (Collider col in targetColliders) 
         { 
             //Debug.Log(col.gameObject.name); 
             if (col.gameObject.GetComponentInParent<NewBasicAI>() != null) 
                 return col.gameObject; 
         } 
         return null; 
     } 


	protected override void DestroySelf()
	{
		Instantiate (exp,transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
