using UnityEngine;
using System.Collections;

public class ShipStats : Health {

	// Health
    //private float health = maxHealth;
    private float shield;
    public bool GodMode = false;


    //Default values for new functional ships. Alter stats in prefabs
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxShield = 100;
	[SerializeField] private int maxMissiles = 20;

	// Thruster Variables -------------------------------------------
    //Default values for new functional ships. Alter stats in prefabs
    [SerializeField] private float mainThrust = 50f;
    [SerializeField] private float boostSpeed = 100f;
    [SerializeField] private float rotSpeed = 350f;
   
	private bool boostMinCutoff = false;
	private float boostFuel = 100f;


	// WEAPONS
	private int MissileAmount = 20;


    private bool currentlyInCombat = false;
    private float outOfCombatTimer = 0;


	private bool shieldPowerUp = false;

    void Awake()
    {
        health = maxHealth;
        shield = maxShield;
    }
    void Update()
    {
        //regenerateShield();
    }


	//-----------------------------------------------------------------------------------------
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
		MissileAmount = (MissileAmount + amount > maxMissiles) ? maxMissiles : MissileAmount + amount;
    }
    public bool LoadMissile()
    {
        return (MissileAmount > 0) ? true : false;
    }

    // health
    public override void TakeDamage(float val)
    {
        if (!GodMode)
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




	// pickup functions
	public void ActivateShieldPU(){
		if (shieldPowerUp) {
			shield = maxShield;
			shieldPowerUp = false;
		}
	}
	public void SetShieldPU()
	{
        ActivateShieldPU(); // if have one already, use it before getting the next one
        shieldPowerUp = true;
	}
    public bool GetShieldPUState()
    {
        return shieldPowerUp;
    }
	public void SetFuel()
	{
		ShipFuel = 100f;
	}
	public void SetHealth(){
		ShipHealth = maxHealth;
	}
	public void SetMissiles(){
		addMissile (maxMissiles);
	}
}
