using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
    private float detonateAt;
    private Rigidbody rb;

	public GameObject exp;

	// Use this for initialization
    void Start () 
	{
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        detonateAt = Time.time + 1f;
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > detonateAt) 
		{
			DestroySelf ();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Asteroid") collision.gameObject.GetComponentInParent<Asteroid>().TakeDamage(80f);
        else if (collision.gameObject.tag == "EnemyShip") collision.gameObject.GetComponentInParent<NewBasicAI>().TakeDamage(80f);
        else if (collision.gameObject.tag == "GeneratorShield") collision.gameObject.GetComponentInParent<Generator>().TakeDamage(50f); 
        DestroySelf();
    }

	private void DestroySelf()
	{
		Instantiate (exp,transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
