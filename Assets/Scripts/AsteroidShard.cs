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
        Vector2 velocity = new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));

        rb.AddForce(new Vector3(velocity.x, 0, velocity.y),ForceMode.Impulse);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + velocity.x, 0, gameObject.transform.position.z + velocity.y);
    }
	
	void Update () 	// Update is called once per frame
    {
        if (Time.time > timeOfDestruction)
        {
            Destroy(gameObject);
        }
	}
}
