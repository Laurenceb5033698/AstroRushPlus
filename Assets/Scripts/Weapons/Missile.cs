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
		if (collision.gameObject.tag == "Asteroid") Destroy(collision.gameObject);
        else if (collision.gameObject.tag == "EnemyShip") collision.gameObject.GetComponentInParent<EnemyAI>().TakeDamage(80f);        
        DestroySelf();
    }

	private void DestroySelf()
	{
		Instantiate (exp,transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
