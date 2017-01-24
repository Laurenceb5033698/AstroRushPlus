using UnityEngine;
using System.Collections;

public class ShipStats : Health {

	// Thruster Variables -------------------------------------------
	private const float mainThrust = 50f;
	private const float rotSpeed = 350f;
	private const float boostSpeed = 1500f;
    private bool boostMinCutoff = false;
	private float boostFuel = 100f;


	// WEAPONS
	private int MissileAmount = 20;

	// Health
    [SerializeField] private float shield = maxShield;
    [SerializeField] private const int maxHealth = 100;
    [SerializeField] private const int maxShield = 100;
    //private float health = maxHealth;

    private bool currentlyInCombat = false;
    private float outOfCombatTimer = 0;

	// LASER
	private bool laserIsOn = false;
	private float laserSpeed = 0.2f;
	private float laserRange = 30f;
	private const float laserWidth = 0.2f;
    private const float laserDamage = 50f;

    void Awake()
    {
        health = maxHealth;
    }
    void Update()
    {
        regenerateShield();
    }


	//-----------------------------------------------------------------------------------------

	// Laser
	public float GetLaserSpeed() 
	{ 
		return laserSpeed; 
	}
	public float GetLaserRange() 
	{ 
		return laserRange; 
	}
	public float GetLaserWidth() 
	{ 
		return laserWidth; 
	}
    public float GetLaserDamage()
    {
        return laserDamage;
    }
    public bool LaserState
    {
        get { return laserIsOn; }
        set { laserIsOn = value; }
    }

    // Speeds
    public float GetBoostFuelAmount()
	{
		return boostFuel;
	}
	public float GetBoostSpeed()
	{
		return boostSpeed;
	}
	public float GetMainThrust()
	{
		return mainThrust;
	}
	public float GetRotSpeed()
	{
		return rotSpeed;
	}
    public bool bco // boost cut off
    {
        get { return boostMinCutoff; }
        set { boostMinCutoff = value; }
    }

    // Missile
    public void DecreaseMissileAmount() 
	{ 
		MissileAmount -= 1; 
	}
    public int GetNoMissiles()
    {
        return MissileAmount;
    }
    public void addMissile(int amount)
    {
        MissileAmount = (MissileAmount + amount > 20) ? 20 : MissileAmount + amount;
    }
    public bool LoadMissile()
    {
        return (MissileAmount > 0) ? true : false;
    }

    // health
    public override void TakeDamage(float val)
    {
        inCombat = true;
        if (ShipShield > 0) //if we have shields
            if (shield - val > 0)   //and the damage taken is lower than sheild health
                ShipShield = -val;   //do damage to shield
            else
            {//otherwise split damage between shield and health
                ShipHealth = -(shield - val);   //remaining damage is delt to health
                ShipShield = -shield;        //and shield is set to 0
            }
        else
            ShipHealth = -val;   //shield is at 0, so deal damage directly to health
    }
    public float ShipHealth
    {
        get { return health; }
        set
        {
            if (value > 0)
            {
                health = (health + value > maxHealth) ? maxHealth : health + value;
            }
            else if (value < 0)
            {
                health = (health + value < 0) ? 0 : health + value;
            }
        }
    }
    public float ShipShield
    {
        get { return shield; }
        set
        {
            if (value > 0)
            {
                shield = (shield + value > maxShield) ? maxShield : shield + value;
            }
            else if (value < 0)
            {
                shield = (shield + value < 0) ? 0 : shield + value;
            }
            //Debug.Log("shield hp: " + shield);
        }
    }
    public float ShipFuel
    {
        get { return boostFuel; }
        set
        {
            if (value > 0)
            {
                boostFuel = (boostFuel + value > 100.0f) ? 100.0f : boostFuel + value;
            }
            else if (value < 0)
            {
                boostFuel = (boostFuel + value < 0.0f) ? 0.0f : boostFuel + value;
            }
        }

    }

    // validate
    public bool inCombat
    {
        get
        {
            if (outOfCombatTimer > 0)
                outOfCombatTimer -= Time.deltaTime;
            else
            {
                currentlyInCombat = false;
            }
            return currentlyInCombat;
        }
        set
        {
            if (value)
            {
                outOfCombatTimer = 8f;
                currentlyInCombat = true;
            }
            else
            {
                outOfCombatTimer = 0f;
                currentlyInCombat = false;
            }
        }
    }

    // other functions
    private void regenerateShield()
    {
        if ((!inCombat) && (ShipShield < maxShield))
            ShipShield = 5 * Time.deltaTime;
    }
}
