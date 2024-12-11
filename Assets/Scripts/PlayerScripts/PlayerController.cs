using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; // for controller rumble


abstract public class PlayerController : MonoBehaviour {

    protected GameObject ship;  // ship gameobject
    //controller number and rumble vars
    protected bool playerIndexSet = false;
    protected PlayerIndex playerIndex;
    protected GamePadState state;
    protected GamePadState prevState;
    protected Inputs controls;
    protected float rumbleTimer = 0;
    //compoent vars
    protected Rigidbody rb; 	// ship's rigid body
    protected Stats stats;
    [SerializeField] protected ParticleSystem shield_Emitter;
    [SerializeField] public Arsenal arsenal;
    [SerializeField] protected Equipment equipment;
    
    protected int weaponType = 0;

    //input varaibles
    protected bool aiming = false;
    protected Vector3 direction = Vector3.zero;
    protected bool usedEquipment = false;
    protected bool UsingAbility = false;

    //fuel usage 
    [SerializeField] private int fuelConsumeRate = 1;
    [SerializeField] private float fuelConsumePeriod = 1.0f;
    private float fuelConsumetimer = 0.0f;

    protected void Awake()
    {
        Debug.Log("PlayerController Awake.");
        ship = transform.gameObject;
        rb = ship.GetComponent<Rigidbody>();
        stats = ship.GetComponent<Stats>();
        arsenal = ship.GetComponentInChildren<Arsenal>();
        arsenal.SetShipObject(ship);
        //single weapon "Equipment" is a collection for Ordinance
        equipment = ship.GetComponentInChildren<Equipment>();
        equipment.SetShipObject(ship);
    }

    protected void Start()
    {
        arsenal.RegisterUI();
    }

    protected void Update()
    {
        UpdateController();
        direction = transform.right;

        if (!stats.GetDisabled())   //can only be controlled if not disabled/emp'd
        {
            ///Handle inputs:
            // Mouse Buttons
            InputLMB();
            InputRMB();

            // Analogs
            InputLeftAnalog();
            InputRightAnalog();

            //DpadY
            InputDpadYAxis();

            // Triggers
            InputLeftTrigger();
            InputRightTrigger();

            // Bumpers
            InputLeftBumper();
            InputRightBumper();
        }
        rb.angularVelocity = new Vector3(0, 0, 0);

    }

    protected void FixedUpdate()
    {
        if (stats.ShipShield < 0.1f) stats.ActivateShieldPU();
        if (rumbleTimer > Time.time)
            GamePad.SetVibration(playerIndex, 0.5f, 0.5f);
        else
            GamePad.SetVibration(playerIndex, 0, 0);

        //process fuel timer
        if(fuelConsumetimer > 0.0f)
        {
            fuelConsumetimer -= Time.deltaTime;
        }
    }

    
    ///################################################################
    /// Player Inputs #################################################
    /// Made Into functions for inherited classes to override as needed
    ///currently functions as boost ship by default
    
    //####  ANALOG STICKS
    virtual protected void InputLeftAnalog()
    {
        if ( stats.IsAlive() && controls.LeftAnalogueInUse )
            MoveShip();
        else {
            rumbleTimer = 0;
            GamePad.SetVibration(playerIndex, 0, 0);
        }
    }
    virtual protected void InputRightAnalog()
    {
        aiming = controls.RightAnalogueInUse;
        if (aiming)
        {   //fire weapon
            direction = new Vector3(controls.RightStick.x, 0, controls.RightStick.y).normalized;
            arsenal.FireWeapon(direction);
        }
    }

    //#### DPAD ####
    virtual protected void InputDpadYAxis()
    {
        if (controls.DpadYPressed)
        {
            if (controls.DpadUp || Input.GetKeyDown(KeyCode.Q)) // DPad up
            {
                equipment.ChangeGun(1);
            }
            if (controls.DpadDown) // DPad down
            {//perhaps else for optimization
                equipment.ChangeGun(-1);
            }
        }
    }

