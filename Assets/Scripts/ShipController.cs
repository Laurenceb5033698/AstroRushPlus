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
	public TextMesh distanceDisplay;

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
		rb.centerOfMass = ship.transform.position;
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
        angleVel = rb.angularVelocity; // for testing only
	}

    private void UpdatePointer()
    {
        Vector3 posA = ship.transform.position;
        Vector3 posB = station.transform.position;

		Vector3 pointDir = (posB - posA).normalized;
		pointer.transform.position = posA + pointDir * 8f;
		pointer.transform.rotation = Quaternion.LookRotation(-pointDir);
    }
		
	private void UpdateUI()
	{
		string tempCargo = "" + stats.ShipCargo.ToString("F2") + "/" + stats.GetMaxCargoSpace ();
		ui.UpdateShipStats (stats.Units,stats.ShipFuel,tempCargo,stats.ShipDamage);

		distanceDisplay.text = Vector3.Distance (ship.transform.position, station.transform.position).ToString("F2");
		distanceDisplay.transform.position = new Vector3 (pointer.transform.position.x + 1.21f,pointer.transform.position.y + 1.46f,pointer.transform.position.z + 1.79f);
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

		// this is for world axis movement controls
		//rb.AddForce (Vector3.right * controls.xAxis * stats.GetMainThrust() * Time.deltaTime);
		//rb.AddForce (Vector3.forward * controls.zAxis * stats.GetMainThrust() * Time.deltaTime);

		// this is for ship axis controls
		/*
		float shipRot = ship.transform.transform.localEulerAngles.y;
		float xAxisValue = (shipRot >= 45 && shipRot <= 135) ? -controls.xAxis : controls.xAxis;  // invert side-ways movement when ship is facing down
		rb.AddForce (ship.transform.forward * (xAxisValue * -stats.GetMainThrust()) * Time.deltaTime); // x axis
		*/

		rb.AddForce (ship.transform.forward * (controls.xAxis * -stats.GetMainThrust()) * Time.deltaTime); // x axis
		rb.AddTorque (ship.transform.up * (controls.yawAxis * stats.GetRotSpeed()) * Time.deltaTime);
		rb.AddForce (ship.transform.right * (controls.zAxis * stats.GetMainThrust()) * Time.deltaTime); // z axis


		if (controls.zAxis != 0)   tempFuelUsed += Mathf.Abs(controls.zAxis) * tempFuelUsage * Time.deltaTime;
		if (controls.xAxis != 0)   tempFuelUsed += Mathf.Abs(controls.xAxis) * tempFuelUsage * Time.deltaTime;
		if (controls.yawAxis != 0) tempFuelUsed += Mathf.Abs(controls.yawAxis) * tempFuelUsage * Time.deltaTime;

        stats.ShipFuel = -tempFuelUsed;
    }

	private void SetShipToYPlane()
	{
		rb.velocity = new Vector3(rb.velocity.x,0.00f,rb.velocity.z);
        rb.angularVelocity = new Vector3(0.00f, rb.angularVelocity.y,0.00f);
		ship.transform.position = new Vector3(ship.transform.position.x,0.00f,ship.transform.position.z);
		ship.transform.eulerAngles = new Vector3(0f,ship.transform.eulerAngles.y,0f); // fix the weird rotation applied to x and z axis
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
		stats.ShipDamage = Convert.ToInt32(c.relativeVelocity.magnitude)/2;
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
