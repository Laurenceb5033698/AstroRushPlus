using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {


    List<GameObject> ActiveShips; //(AIs)

    int maxActiveShips = 20; //(ship limit onscreen)
    int RemainingShipsToSpawn= 0 ;
    int totalShipsThisWave = 2; //(+2 per wave)

    float spawnDelay = 1f;
    float spawnTimer = 0f;
    float ResetDistance = 300f;
    float minResetDistance = 150f;
    bool SpawningShips = false;
    int SlowIndex = 0;  //slowUpdate ship Index
    int WaveCounter = 0;

    [SerializeField] List<GameObject> ShipPrefabs;
    [SerializeField] GameObject SceneGroup; //(parent of ships)
    [SerializeField] AudioSource newWaveSound;
    [SerializeField] PickupManager rPickupManager; //(Pickup Manager)
    [SerializeField] private GameObject player; //(Player Object (assigned through code)






    private void Awake()
    {   //set references

    }
    void OnEnable()
    {
        player = GetComponent<GameManager>().GetShipRef();
    }

    void Start() {

        ActiveShips = new List<GameObject>();
        NewWave();
    }

    void Update() {
        
        //wave management
        if (SpawningShips)
        {
            if (ActiveShips.Count < maxActiveShips)
            {
                SpawnShip();
            }
        }
        else
        {  //all ships spawned
            if (ActiveShips.Count == 0 && RemainingShipsToSpawn == 0)
            {
                newWaveSound.Play();    //play sound on end of wave
                NewWave();  //waits for all ships to die
            }
        }
        SlowUpdate();
        
    }

    private void SpawnShip()
    {   //spawns a random ship from set prefabs in a random place
        if (spawnTimer < Time.time)
        {
            GameObject RandomPref = ShipPrefabs[Random.Range(0, ShipPrefabs.Count)];
            GameObject NewShip = Instantiate(RandomPref, GetRandomPosition(), Quaternion.identity);
            NewShip.GetComponent<AICore>().Initialise(player, this, gameObject);

            ActiveShips.Add(NewShip);
            NewShip.transform.parent = SceneGroup.transform;
            
            spawnTimer = Time.time + spawnDelay;
            if (--RemainingShipsToSpawn == 0)
                SpawningShips = false; //end spawning
        }
    }

    private void NewWave()
    {   //advances number of ships per wave;
        //  ?bosswave? how? spawn separately?

        totalShipsThisWave += 4;
        RemainingShipsToSpawn = totalShipsThisWave;
        SpawningShips = true;
        ++WaveCounter;
    }


    //updates one ship per frame
    // ~good for retargeting and unimportant stuff (repositioning)
    private void SlowUpdate()
    {
        //only do this if ships exist
        if (ActiveShips.Count > 0)
        {
            //number of active ships can change (removeDeadShips) so check and account for that
            if (SlowIndex >= ActiveShips.Count)
                SlowIndex = ActiveShips.Count - 1;


            //retargetship(SlowIndex);

            //reposition lost ships
            RepositionShip(SlowIndex);

            //advance SlowIndex
            if (--SlowIndex < 0)
                SlowIndex = ActiveShips.Count - 1;
        }
    }

    //////////////////////
    //  Ship Processing
    //

    public void Remove(GameObject ship)
    {   //called by ship's OnDestroy();
        rPickupManager.GetComponent<PickupManager>().SpawnPickup(ship.transform.position);
        if (ActiveShips.Contains(ship))
            ActiveShips.Remove(ship);
    }
    

    private void RepositionShip(int index)
    {   //called from SlowUpdate;
        GameObject currShip = ActiveShips[index];
        if (Vector3.Distance(player.transform.position, currShip.transform.position) > ResetDistance)
        {   //move ship into range and halt motion
            currShip.transform.position = GetRandomPosition();
            currShip.GetComponent<Rigidbody>().velocity *= 0;
        }

    }


    ////////////////////////
    //  Utility Functions
    //
    
    private Vector3 GetRandomPosition()
    {   //used in spawning and repositioning ships
        float angle = Mathf.Deg2Rad * Random.Range(0, 360);
        return player.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * Random.Range(minResetDistance, ResetDistance);     // player position + direction * distance
    }

    public Vector3 GetClosestShipPos(Vector3 from)
    {
        Vector3 pos = Vector3.zero;
        float bestDistance = 100000.0f;
        float tempDistance;

        foreach ( GameObject ship in ActiveShips) { 
                if (ship != null)
                {
                    tempDistance = Vector3.Distance(from, ship.transform.position);
                    if (tempDistance < bestDistance)
                    {
                        pos = ship.transform.position;
                        bestDistance = tempDistance;
                    }
                }
                else
                {
                    Debug.Log("ship not found");
                }
            }
        

        if (pos == Vector3.zero) pos = from;

        return pos;
    }

    /////////////
    //  Getters
    //

    public int GetTotalShipLeft()
    {
        return ActiveShips.Count;
    }
    public int GetWaveNumber()
    {
        return WaveCounter;
    }
}
