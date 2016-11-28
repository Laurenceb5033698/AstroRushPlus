using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour 
{
    public GameObject pointer;
	public GameObject ship;  // ship gameobject
    public GameObject station;
    public GameObject mPreF; // missile prefab
	public TextMesh distanceDisplay;
	public Inputs controls;
	public AnimateThrusters thrusters;
	public Rigidbody rb; 	// ship's rigid body
	public Laser shipLaser;
    public ShipStats stats;
    public UI ui;
    public Camera cam;

    private float rotFix = 0f;

    // Mains --------------------------------------------------------------------------------------------------------
    void Start () // Use this for initialization
    {
       
	}
	void Update () // Update is called once per frame
    {
        controls.UpdateInputs();
        thrusters.UpdateThrusters();

		CheckInputs();
        thrusters.SetThrusterState(stats.IsShipWorking());
		if (stats.IsShipWorking())
            MoveShip ();


		UpdateUI ();
	}
	
    // FUNCTIONS --------------------------------------------------------------------------------------------------------	
	private void CheckInputs()
	{
		if (controls.reset) 
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			ship.transform.position = Vector3.zero;
            ship.transform.eulerAngles = Vector3.zero;
            stats.ResetShip ();
		}

        if (Vector3.Distance(ship.transform.position, station.transform.position) < 20f)
        {
            ui.UpdateStationPanelToggle(true);

            if (Input.GetAxis("DPadYAxis") > 0)
            {
                BuyButton();
            }
            if (Input.GetAxis("DPadYAxis") < 0)
            {
                SellButton();
            }
            if (Input.GetAxis("DPadXAxis") > 0)
            {
                RepairButton();
            }
        }
        else
        {
            ui.UpdateStationPanelToggle(false);
        }

        stats.LaserState = controls.RLaser;
			
		if (controls.rocket) 
		{
			if (stats.LoadMissile ()) 
			{
				SpawnMissile ();
				//stats.DecreaseMissileAmount (); // do not decrement missiles until we make an option to reload
			} 
			else
            {
                Debug.Log("Out of Missiles!");
            }
		}
	}
	private void MoveShip()
    { 
        // Reset unwanted xyz rotation and velocity --------------------------------------------------------------------------------------------
        rb.velocity = new Vector3(rb.velocity.x, 0.00f, rb.velocity.z);
        rb.angularVelocity = new Vector3(0.00f, rb.angularVelocity.y, 0.00f);

        if (rb.velocity.magnitude < 2f && Time.time > rotFix)
        {
            rotFix = Time.time + 1f;
            // THIS CAUSES THE SHIP TO JITTER (<---)
            ship.transform.position = new Vector3(ship.transform.position.x, 0.00000f, ship.transform.position.z); //   <---------
            ship.transform.eulerAngles = new Vector3(0f, ship.transform.eulerAngles.y, 0f); // fix the weird rotation applied to x and z axis   // <--------------
        }
        //--------------------------------------------------------------------------------------------------------------------------------------

        float tempFuelUsed = 0f;
        float tempFuelUsage = stats.GetFuelUsage();

        if (controls.boost) 
		{
			rb.AddForce (ship.transform.right * stats.GetBoostSpeed());
			tempFuelUsed += tempFuelUsage * 10;
		}

        rb.AddForce(ship.transform.right * (controls.zAxis * 200f) * Time.deltaTime);
        tempFuelUsed += Mathf.Abs(controls.zAxis) * tempFuelUsage * Time.deltaTime;

        rb.AddForce(ship.transform.forward * (controls.xAxis * -200f) * Time.deltaTime);
        tempFuelUsed += Mathf.Abs(controls.xAxis) * tempFuelUsage * Time.deltaTime;

        rb.AddTorque(ship.transform.up * (controls.yawAxis * stats.GetRotSpeed()) * Time.deltaTime);
        tempFuelUsed += Mathf.Abs(controls.yawAxis) * tempFuelUsage * Time.deltaTime;

        stats.ShipFuel = -tempFuelUsed;
    }	
	private void SpawnMissile ()
	{
		Vector3 shipPos = ship.transform.position;
		Vector3 shipDirection = ship.transform.right;
		Quaternion shipRotation = ship.transform.rotation;
		Vector3 RocketRot = shipPos + (-shipDirection);


		GameObject temp = (GameObject)Instantiate (mPreF,shipPos + (shipDirection * 6f),shipRotation);
        temp.GetComponent<Rigidbody> ().velocity = rb.velocity * 2f;
		temp.transform.LookAt(RocketRot);
		temp.transform.Rotate (-90,0,0);
	}
    private void UpdateUI()
    {
        //ui.UpdateStationPanelToggle(Vector3.Distance(ship.transform.position, station.transform.position) < 20f);

        string tempCargo = "" + stats.ShipCargo.ToString("F2") + "/" + stats.GetMaxCargoSpace();
        ui.UpdateShipStats(stats.Units, stats.ShipFuel, tempCargo, stats.ShipDamage);

        distanceDisplay.text = Vector3.Distance(ship.transform.position, station.transform.position).ToString("F2");
        distanceDisplay.transform.position = new Vector3(pointer.transform.position.x + 1.21f, pointer.transform.position.y + 1.46f, pointer.transform.position.z + 1.79f);

        distanceDisplay.transform.eulerAngles = new Vector3(45f,cam.transform.eulerAngles.y,0f); // make text face camera
    }
    // EVENT HANDLERS-------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision c)
	{
		stats.ShipDamage = Convert.ToInt32(c.relativeVelocity.magnitude)/2;
	}
	void BuyButton()
	{

		Debug.Log ("buy button is pressed");
		int tempPrice = station.GetComponent<StationManager> ().GetFuelPrice ();
		int tempCost = Convert.ToInt32 ((100 - stats.ShipFuel) * tempPrice);
        

		if (tempCost <= stats.Units) 
		{
            stats.Units = -tempCost; // in the background (units += -tempcost)
			stats.ShipFuel = (100 - stats.ShipFuel);
		} 
		else 
		{
			stats.ShipFuel = (stats.Units/tempPrice);
		}
	}
	void SellButton()
	{
        Debug.Log("sell button is pressed");
        int tempUnits = Convert.ToInt32(stats.ShipCargo * station.GetComponent<StationManager>().GetCargoPrice());
		stats.Units = tempUnits;
        stats.ShipCargo = -stats.ShipCargo;
	}
	void RepairButton()
	{
        Debug.Log("repair button is pressed");
        int tempPrice = station.GetComponent<StationManager> ().GetCargoPrice ();
		int tempCost = stats.ShipDamage * tempPrice;

		if (tempCost <= stats.Units) 
		{
            stats.Units = -tempCost;
			stats.ShipDamage = -stats.ShipDamage; // set ship damage value to -ship damage value. by doing this the value will be 0
		} 
		else 
		{
			stats.ShipDamage = (stats.Units / tempPrice);
		}
	}



}
