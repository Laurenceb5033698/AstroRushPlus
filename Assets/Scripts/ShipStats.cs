using UnityEngine;
using System.Collections;

public class ShipStats : MonoBehaviour {

	// game currency
	private int units = 5000;

	// Thruster Variables -------------------------------------------
	private const float mainThrust = 400f;
	private const float rotSpeed = 1500f;
	private const float boostSpeed = 1000f;


	// WEAPONS
	private int MissileAmount = 20;

	// FUEL
	private float boostFuel = 100f;

	// CARGO
	private float cargo = 0;
	private const float MaxcargoSpace = 1000;

	// DAMAGE
	private int damage = 0;

	// LASER
	private bool laserIsOn = false;
	private float laserSpeed = 0.05f;
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
    public bool LaserState
    {
        get { return laserIsOn; }
        set { laserIsOn = value; }
    }
    public int ShipDamage
    {
        get { return damage; }
        set 
        {
            if (value > 0)
            {
                damage = (damage + value > 100) ? 100 : damage + value;
            }
            else if (value < 0)
            {
                damage = (damage + value < 0) ? 0 : damage + value;
            }
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



	// Validate
	public bool LoadMissile() 
	{ 
		return (MissileAmount > 0) ? true : false; 
	}
	public bool IsShipWorking()
	{
		return (damage < 100);
	}
		

	// RESET FUNCTIONS
	public void ResetShip()
	{
		boostFuel = 100;
		cargo = 0;
		damage = 0;
        MissileAmount = 20;
	}




}
