using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICore : MonoBehaviour {

    //Generic AI data and references

    //System data
    private int id;
    private int type;
    //private GameObject sm;
    public int scoreValue;

    //Customise AI range behaviours
    [SerializeField] protected float innerRange = 30f;
    [SerializeField] protected float MaxShootRange = 30f;
    [SerializeField] protected float MinShootRange = 9f;
    [SerializeField] protected float KeepAwayRange = 10f;
    [SerializeField] protected float torqueMultiplier = 1.5f;
    //Component References
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Stats stats;
    [SerializeField] private Arsenal arsenal;
    public GameObject SceneManagerObject;
    public AIManager aiManager;

    //Visual Effects
    [SerializeField] private GameObject psDestructPrefab;
    [SerializeField] private ParticleSystem shield_Emitter;

    //Movement Behavior
    public GameObject player;
    [SerializeField] protected Vector3 destination;
    protected Vector3 controlDir;
    protected Vector3 AttackDir;

    public float CollisionImpulse = 50;
    protected float dist;             //dist to destination
    protected float torqueMul = 1f;   //amplify turn speed

    //boids


    // Use this for initialization
    void Awake()
    {   //Setup AICore references
        stats = gameObject.GetComponent<Stats>();       //local stats
        rb = gameObject.GetComponent<Rigidbody>();     //local rigidbody

        arsenal = GetComponentInChildren<Arsenal>();        //local weapons platform
        if (arsenal == null) Debug.Log("No weapon attached.");
        else
            arsenal.SetShipObject(gameObject);

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
        {   //die immediately
            DestroySelf();
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
        if ((dist > MinShootRange) && (dist <= MaxShootRange) && (arsenal != null))
            arsenal.FireWeapon(AttackDir); //shoot at target
    }


    //
    //
    // Locamotion
    //
    
    virtual protected void DistToDestination()
    {
        dist = Vector3.Distance(destination, gameObject.transform.position);
        controlDir = (destination - gameObject.transform.position).normalized;
        AttackDir = controlDir;
        //if dest is too close, fly away
        torqueMul = 1f;

        if (dist < KeepAwayRange)
        {
            controlDir = (gameObject.transform.position - destination).normalized;
            torqueMul = torqueMultiplier;
        }
    }

    virtual protected void Move()
    {   //Default Ship movement behavior
        float currentSpeed = (dist <= innerRange) ? stats.GetSpecial() : stats.GetMainThrust();//use speeds from Stats. Change on prefabs

        rb.AddForce(gameObject.transform.forward * currentSpeed * 20 * Time.deltaTime, ForceMode.Acceleration);
        
    }

    virtual protected void Turn()
    {
        float angle = Vector3.Angle(controlDir, gameObject.transform.forward);

        if (Mathf.Abs(angle) < 10f)
            rb.angularVelocity *= 0.99f;

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

    virtual public void TakeDamage(Vector3 otherpos, int amount)
    {   //Default method for Taking Damage
        if (shield_Emitter != null && stats.ShipShield > 0)
            Shield_effect(otherpos);
        stats.TakeDamage(amount);
    }

    private void OnCollisionStay(Collision c)
    {
        if (!c.gameObject.GetComponent<Projectile>())
            rb.AddForce(((gameObject.transform.position - c.gameObject.transform.position).normalized) * 100, ForceMode.Force);
    }

    void OnCollisionEnter(Collision c)
    {   //Collided with any rigidbody object
        if (!c.gameObject.GetComponent<Projectile>())
        {
            rb.AddForce(((gameObject.transform.position - c.gameObject.transform.position).normalized) * CollisionImpulse, ForceMode.Impulse);
            
            int impactDamage = Mathf.FloorToInt(c.relativeVelocity.magnitude);
            TakeDamage(c.gameObject.transform.position, impactDamage);
        }
    }

    public void DestroySelf()
    {
        //sm.GetComponent<GameManager>().AddScore(scoreValue);
        //if (type != -1) sm.GetComponent<EnemyManager>().RemoveShip(id, type);
        Instantiate(psDestructPrefab, transform.position, transform.rotation);
        aiManager.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    public void Initialise(GameObject ThePlayer, AIManager aiMngr, GameObject sceneManager, float difficultyBonus)
    {
        player = ThePlayer;
        aiManager = aiMngr;
        SceneManagerObject = sceneManager;
        //now update Base score Value to scaled value
        scoreValue = (int)(scoreValue * (1.0f + difficultyBonus));
    }


    public void UpdateStats(float statbonus)
    {   //used by UI_Upgrade
        stats.Health.SetBonusMod(statbonus);
        stats.Shield.SetBonusMod(statbonus);
        stats.Attack.SetBonusMod(statbonus);
        stats.Special.SetBonusMod(statbonus);
        stats.Speed.SetBonusMod(statbonus);
        stats.Fuel.SetBonusMod(statbonus);
        //now recalculate
        stats.RecalculateStats();

        //now propagate to arsenal
        arsenal.UpdateDamageFromAttackStat();

        
    }

    public bool GetAlive()
    {
        return stats.IsAlive();
    }
}
