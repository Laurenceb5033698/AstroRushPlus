using UnityEngine;
using System.Collections;

public class SpawnAsteroidField : MonoBehaviour {

	public GameObject[] asteroids = new GameObject[5];
	public GameObject[] spawnedAsteroids = new GameObject[NoAsteroids];
	private const int NoAsteroids = 50;

	public GameObject ship;
	private Vector3 position; // position of ship

	private float spawnAngle;
	private float spawnDistance;

	private const float minSpawnDist = 40f;
	private const float maxSpawnDist = 80f;
	private const float killZoneDist = 80f;
	int id = 0;
	float rotSpeed;

	Vector3 randAstRot;
	Vector3 pivot;
	Vector3 point;
	Vector3 dir;

	// Use this for initialization
	void Start () 
	{
		position = ship.transform.position;
		Initalise ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		position = ship.transform.position;
		ResetAsteroid ();
	}

	private void Initalise()
	{
		for (int i = 0; i < NoAsteroids; i++) 
		{
			spawnedAsteroids [i] = SpawnAsteroid();
		}
	}

	private void ResetAsteroid()
	{
		float dist;
		for (int i = 0; i < NoAsteroids; i++) 
		{
			dist = Vector3.Distance (position, spawnedAsteroids [i].transform.position);
			if (dist >= killZoneDist) 
			{
				Destroy (spawnedAsteroids[i].gameObject);
				spawnedAsteroids [i] = SpawnAsteroid ();
			}
		}
	}

	private GameObject SpawnAsteroid()
	{
		GenerateRandoms ();

		GameObject temp = (GameObject)Instantiate(asteroids[id], point , Quaternion.identity);
		temp.transform.parent = transform;
		temp.GetComponent<Rigidbody>().AddTorque (randAstRot * rotSpeed);
		temp.AddComponent<Asteroid> ();
		temp.name = "Asteroid";

		return temp;
	}

	private void GenerateRandoms()
	{
		id = Random.Range(0,4);
		spawnDistance = Random.Range (minSpawnDist,maxSpawnDist);
		spawnAngle = Random.Range (0f, 360f);
		rotSpeed = Random.Range (-0.5f,0.5f);

		randAstRot = new Vector3 (Random.Range (0f, 360f),Random.Range (0f, 360f),Random.Range (0f, 360f));

		pivot = position;
		point = new Vector3 (position.x + spawnDistance, position.y, position.z);
		dir = point - pivot;
		dir = Quaternion.Euler (new Vector3 (0f, spawnAngle, 0f)) * dir;
		point = dir + pivot;
	}
}
