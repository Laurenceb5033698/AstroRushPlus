using UnityEngine;
using System.Collections;

public class Missile : Projectile 
{
    //private float detonateAt;
    private Rigidbody rb;

	public GameObject exp;

	// Use this for initialization
    void Start () 
	{
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        lifetime = Time.time + 1f;
    }
	
	// Update is called once per frame
	protected override void Update () 
	{
		if (Time.time > lifetime) 
		{
			DestroySelf ();
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

	protected override void DestroySelf()
	{
		Instantiate (exp,transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
