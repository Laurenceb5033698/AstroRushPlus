using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour 
{
	[SerializeField] private GameObject ship;  // ship gameobject
    [SerializeField] private GameObject station;
    [SerializeField] private GameObject mPreF; // missile prefab
	[SerializeField] private Inputs controls;
	[SerializeField] private AnimateThrusters thrusters;
	[SerializeField] private Rigidbody rb; 	// ship's rigid body
	[SerializeField] private Laser shipLaser;
    [SerializeField] private ShipStats stats;
    [SerializeField] private UI ui;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject boundaryz;
    [SerializeField] private GameObject boundaryx;
    [SerializeField] private GameObject shieldSphere = null;

    private const int SBOUND = 600;
    private const int HBOUND = SBOUND + 70;
    private float rotFix = 0f;

    // Mains --------------------------------------------------------------------------------------------------------
    void Start () // Use this for initialization
    {

	}
	void Update () // Update is called once per frame
    {
        controls.UpdateInputs();
        thrusters.UpdateThrusters();
        stats.regenerateShield();
        ShieldSphereOpacity();

        CheckInputs();

        if (stats.IsShipWorking())
        {
            thrusters.SetThrusterState(true);
            MoveShip();
        }
        else
        { 
            thrusters.SetThrusterState(false);
            ui.setMessage(0);
            ui.menu = true;
        }

        UpdateUI ();
        UpdateBoundary();

    }

    void FixedUpdate()
    {
        
    }
	
    // FUNCTIONS --------------------------------------------------------------------------------------------------------	
    public void ShieldSphereOpacity()
    {
        if (shieldSphere != null)
        {

            float newalpha = 0.5f * ((stats.ShipShield / 40));

            Color oldcol = shieldSphere.GetComponent<MeshRenderer>().materials[0].color;
            oldcol = new Color(oldcol.r, oldcol.g, oldcol.b, newalpha);
            shieldSphere.GetComponent<MeshRenderer>().materials[0].color = oldcol;
            //Debug.Log("shield amount: " + stats.ShipShield);
            if ((stats.ShipShield == 0))
            {
                shieldSphere.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                shieldSphere.GetComponent<MeshRenderer>().enabled = true;
            }

        }
    }

	private void CheckInputs()
	{
		if (controls.reset) ResetShip();

        ToggleStationPanel();
        stats.LaserState = controls.RLaser;
			
		if (controls.rocket) 
		{
			if (stats.LoadMissile ()) 
			{
				SpawnMissile ();
				stats.DecreaseMissileAmount ();
			} 
			else
            {
                Debug.Log("Out of Missiles!");
            }
		}
	}
	private void MoveShip()
    {
        CorrectShipTransforms();

       

        if (controls.boost && !stats.bco && stats.GetBoostFuelAmount() > 0.0f)
        {
            if (stats.GetBoostFuelAmount() < 0.5f)
            {
                stats.bco = true;
            }

            // up and down
            if (controls.zAxis > 0.1f)
            {
                //Debug.Log("BOOSTING UP");
                rb.AddForce(ship.transform.right * (stats.GetBoostSpeed()) * Time.deltaTime);
                stats.ShipFuel = -20 * Time.deltaTime;
            }
            else if (controls.zAxis < -0.1f)
            {
                //Debug.Log("BOOSTING DOWN");
                rb.AddForce(ship.transform.right * (-stats.GetBoostSpeed()) * Time.deltaTime);
                stats.ShipFuel = -20 * Time.deltaTime;
            }


            // left and right
            if (controls.xAxis > 0.1f)
            {
                //Debug.Log("BOOSTING RIGHT");
                rb.AddForce(ship.transform.forward * (-stats.GetBoostSpeed()) * Time.deltaTime);
                stats.ShipFuel = -20 * Time.deltaTime;
            }
            else if (controls.xAxis < -0.1f)
            {
                //Debug.Log("BOOSTING LEFT");
                rb.AddForce(ship.transform.forward * (stats.GetBoostSpeed()) * Time.deltaTime);
                stats.ShipFuel = -20 * Time.deltaTime;
            }
        }
        else
        {
            if (stats.GetBoostFuelAmount() >= 20f)
            {
                stats.bco = false;
            }
            stats.ShipFuel = 3 * Time.deltaTime;
        }

        // forward and backward
        rb.AddForce(ship.transform.right * (controls.zAxis * stats.GetMainThrust()) * Time.deltaTime);
        // left and right
        if (Mathf.Abs(controls.xAxis) > 0.1f) rb.AddForce(ship.transform.forward * (controls.xAxis * -stats.GetMainThrust()) * Time.deltaTime);
        else dampenSidewaysMotion();
        //rotate
        rb.AddTorque(Vector3.up * (controls.yawAxis * stats.GetRotSpeed()) * Time.deltaTime);
    }
    private void dampenSidewaysMotion()
    {
        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        //Debug.Log(locVel.z);

        locVel.z += -(locVel.z/100*80)*Time.deltaTime;
        rb.velocity = transform.TransformDirection(locVel);     
    }


    private void CorrectShipTransforms()
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
    }
    private void ResetShip()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ship.transform.position = Vector3.zero;
        ship.transform.eulerAngles = Vector3.zero;
        stats.ResetShip();
    }

    private void SpawnMissile ()
	{
		Vector3 shipPos = ship.transform.position;
		Vector3 shipDirection = ship.transform.right;
		Quaternion shipRotation = ship.transform.rotation;
		Vector3 RocketRot = shipPos + (-shipDirection);


		GameObject temp = (GameObject)Instantiate (mPreF,shipPos + (shipDirection * 6f),shipRotation);
        temp.GetComponent<Rigidbody>().AddForce(ship.transform.position + ship.transform.right * (rb.velocity.magnitude * 5f));
		temp.transform.LookAt(RocketRot);
		temp.transform.Rotate (-90,0,0);
	}


    private void UpdateUI()
    {
        string tempCargo = "" + stats.ShipCargo.ToString("F2") + "/" + stats.GetMaxCargoSpace();
        ui.UpdateShipStats(stats.Units, stats.ShipFuel, tempCargo, stats.ShipHealth);
    }
    private void UpdateBoundary()
    {
        if (Mathf.Abs(ship.transform.position.x) > SBOUND || Mathf.Abs(ship.transform.position.z) > SBOUND)
        {
            ui.BoundaryWarning = true;//enable warning text
            boundaryx.GetComponent<BoundaryLine>().drawstate = true;
            boundaryz.GetComponent<BoundaryLine>().drawstate = true;
        }
        else
        {
            ui.BoundaryWarning = false; ;//hide warning text
            boundaryx.GetComponent<BoundaryLine>().drawstate = false;
            boundaryz.GetComponent<BoundaryLine>().drawstate = false;
        }

        if (Mathf.Abs(ship.transform.position.x) > HBOUND || Mathf.Abs(ship.transform.position.z) > HBOUND)
        {
            stats.takeDamage(10.0f * Time.deltaTime); //every sec ship takes 5% damage
        }
    }
    private void ToggleStationPanel()
    {
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

            // auto dock positioning
            Quaternion temp = Quaternion.LookRotation(-station.transform.forward);
            ship.transform.rotation = Quaternion.RotateTowards(ship.transform.rotation, temp, 10f * Time.deltaTime);
            ship.transform.position = Vector3.MoveTowards(ship.transform.position,station.transform.position,0.5f*Time.deltaTime);

        }
        else
        {
            ui.UpdateStationPanelToggle(false);
        }
    }
    
    
    // EVENT HANDLERS-------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision c)
    {
        stats.takeDamage(c.relativeVelocity.magnitude / 2);
        ShieldSphereOpacity();
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
		int tempCost = (int)((100 - stats.ShipHealth) * tempPrice);

		if (tempCost <= stats.Units) 
		{
            stats.Units = -tempCost;
            stats.ShipHealth = 100f; // set ship damage value to -ship damage value. by doing this the value will be 0
		} 
		else 
		{
            stats.ShipHealth = (stats.Units / tempPrice);
		}
	}

}
