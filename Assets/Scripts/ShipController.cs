using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour 
{
	// Player Variables -------------------------------------------
	public int units = 5000;


	// Thruster Variables -------------------------------------------
	private const float mainThrust = 100f;
	private const float sideThrust = 50f;
	private const float frontThrust = 40f;
	private const float rotSpeed = 80f;
	private const float boostSpeed = 200f;
	private const float FullFThrustSpeed = 100f;

	// Ship Variables -------------------------------------------
	private bool shipIsWorking = true;
	private float fuelUsage = 0.005f; // per second per thruster
	private float fuel = 100f;
	private float cargo = 0;
	private const float MaxcargoSpace = 1000;

	private int damage = 0;
	private float mineSpeed = 0.05f;

	private const float stationInteractionDistance = 20f;
	// UI elements -----------------------------------------------
	public GameObject SSIpanel;
	public Text un;
	public Text fu;
	public Text ca;
	public Text da;

	// ship parts ------------------------------------------------
	private GameObject target;
	public GameObject ship;	 // ship gameobject
	public GameObject mPreF; // missile prefab

	// Ship Components -------------------------------------------
	public Inputs controls;
	public AnimateThrusters thrusters;
	public Rigidbody rb; 	// ship's rigid body
	public Laser shipLaser;
	// -----------------------------------------------------------


	void Start () // Use this for initialization
    {
		SSIpanel.SetActive (false);
	}
	void Update () // Update is called once per frame
    {
		PreChecks ();
		CheckInputs();

		if (shipIsWorking) {
			thrusters.SetThrusterState (true);
			MoveShip ();
		}
		else {
			controls.ThrustLevel = 0f;
			thrusters.SetThrusterState (false);
		}
		
		UpdateUI ();
	}

	private void PreChecks()
	{
		shipIsWorking = (fuel > 0 && damage < 100);

		if (fuel < 0)
			fuel = 0;
		
		if (damage > 100)
			damage = 100;
	}
	private void UpdateUI()
	{
		un.text = "Units: " + units;
		fu.text = "Fuel: " + fuel.ToString ("F2") + "%";
		float temp = cargo / MaxcargoSpace * cargo;
		ca.text = "Cargo: " + temp.ToString ("F2") + "%";
		da.text = "Damage: " + damage + "%";
	}
	private void CheckInputs()
	{
		if (Input.GetKeyDown (KeyCode.R)) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			//rb.rotation = Quaternion.identity;
		}
			
		if (controls.RLaser)
		{
			MineAsteroid();
		}

		if (Input.GetMouseButtonDown (0)) {
			FindTarget (controls.ThirdP);
		}
			
		if (controls.rocket)
			SpawnMissile ();
	}
	private void MoveShip()
    {
		if (controls.ThrustLevel > 0) {
			rb.AddForce (ship.transform.right * (FullFThrustSpeed / 100 * controls.ThrustLevel) * Time.deltaTime);
			fuel -= ((fuelUsage * 10 / 100) * controls.ThrustLevel) * Time.deltaTime;
		} 
		else if (controls.forward) {
			rb.AddForce (ship.transform.right * mainThrust * Time.deltaTime);
			fuel -= fuelUsage * 2 * Time.deltaTime;
		}
		if (controls.boost) {
			rb.AddForce (ship.transform.right * boostSpeed);
			fuel -= fuelUsage * 10;
		}

		if (controls.backward) {
			rb.AddForce (-ship.transform.right * frontThrust * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}

		if (controls.left) {
			rb.AddForce (ship.transform.forward * sideThrust * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}
		if (controls.right) {
			rb.AddForce (-ship.transform.forward * sideThrust * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}

		if (controls.up) {
			rb.AddForce (ship.transform.up * sideThrust * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}
		if (controls.down) {
			rb.AddForce (-ship.transform.up * sideThrust * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}

		if (controls.rollLeft) {
			rb.AddTorque (ship.transform.right * rotSpeed * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}
		if (controls.rollRight) {
			rb.AddTorque (ship.transform.right * -rotSpeed * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}

		if (controls.PitchUp) {
			rb.AddTorque (ship.transform.forward * rotSpeed * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}
		if (controls.PitchDown) {
			rb.AddTorque (ship.transform.forward * -rotSpeed * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}

		if (controls.yawLeft) {
			rb.AddTorque (ship.transform.up * -rotSpeed * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}
		if (controls.yawRight) {
			rb.AddTorque (ship.transform.up * rotSpeed * Time.deltaTime);
			fuel -= fuelUsage * Time.deltaTime;
		}


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
	void OnCollisionEnter(Collision c)
	{
		damage += Convert.ToInt32(c.relativeVelocity.magnitude);
	}
	public void BuyButton()
	{
		if (Vector3.Distance (ship.transform.position, target.transform.position) < stationInteractionDistance) {
			int tempPrice = target.GetComponent<StationManager> ().GetFuelPrice ();
			int tempCost = Convert.ToInt32 ((100 - fuel) * tempPrice);


			if (tempCost <= units) {
				units -= tempCost;
				fuel = 100;
			} else {
				fuel += units / tempPrice;
			}
		} else
			Debug.Log ("Station out of range");
	}
	public void SellButton()
	{
		if (Vector3.Distance (ship.transform.position, target.transform.position) < stationInteractionDistance) 
		{
			units += Convert.ToInt32(cargo * target.GetComponent<StationManager> ().GetCargoPrice ());
			cargo = 0;
		}
		else
			Debug.Log ("Station out of range");
	}
	public void RepairButton()
	{
		if (Vector3.Distance (ship.transform.position, target.transform.position) < stationInteractionDistance) {
			int tempPrice = target.GetComponent<StationManager> ().GetCargoPrice ();
			int tempCost = damage * tempPrice;

			if (tempCost <= units) {
				units -= tempCost;
				damage = 0;
			} else {
				damage -= units / tempPrice;
			}
		} else
			Debug.Log ("Station out of range");
	}

	public void MineAsteroid()
	{
		GameObject temp = shipLaser.GetTarget ();


		if (temp != null) 
		{
			if (temp.name == "Asteroid") 
			{
				if (temp.GetComponent<Asteroid> ().GetOreAmountLeft () > 0.1f) {
					if (cargo < MaxcargoSpace) {
						cargo += temp.GetComponent<Asteroid> ().MineOre (mineSpeed);
					}
				} else
					temp.GetComponent<Asteroid> ().DestroyAsteroid ();
			}
		}
	}
}
