using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {


    List<GameObject> ActiveShips; //(AIs)

    int maxActiveShips = 20; //(ship limit onscreen)
    int RemainingShipsToSpawn= 0 ;
    int totalShipsThisWave = 6; //(+2 per wave)

    float spawnDelay = 1f;
    float spawnTimer = 0f;
    float ResetDistance = 300f;
    float minResetDistance = 200f;
    bool InWave = false;
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


    }

    void Update() {

        RemoveDeadShips();
        SlowUpdate();
        
    }

    private void SpawnShip()
    {   //spawns a random ship from set prefabs in a random place
        if (spawnTimer < Time.time)
        {
            GameObject RandomPref = ShipPrefabs[Random.Range(0, ShipPrefabs.Count)];
            GameObject NewShip = Instantiate(RandomPref, GetRandomPosition(), Quaternion.identity);
            NewShip.GetComponent<AICore>().Initialise(player, gameObject);

            ActiveShips.Add(NewShip);
            NewShip.transform.parent = SceneGroup.transform;

            --RemainingShipsToSpawn;
            spawnTimer = Time.time + spawnDelay;
        }
    }

    private void NewWave()
    {   //advances number of ships per wave;
        //  ?bosswave? how? spawn separately?

        totalShipsThisWave += 4;
        RemainingShipsToSpawn = totalShipsThisWave;
        InWave = true;
        newWaveSound.Play();
        ++WaveCounter;
    }


    //updates one ship per frame
    // ~good for retargeting and unimportant stuff (repositioning)
    private void SlowUpdate()
    {
        //number of active ships can change (removeDeadShips) so check and account for that
        if (SlowIndex >= ActiveShips.Count)
            SlowIndex = ActiveShips.Count - 1;

        //wave management
        if (InWave)
        {
            if (ActiveShips.Count < maxActiveShips)
            {
                SpawnShip();
            }
        }
        else
        {  //all ships spawned
            if (ActiveShips.Count == 0 && RemainingShipsToSpawn == 0)
                NewWave();  //waits for all ships to die
        }

        //retargetship(SlowIndex);

        //reposition lost ships
        RepositionShip(SlowIndex);

        //advance SlowIndex
        if(--SlowIndex < 0)
            SlowIndex = ActiveShips.Count-1;
      
    }

    //////////////////////
    //  Ship Processing
    //

    private void RemoveDeadShips()
    {   //called from Update;   processes all ships and destroys dead ones
        foreach (GameObject ship in ActiveShips) {
            if (!ship.GetComponent<AICore>().GetAlive()){
                rPickupManager.GetComponent<PickupManager>().SpawnPickup(ship.transform.position);
                ActiveShips.Remove(ship);
                Destroy(ship);
          }
      }
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

    public int GetTotalShipLeft()
    {
        return ActiveShips.Count;
    }
}
