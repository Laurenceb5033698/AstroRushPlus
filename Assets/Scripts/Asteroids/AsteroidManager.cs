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
            spawnedAsteroids[i] = SpawnAsteroid(i);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        for (int i = 0; i < NoAsteroids; i++)
        {
            if (Vector3.Distance(ship.transform.position, spawnedAsteroids[i].transform.position) > ResetDist) Reset(i);
        }
	}

    private GameObject SpawnAsteroid(int id)
    {
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 250);
        Vector3 dir = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle));

        GameObject temp = (GameObject)Instantiate(asteroids[Random.Range(0, 5)], ship.transform.position + dir * distance, Quaternion.identity);
        temp.transform.parent = transform;
        temp.AddComponent<ID>();
        temp.AddComponent<Health>();

        temp.GetComponent<ID>().Initalise(id, transform.gameObject);
        temp.GetComponent<Health>().SetHealth(100);
        temp.tag = "Asteroid";
        temp.name = "Asteroid";

        return temp;
    }

    public void Reset(int i)
    {
        Destroy(spawnedAsteroids[i].gameObject);
        spawnedAsteroids[i] = SpawnAsteroid(i);
    }
}
