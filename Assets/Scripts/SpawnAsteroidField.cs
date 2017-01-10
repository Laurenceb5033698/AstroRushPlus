using UnityEngine;
using System.Collections;

public class SpawnAsteroidField : MonoBehaviour {

	public GameObject[] asteroids = new GameObject[8];
	private GameObject[] spawnedAsteroids = new GameObject[NoAsteroids];
	private const int NoAsteroids = 60;

	public GameObject ship;
	private Vector3 position; // position of ship

	public GameObject Checker; // prefab
	private GameObject ChGo;

	private float spawnAngle;
	private float spawnDistance;

	private const float minSpawnDist = 80f;
	private const float maxSpawnDist = 180f;
	private const float killZoneDist = 190f;

	// Generate random variables
	private int id = 0;
	private float rotSpeed;
	private Vector3 randAstRot;
	private Vector3 pivot;
	private Vector3 point;
	private Vector3 dir;

	const float sMin = 80f;
	const float sMax = 100f;
	// original scale 0.013f max is 0.03f
	private Vector3 scale;

	// Use this for initialization
	void Start () 
	{
		position = ship.transform.position;
		ChGo = (GameObject)Instantiate (Checker,Vector3.zero,Quaternion.identity);
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
			spawnedAsteroids [i] = SpawnAsteroid ();
		}
	}

	private void ResetAsteroid()
	{
		for (int i = 0; i < NoAsteroids; i++) 
		{
			if (spawnedAsteroids [i].gameObject == null) 
			{
				spawnedAsteroids [i] = SpawnAsteroid ();
			}
			else if (Vector3.Distance (position, spawnedAsteroids [i].transform.position) >= killZoneDist) 
			{
				spawnedAsteroids [i].gameObject.GetComponent<Asteroid> ().DestroyAsteroid ();
				spawnedAsteroids [i] = SpawnAsteroid ();
			}
		}
	}

	private GameObject SpawnAsteroid()
	{
		GetFreePosition (); // get an available space on map

		GameObject temp = (GameObject)Instantiate(asteroids[id], point , Quaternion.identity); 	// create gameobject
		temp.transform.parent = transform; 														// add gameobject to sceneManager as child
		temp.GetComponent<Rigidbody>().AddTorque (randAstRot * rotSpeed); 						// add random rotation to gameobject
		temp.GetComponent<Rigidbody>().maxDepenetrationVelocity = 20f;
		temp.transform.localScale = scale; 														// scale gameobject
		temp.AddComponent<Asteroid> (); 														// add asteroid script to it
		temp.name = "Asteroid"; 																// rename it to Asteroid

		return temp;
	}

	private void GenerateRandoms()
	{
		id = Random.Range(0,8);
		spawnDistance = Random.Range (minSpawnDist,maxSpawnDist);
		spawnAngle = Random.Range (0f, 360f);
		rotSpeed = Random.Range (-0.5f,0.5f);

		scale = new Vector3(Random.Range(sMin,sMax)/1000,Random.Range(sMin,sMax)/1000,Random.Range(sMin,sMax)/1000);

		randAstRot = new Vector3 (Random.Range (0f, 360f),Random.Range (0f, 360f),Random.Range (0f, 360f));

		pivot = position;
		point = new Vector3 (position.x + spawnDistance, position.y + 1f, position.z);
		dir = point - pivot;
		dir = Quaternion.Euler (new Vector3 (0f, spawnAngle, 0f)) * dir;
		point = dir + pivot;
	}

	private void GetFreePosition()
	{
		CheckerScript cs = ChGo.GetComponent<CheckerScript> ();

		do
		{
			cs.ResetCollider();
			GenerateRandoms ();
			ChGo.transform.position = point;
		}
		while(cs.GetColliderState());
	}
}
