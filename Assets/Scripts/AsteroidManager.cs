using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidManager : MonoBehaviour
{
    private float ResetDist = 270;
    private const int NoAsteroids = 60;

    private GameObject ship;
    [SerializeField] private GameObject[] chunks = new GameObject[2];
    [SerializeField] private GameObject[] asteroids = new GameObject[5]; // asteroid prefab

    [SerializeField] private GameObject[] backgroundAsteroidPrefs = new GameObject[5];
    [SerializeField] private List<GameObject> backgroundAsteroids = new List<GameObject>();

    [SerializeField] private GameObject[] spawnedAsteroids = new GameObject[NoAsteroids];
    [SerializeField] private GameObject[] groups = new GameObject[3]; // game objects to store the spawned objects like folders

    // Use this for initialization
    void Start ()
    {
        SpawnBackgroundAsteroids(100);

        for (int i = 0; i < NoAsteroids; i++)
        {
            spawnedAsteroids[i] = SpawnAsteroid();
        }
	}
    void OnEnable()
    {
        ship = GetComponent<GameManager>().GetShipRef(); // assighn player ship reference to local pointer
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        for (int i = 0; i < NoAsteroids; i++)
        {
            if (Vector3.Distance(ship.transform.position, spawnedAsteroids[i].transform.position) > ResetDist)
                spawnedAsteroids[i].GetComponent<Asteroid>().Reset();
        }
	}



    private void SpawnBackgroundAsteroids(int amount) {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-1500, 1500), Random.Range(-1000, -500), Random.Range(-1500, 1500));
            GameObject temp =  (GameObject)Instantiate(backgroundAsteroidPrefs[Random.Range(0, 5)], randomPos, Quaternion.identity);
            temp.transform.parent = groups[2].transform;
            backgroundAsteroids.Add(temp);
        }
    }

    private Vector3 GetRandomPos() {
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 250);
        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        return ship.transform.position + dir * distance;
    }

    private GameObject SpawnAsteroid()
    {   
        GameObject temp = (GameObject)Instantiate(asteroids[Random.Range(0, 5)], GetRandomPos(), Random.rotation);
        //temp.transform.parent = transform;
        temp.transform.parent = groups[0].transform;
        temp.GetComponent<Asteroid>().SetAsteroidManager(gameObject);
        temp.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere;
        //temp.AddComponent<ID>();
        //temp.AddComponent<Health>();
        //temp.GetComponent<ID>().Initalise(id, transform.gameObject);
        //temp.GetComponent<Health>().SetHealth(100);
        temp.tag = "Asteroid";
        temp.name = "Asteroid";

        return temp;
    }

    public void Reset(GameObject go)
    {
        //use checker here to find suitable location (i.e. not inside another object)
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 250);
        Vector3 dir = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle));
        go.transform.position = ship.transform.position + dir * distance;
        
    }

    public void SpawnChunks(Vector3 pos)
    {
        int shards = Random.Range(3,10);

        for (int i = 0; i < shards; i++)
        {
            GameObject temp = (GameObject)Instantiate(chunks[Random.Range(0, 2)], pos, Quaternion.identity);
            temp.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere;
            //temp.transform.parent = transform;
            temp.transform.parent = groups[1].transform;
        }
    }
}
