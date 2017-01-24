using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	private const float lifeSpam = 0.5f;
	private float countDown = lifeSpam;
	private float progress = 0f;

	// Use this for initialization
	void Start () 
	{
		//transform.gameObject.GetComponent<ParticleSystem> ().enableEmission = true;
        ParticleSystem.EmissionModule temp = transform.gameObject.GetComponent<ParticleSystem>().emission;
        temp.enabled = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
		progress += 1f * Time.deltaTime;
		transform.gameObject.GetComponent<ParticleSystem> ().Simulate (progress);

		countDown -= 1 * Time.deltaTime;
		if (countDown < 0)
		{
			Destroy (transform.gameObject);
		}
	}
}
