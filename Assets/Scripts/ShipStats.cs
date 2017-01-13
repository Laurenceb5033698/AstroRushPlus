using UnityEngine;
using System.Collections;

public class ShipStats : MonoBehaviour {

	// game currency
	private int units = 5000;

	// Thruster Variables -------------------------------------------
	private const float mainThrust = 50f;
	private const float rotSpeed = 350f;
	private const float boostSpeed = 1500f;
    private bool boostMinCutoff = false;


	// WEAPONS
	private int MissileAmount = 20;

	// FUEL
	private float boostFuel = 100f;

	// CARGO
	private float cargo = 0;
	private const float MaxcargoSpace = 1000;

	// DAMAGE
    private float health = 100;
    private const int maxHealth = 100;
    private float shield = 0f;
    private const int maxShield = 40;

    private bool currentlyInCombat = false;
    private float outOfCombatTimer = 0;

	// LASER
	private bool laserIsOn = false;
	private float laserSpeed = 0.2f;
	private float laserRange = 50f;
	private const float laserWidth = 0.2f;

	//-----------------------------------------------------------------------------------------

	// GET
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

	public int GetNoMissiles() 
	{ 
		return MissileAmount;
	}

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

	public float GetMaxCargoSpace()
	{
		return MaxcargoSpace;
	}

	// SET
	public void DecreaseMissileAmount() 
	{ 
		MissileAmount -= 1; 
	}




    // GET & SET
    public bool bco // boost cut off
    {
        get { return boostMinCutoff; }
        set { boostMinCutoff = value; }
    }


    public bool LaserState
    {
        get { return laserIsOn; }
        set { laserIsOn = value; }
    }
    public void takeDamage(float val)
    {
        inCombat = true;
        if (ShipShield > 0) //if we have shields
            if (shield - val > 0)   //and the damage taken is lower than sheild health
                ShipShield = -val;   //do damage to shield
            else
            {//otherwise split damage between sheild and health
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
    public int Units
    {
        get { return units; }
        set { units += value; }
    }
    public float ShipCargo
    {
        get { return cargo; }
        set 
        {
            if (value > 0)
            {
				cargo = (cargo + value > MaxcargoSpace) ? MaxcargoSpace : cargo + value;
            }
            else if (value < 0)
            {
                cargo = (cargo + value < 0) ? 0f : cargo + value;
            }
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

    public void addMissile(int amount)
    {
        MissileAmount = (MissileAmount + amount > 20) ? 20 : MissileAmount + amount;
    }
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

	// Validate
	public bool LoadMissile() 
	{ 
		return (MissileAmount > 0) ? true : false; 
	}
	public bool IsShipWorking()
	{
        return (health > 0);
	}
		

	// RESET FUNCTIONS
	public void ResetShip()
	{
		boostFuel = 100;
		cargo = 0;
        health = maxHealth;
        shield = maxShield;
        MissileAmount = 20;

        inCombat = false;
	}
    public void regenerateShield()
    {
        if ((!inCombat) && (ShipShield < maxShield))
            ShipShield = 5 * Time.deltaTime;
    }


}
