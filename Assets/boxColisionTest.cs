using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxColisionTest : MonoBehaviour {

    private Rigidbody rb;
    private bool inColision = false;
    Vector3 dir = Vector3.zero;
    private float resetTimer;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (inColision && Time.time < resetTimer)
            rb.AddForce(dir * 20, ForceMode.Force);
        else
            inColision = false;
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Projectile>() == null)
        {
            dir = (transform.position - collision.transform.position).normalized;
            Debug.Log("coliding");
            inColision = true;
            resetTimer = Time.time + 2;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        inColision = false;
    }
    
}
