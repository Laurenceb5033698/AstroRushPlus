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
    [SerializeField] protected Shield m_ShieldVisuals;
    ShipRotationHandler tilthandler;

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
        m_ShieldVisuals = GetComponentInChildren<Shield>();
        tilthandler = GetComponentInChildren<ShipRotationHandler>();

        //set upgrade stats
        GetComponent<UpgradeManager>().shipStats = stats;

        arsenal = GetComponentInChildren<Arsenal>();        //local weapons platform
    }

    protected void Start () {
        if (arsenal == null) 
            Debug.Log("No weapon attached.");
        else
            arsenal.SetShipObject(gameObject);
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
        float currentSpeed = GetMaxSpeed();//use speeds from Stats. Change on prefabs

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
        ShipTilt();
    }

    protected void ShipTilt()
    {
        float newSpeed = rb.linearVelocity.magnitude;

        //calculate tilt amount
        //if speed > amount then tilt more
        float tiltModifier = 0.5f;
        if (newSpeed > GetMaxSpeed() / 2)
        {
            tiltModifier = newSpeed / GetMaxSpeed();
        }
        //rotate visualrig by tilt amount
        tilthandler.Tilt(controlDir.normalized, tiltModifier);
    }

    protected void LateUpdate()
    {   //Align ship to y-plane (stops unwanted wobbling)
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //if(transform.position.y > 1 || transform.position.y < -1)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        //}
    }


    virtual protected void Shield_effect(Vector3 _dmgPos)
    {
        m_ShieldVisuals.ShieldHit(_dmgPos);
    }

    virtual public void TakeDamage(EventSource _offender, Vector3 otherpos, float amount)
    {   //Default method for Taking Damage
        if (m_ShieldVisuals != null && stats.ShipShield > 0)
            Shield_effect(otherpos);
        Stats.OnDamageReturn ret = stats.TakeDamage(amount);
        if (_offender)
        {
            if (ret != Stats.OnDamageReturn.None)
            {
                if (ret == Stats.OnDamageReturn.Damaged)
                    _offender.OnDamageEvent(this.gameObject);
                else
                    _offender.OnKillEvent(this.gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision c)
    {
        if (!c.gameObject.GetComponent<Projectile>())
            rb.AddForce(((gameObject.transform.position - c.gameObject.transform.position).normalized) * 100, ForceMode.Force);
    }

    void OnCollisionEnter(Collision c)
    {   //Collided with any rigidbody object
        Damageable damageable = c.gameObject.GetComponent<Damageable>();           
        if (!c.gameObject.GetComponent<Projectile>())
        {
            rb.AddForce(((gameObject.transform.position - c.gameObject.transform.position).normalized) * CollisionImpulse, ForceMode.Impulse);
            //no damage if collision was sufficiently slow
            if (c.relativeVelocity.magnitude > 5)
            {   //enemies generally slower than player. max velocity impact normally <60
                //1 damage minimum on impact, takes more damage from fast collision.
                float velocityDamage = c.relativeVelocity.magnitude;
                int impactDamage = 1 + Mathf.FloorToInt(velocityDamage / 20);
                TakeDamage(damageable?damageable.GetEventSource():null, c.gameObject.transform.position, impactDamage);
            }
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
        stats.block.Get(StatType.sHealth).SetBonusMod(statbonus);
        stats.block.Get(StatType.sShield).SetBonusMod(statbonus);
        stats.block.Get(StatType.gAttack).SetBonusMod(statbonus);
        stats.block.Get(StatType.sSpecial).SetBonusMod(statbonus);
        stats.block.Get(StatType.sSpeed).SetBonusMod(statbonus);
        stats.block.Get(StatType.sTurnrate).SetBonusMod(statbonus);
        stats.block.Get(StatType.sFuel).SetBonusMod(statbonus);

        //now propagate to arsenal
        arsenal.UpdateDamageFromAttackStat();
    }

    //#######
    //util
    public bool GetAlive()
    {
        return stats.IsAlive();
    }

    private float GetMaxSpeed()
    {
        if (dist <= innerRange)
        {
            return stats.GetMainThrust() * 1.5f;
        }
        return stats.GetMainThrust();
    }
}
