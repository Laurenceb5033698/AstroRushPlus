using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	private float lifeSpam = 0.5f;
	private float progress = 0f;

	// Use this for initialization
	void Start () 
	{
        lifeSpam = Time.time + 0.5f;
		//transform.gameObject.GetComponent<ParticleSystem> ().enableEmission = true;
        ParticleSystem.EmissionModule temp = transform.gameObject.GetComponent<ParticleSystem>().emission;
        temp.enabled = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
		progress += 1f * Time.deltaTime;
		transform.gameObject.GetComponent<ParticleSystem> ().Simulate (progress);

		
		if (lifeSpam < Time.time)
		{
			Destroy (transform.gameObject);
		}
	}
}
