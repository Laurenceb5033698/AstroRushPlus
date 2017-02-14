using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private struct ShipT
    {
        public int typeID;             // type of ship
        public int shipToSpawn;        // how many ships need to be spawned
        public int spawnCounter;       // how many ships already spawned
        public int deadCounter;        // how many of them is already dead
        public List<GameObject> shipP; // list of all ships on scene
    }

    private const int shipPreftypes = 2;
    [SerializeField] private GameObject[] prefRef = new GameObject[shipPreftypes]; // prefab list
    [SerializeField] private GameObject group;
    private ShipT[] shipOrder = new ShipT[shipPreftypes];
    [SerializeField] private int numberOfShipOrders = 0;
    private GameObject player;

    private int globalID = 0;
    private const float spawnDelay = 0.5f;
    private float spawnTimer = 0.0f;
    private List<int> toSpawn = new List<int>(); // spawnShips() - create a list of ship type IDs to spawn from

    //------------------------------------------------------------------------

    private int SpawnLimit;
    private bool SpawnComplete = false;
    private const int shipLimitOnScreen = 20;
    private const float ResetDistance = 250;

    private bool spawnerActive = false;

    void Start() // Use this for initialization
    {
        player = GetComponent<GameManager>().GetShipRef();
    }
    void Update() // Update is called once per frame
    {
        if (spawnerActive) 
        { 
            RunUpdates();
            RepositionShips();
        }
    }

    private void RunUpdates()
    {
        if (GetTotalShipLeft() == 0) // buggy  somewhy it spawn an extra 1-2 ships
        {
            SpawnComplete = true;
            spawnerActive = false;
            //DestroyAllShip();
            Debug.Log("Wave Complete!");
        }
        else
        {
            SpawnShips();
        }
    }

    public void CreateOrder(int i, int sts)
    {
        if (i <= shipPreftypes && i >= 0)
        {
            ShipT temp;
            temp.typeID = i;
            temp.shipToSpawn = sts;
            temp.spawnCounter = 0;
            temp.deadCounter = 0;
            temp.shipP = new List<GameObject>();
            shipOrder[i] = temp;

            SpawnLimit = 0;
            for (int j = 0; j < shipPreftypes; j++) SpawnLimit += shipOrder[j].shipToSpawn;
        }
        else
        {
            Debug.Log("Out of Range order!");
        }
    }

    private void SpawnShips()
    {
        if (Time.time > spawnTimer) // if current time is more than delay
        {
            spawnTimer = Time.time + spawnDelay; // update new time

            if (GetTotalShipsOnScene() < shipLimitOnScreen)
            {
                toSpawn.Clear();

                for (int i = 0; i < shipPreftypes; i++) // find ship types to spawn
                {
                    if (shipOrder[i].shipToSpawn > shipOrder[i].spawnCounter)
                        toSpawn.Add(i);
                }
                if (toSpawn.Count >= 2) // if there is more than one types to spawn, pick a random one
                {
                    SpawnShip(Random.Range(0, toSpawn.Count));
                }
                else if (toSpawn.Count > 0) // if there is only one type, spawn that
                {
                    SpawnShip(toSpawn[0]);
                }
            }
            else
                Debug.Log("too much ship on scene to spawn a new");

        }
    }


    private Vector3 GetRandomPosition() {
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        float distance = Random.Range(100, 200); // spawn distance relative to the player
        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        return player.transform.position + dir * distance;
    }


    private void SpawnShip(int type)
    {
        GameObject temp = (GameObject)Instantiate(prefRef[type], GetRandomPosition(), Quaternion.identity);
        //temp.transform.position = player.transform.position + dir * distance;
        temp.GetComponent<NewBasicAI>().Initalise(player, transform.gameObject, globalID, type);

        shipOrder[type].shipP.Add(temp);
        shipOrder[type].spawnCounter++;
        globalID++;

        temp.transform.parent = group.transform; // put ship in group game object

        //Debug.Log("ship spawned");
    }

    public void RemoveShip(int id, int type)
    {
        shipOrder[type].deadCounter++;

        int counter = 0;
        bool found = false;
        do
        {
            if (shipOrder[type].shipP[counter].GetComponent<NewBasicAI>().GetId() == id)
            {
                shipOrder[type].shipP.RemoveAt(counter);
                found = true;
            }
            counter++;
        } while (found == false && counter < shipOrder[type].shipP.Count);
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
        SpawnLimit = 0;
        SpawnComplete = false;
    }
    public void SetActive(bool state)
    {
        spawnerActive = state;
    }

    public bool GetSpawnState()
    {
        return SpawnComplete;
    }
    private int GetTotalShipsKilled()
    {
        int sum = 0;
        for (int i = 0; i < shipPreftypes; i++)
        {
            sum += shipOrder[i].deadCounter;
        }
        return sum;
    }
    public int GetTotalShipLeft()
    {
        int sum = 0;
        for (int i = 0; i < shipPreftypes; i++)
        {
            sum += shipOrder[i].shipToSpawn - shipOrder[i].deadCounter;
        }
        return sum;
    }
    private int GetTotalShipsOnScene()
    {
        int sum = 0;
        for (int i = 0; i < shipPreftypes; i++)
        {
            sum += shipOrder[i].spawnCounter - shipOrder[i].deadCounter;
        }
        return sum;

    }


    private void RepositionShips(){

        float tempDistance = 0;
        

        for (int i = 0; i < shipPreftypes; i++)
        {
            for (int j = 0; j < shipOrder[i].shipP.Count; j++)
            {
                if (shipOrder[i].shipP[j] != null)
                {
                    tempDistance = Vector3.Distance(player.transform.position, shipOrder[i].shipP[j].transform.position);
                    if (tempDistance > ResetDistance)
                    {
                        shipOrder[i].shipP[j].transform.position = GetRandomPosition();          
                    }
                }
            }
        }

    }

    public Vector3 getClosestShipPos(Vector3 from)
    {
        Vector3 pos = Vector3.zero;
        float bestDistance = 100000.0f;
        float tempDistance;

        for (int i = 0; i < shipPreftypes; i++)
        {
            for (int j = 0; j < shipOrder[i].shipP.Count; j++)
            {
                if (shipOrder[i].shipP[j] != null)
                {
                    tempDistance = Vector3.Distance(from, shipOrder[i].shipP[j].transform.position);
                    if (tempDistance < bestDistance)
                    { 
                        pos = shipOrder[i].shipP[j].transform.position;
                        bestDistance = tempDistance;
                    }
                }
                else{
                    Debug.Log("ship not found");
                }
            }
        }

        if (pos == Vector3.zero) pos = from;

        return pos;
    }
}
