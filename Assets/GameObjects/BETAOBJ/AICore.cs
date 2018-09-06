using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICore : MonoBehaviour {

    //Generic AI data and references

    //System data
    private int id;
    private int type;
    //private GameObject sm;
    [SerializeField] private int scoreValue;

    //Customise AI range behaviours
    [SerializeField] private float innerRange = 30f;
    [SerializeField] private float ShootRange = 30f;
    [SerializeField] private float KeepAwayRange = 10f;
    [SerializeField] private float torqueMultiplier = 1.5f;
    //Component References
    [SerializeField] private GameObject ship; // enemy ship object
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ShipStats stats;
    [SerializeField] private Arsenal arsenal;
    public GameObject SceneManagerObject;

    //Visual Effects
    [SerializeField] private GameObject psDestructPrefab;
    [SerializeField] private ParticleSystem shield_Emitter;

    //Movement Behavior
    public GameObject player;
    [SerializeField] private Vector3 destination;
    private Vector3 controlDir;
    public float CollisionImpulse = 50;
    float dist;             //dist to destination
    float torqueMul = 1f;   //amplify turn speed

    // Use this for initialization
    void Awake()
    {   //Setup AICore references
        stats = gameObject.GetComponent<ShipStats>();       //local stats
        ship = gameObject;                                  //local gameobject
        rb = ship.gameObject.GetComponent<Rigidbody>();     //local rigidbody

        arsenal = GetComponentInChildren<Arsenal>();        //local weapons platform
        if (arsenal == null) Debug.Log("No weapon attached.");
        else
            arsenal.SetShipObject(ship);

    }

    void Start () {
		
	}
	

	void Update () {
        if (stats.IsAlive())
        {
            if (!stats.GetDisabled())
            {
                destination = player.transform.position;
                Movement();
                //perhaps do interaction here?
                //perhaps do shoot here?
            }
        }
        else
        {
            //if (!spawnedPickup) spawnedPickup = sm.GetComponent<PickupManager>().SpawnPickup(transform.position);
            Instantiate(psDestructPrefab, transform.position, transform.rotation);
            //spawn debris?

            //DestroySelf();    //destruction handled by AIManager
            gameObject.SetActive(false);
        }

    }

    //Base function called by evey AI
    //  contains logic for each AI's movement behaviour
    
    //Is able to move (not disabled, not emp'd, etc.)
    virtual protected void Movement()
    {   //determins target/destination
        //GetTarget();  //player or other

        //do dist check with target
        DistToDestination();
        //calls relevant movemnt function based on kind of target
        MovementAgainstTarget();    //default mode : attack target (player)
    }

    //has non-hostile destination/target, moves to destination, or interactes with target object
    virtual protected void MovementToDestination()
    {   //used for movement to destinations/objects that are not hostile to this target (e.g. asteroid, other AI ship)

        //determin behaviour based on dist to target


        //turn towards target
        Turn();
        //movment to target
        Move();

        //perhaps do interaction here? (grab asteroid, repair object, upgrade, drop item etc)
    }

    //has hostile target, tries to engage target in combat (usually player)
    virtual protected void MovementAgainstTarget()
    {
        //do dist check with target
        //perform if statments to determine behavioural outcome based on dist to target

        //call turn function
        Turn();
        //call movement function
        Move();

        //attack method (sometimes melee? sometimes ranged?)

        //determine if I can shoot at the target
        if ((dist > KeepAwayRange) && (dist <= ShootRange) && (arsenal != null))
            arsenal.FireWeapon(controlDir); //shoot at target
    }

    //
    //
    //
    //
    
    virtual protected void DistToDestination()
    {
        dist = Vector3.Distance(destination, gameObject.transform.position);
        controlDir = (destination - ship.transform.position).normalized;
        //if dest is too close, fly away
        torqueMul = 1f;

        if (dist < KeepAwayRange)
        {
            controlDir = (ship.transform.position - destination).normalized;
            torqueMul = torqueMultiplier;
        }
    }

    virtual protected void Move()
    {   //Default Ship movement behavior
        float currentSpeed = (dist <= innerRange) ? stats.GetBoostSpeed() : stats.GetMainThrust();//use speeds from shipStats. Change on prefabs

        rb.AddForce(gameObject.transform.forward * currentSpeed * 20 * Time.deltaTime, ForceMode.Acceleration);
        
    }

    virtual protected void Turn()
    {
        float angle = Vector3.Angle(controlDir, gameObject.transform.forward);

        if (Vector3.Cross(controlDir, gameObject.transform.forward).y < 0) angle = -angle;
        angle = angle / -180;

        float torque = stats.GetRotSpeed() * torqueMul;
        rb.AddRelativeTorque(Vector3.up * torque * angle * Time.deltaTime);
    }

    protected void LateUpdate()
    {   //Align ship to y-plane (stops unwanted wobbling)
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }


    virtual protected void Shield_effect(Vector3 other)
    {   //Default shield visual effect method
        Vector3 dir = other - shield_Emitter.transform.position;
        dir.Normalize();
        shield_Emitter.transform.rotation = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0, -90, 0);

        shield_Emitter.Play();
        GetComponentInChildren<Animation>().Stop();
        GetComponentInChildren<Animation>().Play();
    }

    virtual public void TakeDamage(Vector3 otherpos, float amount)
    {   //Default method for Taking Damage
        if (shield_Emitter != null && stats.ShipShield > 0)
            Shield_effect(otherpos);
        stats.TakeDamage(amount);
    }

    private void OnCollisionStay(Collision c)
    {
        if (!c.gameObject.GetComponent<Projectile>())
            rb.AddForce(((ship.transform.position - c.gameObject.transform.position).normalized) * 100, ForceMode.Force);
    }

    void OnCollisionEnter(Collision c)
    {   //Collided with any rigidbody object
        if (!c.gameObject.GetComponent<Projectile>())
        {
            rb.AddForce(((ship.transform.position - c.gameObject.transform.position).normalized) * CollisionImpulse, ForceMode.Impulse);
            TakeDamage(c.gameObject.transform.position, 10 * Time.deltaTime);
        }
    }

    private void DestroySelf()
    {
        //sm.GetComponent<GameManager>().AddScore(scoreValue);
        //if (type != -1) sm.GetComponent<EnemyManager>().RemoveShip(id, type);
        Destroy(transform.gameObject);
    }

    public void Initialise(GameObject ThePlayer, GameObject sceneManager)
    {
        player = ThePlayer;
        SceneManagerObject = sceneManager;
    }

    public bool GetAlive()
    {
        return stats.IsAlive();
    }
}