    //####  TRIGGERS    ####
    virtual protected void InputLeftTrigger()
    {
        if (controls.LTriggerInUse)
            UsingAbility = true;
        else
            UsingAbility = false;

    }
    virtual protected void InputRightTrigger()
    {
        if (controls.RTriggerPressed || Input.GetKeyDown(KeyCode.Space))
        { //depends on direction being resolved by aiming||LMB
            if (!usedEquipment && equipment.HasAmmo())
            {
                usedEquipment = true;
                equipment.UseOrdinance(direction);
            }
        }
        if (!controls.RTriggerPressed)
            usedEquipment = false;
    }

    //####  BUMPERS     ####
    virtual protected void InputLeftBumper()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
            weaponType = arsenal.ChangeGun(-1);
    }
    virtual protected void InputRightBumper()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
            weaponType = arsenal.ChangeGun(1);
    }

    //#### MOUSE BUTTONS    ####
    virtual protected void InputLMB()
    {
        if (Input.GetMouseButton(0) && !aiming)
        {   //fire weapon
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                direction = (targetPoint - transform.position).normalized;
                arsenal.FireWeapon(direction);
                //Debug.Log(direction);
            }
        }
    }
    virtual protected void InputRMB()
    {
        if (Input.GetMouseButtonDown(1))
            weaponType = arsenal.ChangeGun(1);

    }

    //###########################################
    //###########################################

    abstract protected void MoveShip();

    // FUNCTIONS --------------------------------------------------------------------------------------------------------	
    protected void UpdateController()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }
    // EVENT HANDLERS-------------------------------------------------------------------------------------
    virtual protected void OnCollisionEnter(Collision c)
    {
        int impactDamage = Mathf.FloorToInt(c.relativeVelocity.magnitude/4);
        TakeDamage(c.transform.position, impactDamage);
        if (c.gameObject.tag == "EnemyShip")
        {
            c.gameObject.GetComponent<AICore>().TakeDamage(transform.position, impactDamage);
        }
    }
    protected void Shield_effect(Vector3 other)
    {
        //Debug.Log("Collision Entered: " + other.gameObject.name);
        Vector3 dir = other - shield_Emitter.transform.position;
        dir.Normalize();
        shield_Emitter.transform.rotation = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0, -90, 0);

        shield_Emitter.Play();
        GetComponentInChildren<Animation>().Stop();
        GetComponentInChildren<Animation>().Play();

    }

    virtual public void TakeDamage(Vector3 otherpos, int amount)
    {
        if (stats.ShipShield > 0)
            Shield_effect(otherpos);

        stats.TakeDamage(amount);
        rumbleTimer = Time.time + 0.3f;
    }

    public void UpdateStats(float bHp, float bSh, float bAt, float bSp, float bSd, float bFl)
    {   //used by UI_Upgrade
        stats.Health.SetBonusMod(bHp);
        stats.Shield.SetBonusMod(bSh);
        stats.Attack.SetBonusMod(bAt);
        stats.Special.SetBonusMod(bSp);
        stats.Speed.SetBonusMod(bSd);
        stats.Fuel.SetBonusMod(bFl);
        //now recalculate
        stats.RecalculateStats();

        //now propagate to arsenal
        arsenal.UpdateDamageFromAttackStat();
    }

    virtual protected void SpendShipFuel()
    {
        //remove fuel each period (defualt 1s)
        if (fuelConsumetimer <= 0.0f)
        {
            fuelConsumetimer = fuelConsumePeriod;
            stats.ShipFuel = -fuelConsumeRate;
        }
    }

    //UTIL
    public int GetWeaponType()
    {
        return weaponType;
    }
    void OnApplicationQuit()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    void ShootVolumeChanged(bool x)
    {
        arsenal.volumeChanged(PlayerPrefs.GetFloat("gameVolume") / 10);
    }

    public void SetInputs(Inputs glblinputs)
    {
        controls = glblinputs;
    }
}
