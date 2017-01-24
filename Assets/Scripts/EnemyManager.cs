using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyManager : MonoBehaviour {

    private int globalID = 0;

    //[SerializeField]
    private struct ShipT
    {
        int typeID;             // type of ship
        GameObject shipPref;    // prefab reference
        int shipToSpawn;        // how many ships need to be spawned
        int spawnCounter;       // how many ships already spawned
        int deadCounter;        // how many of them is already dead
        List<GameObject> shipP; // list of all ships on scene
    }
    [SerializeField] private List<ShipT> shipOrder = new List<ShipT>();





    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPref;

    [SerializeField] private int shipsSpawned = 0; // per wave
    [SerializeField] private int shipsKilled = 0;
    private int waveLimit = 100;
    private bool waveComplete = false;
    private const int shipLimitOnScreen = 20;

    [SerializeField] private List<GameObject> ships = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        Initalise();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    private GameObject SpawnShip(int id)
    {
        shipsSpawned++;
        Debug.Log("ship spawned");

        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 250);
        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));


        GameObject temp = (GameObject)Instantiate(enemyPref, player.transform.position + dir * distance, Quaternion.identity);
        temp.transform.position = player.transform.position + dir * distance;
        temp.GetComponent<BasicAI>().Initalise(player, transform.gameObject, id);
        return temp;
    }

    private void Initalise()
    {
        for (int i = 0; i < shipLimitOnScreen; i++)
        {
            ships.Add(SpawnShip(i));
        }
    }
    private void DestroyAll()
    {
        int s = ships.Count;
        for (int i = 0; i < s; i++)
        {
            if (ships[0].transform.gameObject != null) Destroy(ships[0].transform.gameObject);
            ships.RemoveAt(0);
        }
        ships.Clear();
    }

    public void SetSpawnLimit(int v)
    {
        // update
        waveLimit = v;

        // reset
        DestroyAll();
        waveComplete = false;
        shipsSpawned = 0;
        shipsKilled = 0;

        // initalise
        Initalise();
    }

    public bool GetWaveState()
    {
        return waveComplete;
    }

    public void RemoveShip(int id)
    {
        shipsKilled++;
        //Destroy(ships[id].transform.gameObject);

        if (shipsSpawned < waveLimit)
        {
            ships[id] = SpawnShip(id);
        }
        else if (shipsKilled >= waveLimit - 2) // buggy  somewhy it spawn an extra 1-2 ships
        {
            waveComplete = true;
            Debug.Log("Wave Complete!");
            ships.Clear();
        }

    }
}
