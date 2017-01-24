using UnityEngine;
using System.Collections;

public class AsteroidManager : MonoBehaviour
{
    private float ResetDist = 270;
    private const int NoAsteroids = 60;

    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject[] asteroids = new GameObject[5]; // asteroid prefab
    [SerializeField] private GameObject[] spawnedAsteroids = new GameObject[NoAsteroids];

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < NoAsteroids; i++)
        {
            spawnedAsteroids[i] = SpawnAsteroid();
        }
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

    private GameObject SpawnAsteroid()
    {
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 250);
        Vector3 dir = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle));

        GameObject temp = (GameObject)Instantiate(asteroids[Random.Range(0, 5)], ship.transform.position + dir * distance, Quaternion.identity);
        temp.transform.parent = transform;
        temp.GetComponent<Asteroid>().SetAsteroidManager(gameObject);

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
}
