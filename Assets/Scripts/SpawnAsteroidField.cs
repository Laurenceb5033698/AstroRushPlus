using UnityEngine;
using System.Collections;

public class SpawnAsteroidField : MonoBehaviour {

	public GameObject[] asteroids = new GameObject[5];
	private Vector3 maxSpawnDistance = new Vector3(300,100,300);
	private const int NoAsteroids = 2000;

	// Use this for initialization
	void Start () 
	{
		int id = 0;

		for (int i = 0; i < NoAsteroids; i++) 
		{
			id = Random.Range(0,4);
			Vector3 pos = new Vector3 (Random.Range(-maxSpawnDistance.x,maxSpawnDistance.x),Random.Range(-maxSpawnDistance.y,maxSpawnDistance.y),Random.Range(-maxSpawnDistance.z,maxSpawnDistance.z));
			Vector3 rot = new Vector3(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
			float rotSpeed = Random.Range (-0.5f,0.5f);

			GameObject temp = (GameObject)Instantiate(asteroids[id], pos, Quaternion.identity);
			temp.transform.parent = transform;
			temp.GetComponent<Rigidbody>().AddTorque (rot * rotSpeed);
			temp.AddComponent<Asteroid> ();
			temp.name = "Asteroid";
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
