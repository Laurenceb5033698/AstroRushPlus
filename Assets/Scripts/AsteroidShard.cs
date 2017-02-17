using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShard : MonoBehaviour {

    private float timeOfDestruction;
    private Rigidbody rb;
    private const float maxSpeed = 10;

    void Start ()     // Use this for initialization
    {
        timeOfDestruction = Time.time + 5;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-maxSpeed,maxSpeed),0, Random.Range(-maxSpeed, maxSpeed)),ForceMode.Impulse);
    }
	
	void Update () 	// Update is called once per frame
    {
        if (Time.time > timeOfDestruction)
        {
            Destroy(gameObject);
        }
	}
}
