using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyManager : MonoBehaviour {

    private int globalID = 0;
    private const int shipPreftypes = 2;

    private struct ShipT
    {
        public int typeID;             // type of ship
        public int shipToSpawn;        // how many ships need to be spawned
        public int spawnCounter;       // how many ships already spawned
        public int deadCounter;        // how many of them is already dead
        public List<GameObject> shipP; // list of all ships on scene
    }

    [SerializeField] GameObject[] prefRef = new GameObject[shipPreftypes]; // prefab list
    [SerializeField] private ShipT[] shipOrder = new ShipT[shipPreftypes];
    [SerializeField] private GameObject player;


    //------------------------------------------------------------------------

    private int waveLimit = 100;
    private bool waveComplete = false;
    private const int shipLimitOnScreen = 20;


	// Use this for initialization
	void Start ()
    {
        Initalise();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public void CreateOrder(int i, int sts)
    {
        ShipT temp;
        temp.typeID = i;
        temp.shipToSpawn = sts;
        temp.spawnCounter = 0;
        temp.deadCounter = 0;
        temp.shipP = new List<GameObject>();
        shipOrder[i] = temp;
    }


    private void SpawnShip(int type)
    {
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 250);
        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        GameObject temp = (GameObject)Instantiate(prefRef[type], player.transform.position + dir * distance, Quaternion.identity);
        temp.transform.position = player.transform.position + dir * distance;
        temp.GetComponent<BasicAI>().Initalise(player, transform.gameObject, globalID, type);

        shipOrder[type].shipP.Add(temp);
        shipOrder[type].spawnCounter++;
        globalID++;

        Debug.Log("ship spawned");
    }

    private void Initalise()
    {
        CreateOrder(0,10);
        CreateOrder(1,30);

        waveLimit = 0;

        for (int i = 0; i < shipPreftypes; i++)
        {
            waveLimit += shipOrder[i].shipToSpawn;
        }

        for (int i = 0; i < shipPreftypes; i++)
        {
            for (int j = 0; j < shipOrder[i].shipToSpawn; j++)
            {
                SpawnShip(i);
            }
        }

    }

    private void DestroyAllShip()
    {
        for (int i = 0; i < shipPreftypes; i++)
        {
            int s = shipOrder[i].shipP.Count;
            for (int j = 0; j < s; j++)
            {
                if (shipOrder[i].shipP[0].transform.gameObject != null) Destroy(shipOrder[i].shipP[0].transform.gameObject);
                shipOrder[i].shipP.RemoveAt(0);
            }
            shipOrder[i].shipP.Clear();
        }     
    }

    public void Reset()
    {
        waveLimit = 0;
        DestroyAllShip();
        waveComplete = false;
        Initalise();
    }

    public bool GetWaveState()
    {
        return waveComplete;
    }

    public void RemoveShip(int id, int type)
    {
        shipOrder[type].deadCounter++;

        int totalKilled = 0;

        for (int i = 0; i < shipPreftypes; i++)
        {
            totalKilled += shipOrder[type].deadCounter;
        }


        int counter = 0;
        bool found = false;
        do
        {
            if (shipOrder[type].shipP[counter].GetComponent<BasicAI>().GetId() == id)
            {
                shipOrder[type].shipP.RemoveAt(counter);
                found = true;
            }
            counter++;
        } while (found == false && counter < shipOrder[type].shipP.Count);



        if (shipOrder[type].spawnCounter < shipOrder[type].shipToSpawn)
        {
            SpawnShip(type);
        }
        else if (totalKilled >= waveLimit - 2) // buggy  somewhy it spawn an extra 1-2 ships
        {
            waveComplete = true;
            DestroyAllShip();
            Debug.Log("Wave Complete!");
        }

    }
}
