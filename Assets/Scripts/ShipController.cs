using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour 
{
	public ShipStats stats;

	// UI elements -----------------------------------------------
	public GameObject SSIpanel;
	public UI ui;
	public GameObject station;
    public GameObject pointer;

	// ship parts ------------------------------------------------
	private GameObject target;
	public GameObject ship;	 // ship gameobject
	public GameObject mPreF; // missile prefab
	public Camera mainC;

	// Ship Components -------------------------------------------
	public Inputs controls;
	public AnimateThrusters thrusters;
	public Rigidbody rb; 	// ship's rigid body
	public Laser shipLaser;
	// -----------------------------------------------------------

    public Vector3 angleVel;


	void Start () // Use this for initialization
    {
		target = station;
		SSIpanel.SetActive (false);
	}
	void Update () // Update is called once per frame
    {
        UpdatePointer();
		CheckInputs();
		ToggleStationWindow ();

		if (stats.IsShipWorking()) 
		{
			thrusters.SetThrusterState (true);
			MoveShip ();
		}
		else {
			thrusters.SetThrusterState (false);
		}
		
		UpdateUI ();
        angleVel = rb.angularVelocity;
	}

    private void UpdatePointer()
    {
        // REALLY BUGGY
        Vector3 posA = ship.transform.position;
        Vector3 posB = station.transform.position;
        
        const float dist = 8f;
        Vector3 pointDir = Vector3.zero;
        Quaternion stationDir = Quaternion.identity;

        pointDir = (posB - posA).normalized;
        stationDir = Quaternion.LookRotation(-pointDir);

        Quaternion sRot = ship.transform.rotation;

        Vector3 pdir = pointer.transform.position - posA;
        pdir = Quaternion.Euler(pointDir) * pdir;
        pointer.transform.position = pdir + posA;

        //pointer.transform.position = new Vector3(posA.x + Mathf.Sin(pointDir.y) + dist, posA.y, posA.z + Mathf.Cos(pointDir.y));
        pointer.transform.localRotation = stationDir;

        



    }
		
	private void UpdateUI()
	{
		float temp = stats.ShipCargo / stats.GetMaxCargoSpace() * stats.ShipCargo;
		ui.UpdateShipStats (stats.Units,stats.ShipFuel,temp,stats.ShipDamage);
	}
	private void CheckInputs()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			ship.transform.position = Vector3.zero;
			stats.ResetShip ();
		}

        stats.LaserState = controls.RLaser;

		if (Input.GetMouseButtonDown (0)) {
			FindTarget (mainC);
		}
			
		if (controls.rocket) 
		{
			if (stats.LoadMissile ()) 
			{
				SpawnMissile ();
				stats.DecreaseMissileAmount ();
			} 
			else 
			{
				Debug.Log ("Out of Missiles!");
			}
		}
	}
	private void MoveShip()
    { 
		float tempFuelUsed = 0f;
		float tempFuelUsage = stats.GetFuelUsage ();
		SetShipToYPlane ();

		if (controls.boost) 
		{
			rb.AddForce (ship.transform.right * stats.GetBoostSpeed());
			tempFuelUsed += tempFuelUsage * 10;
		}
			
		rb.AddForce (ship.transform.right * (controls.zAxis * stats.GetMainThrust()) * Time.deltaTime);
		rb.AddForce (ship.transform.forward * (controls.xAxis * -stats.GetMainThrust()) * Time.deltaTime);
		rb.AddTorque (ship.transform.up * (controls.yawAxis * stats.GetRotSpeed()) * Time.deltaTime);

		if (controls.zAxis != 0)   tempFuelUsed += Mathf.Abs(controls.zAxis) * tempFuelUsage * Time.deltaTime;
		if (controls.xAxis != 0)   tempFuelUsed += Mathf.Abs(controls.xAxis) * tempFuelUsage * Time.deltaTime;
		if (controls.yawAxis != 0) tempFuelUsed += Mathf.Abs(controls.yawAxis) * tempFuelUsage * Time.deltaTime;

        stats.ShipFuel = -tempFuelUsed;
    }

	private void SetShipToYPlane()
	{
		rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.angularVelocity = new Vector3(0f, rb.angularVelocity.y,0f);
		ship.transform.position = new Vector3(ship.transform.position.x,0f,ship.transform.position.z);
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

	public void FindTarget(Camera c)
	{
		RaycastHit hitInfo;
		Ray ray = c.ScreenPointToRay (Input.mousePosition);
		bool hit = Physics.Raycast(ray, out hitInfo);

		if (hit) 
		{
			Debug.Log ("Targeting " + hitInfo.transform.gameObject.name);

			if (hitInfo.transform.gameObject.name == "SpaceStation") 
			{
				target = hitInfo.transform.gameObject;
				SSIpanel.SetActive (true);
			} else {
				SSIpanel.SetActive (false);
			}
		}


	}

	private void ToggleStationWindow()
	{
		if (Vector3.Distance (ship.transform.position, station.transform.position) < 20f) {
			SSIpanel.SetActive (true);
		}
		else {
			SSIpanel.SetActive (false);
		}
	}


	void OnCollisionEnter(Collision c)
	{
		stats.ShipDamage = Convert.ToInt32(c.relativeVelocity.magnitude);
	}

	void BuyButton()
	{

		Debug.Log ("buy button is pressed");
		int tempPrice = target.GetComponent<StationManager> ().GetFuelPrice ();
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
        int tempUnits = Convert.ToInt32(stats.ShipCargo * target.GetComponent<StationManager>().GetCargoPrice());
		stats.Units = tempUnits;
        stats.ShipCargo = -stats.ShipCargo;
	}
	void RepairButton()
	{
		int tempPrice = target.GetComponent<StationManager> ().GetCargoPrice ();
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
