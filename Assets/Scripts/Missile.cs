using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	private const float lifeSpam = 2f;
	private float countDown = lifeSpam;
    private Rigidbody rb;

	public GameObject exp;

	// Use this for initialization
    void Start () 
	{
        rb = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        rb.AddForce(transform.up * 20f); // accelerate

		countDown -= 1 * Time.deltaTime;
		if (countDown < 0) 
		{
			DestroySelf ();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Asteroid")
        	Destroy(collision.gameObject); // this will destroy the object that the missile collide with

		DestroySelf ();
	}

	private void DestroySelf()
	{
		Instantiate (exp,transform.position,transform.rotation);
        Destroy (transform.gameObject);
	}
}
