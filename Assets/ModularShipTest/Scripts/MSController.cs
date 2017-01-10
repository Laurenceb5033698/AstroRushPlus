using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MSController : MonoBehaviour
{
    public GameObject ship;
    public List<GameObject> thrusters = new List<GameObject>();
    public List<GameObject> cargos = new List<GameObject>();
    public List<GameObject> batteries = new List<GameObject>();
    public List<GameObject> fuelTanks = new List<GameObject>();
    public List<GameObject> SolarPanels = new List<GameObject>();
    public GameObject Laser;

    public Rigidbody rb;
    public Inputs controls;
    public ShipStats stats;

    public float overallFuel = 0f;
    public float overallCargo = 0f;
    public float overallPower = 0f;

    // Use this for initialization
    void Start ()
    {
        InitaliseThrusters();
    }

    void Update() // Update is called once per frame
    {
        CheckInputs();
        UpdateOveralValues();
        MoveShip();
        AnimateThrust();
    }

    // FUNCTIONS --------------------------------------------------------------------------------------------------------	
    private void CheckInputs()
    {
        if (controls.reset)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ship.transform.position = Vector3.zero;
            stats.ResetShip();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3)) // Y button on Xbox Controller
            ToggleSolarPanels();


        stats.LaserState = controls.RLaser;
    }
    private void MoveShip()
    {
        // Reset unwanted xyz rotation and velocity --------------------------------------------------------------------------------------------
        rb.velocity = new Vector3(rb.velocity.x, 0.00f, rb.velocity.z);
        rb.angularVelocity = new Vector3(0.00f, rb.angularVelocity.y, 0.00f);
        ship.transform.position = new Vector3(ship.transform.position.x, 0.00f, ship.transform.position.z);
        ship.transform.eulerAngles = new Vector3(0f, ship.transform.eulerAngles.y, 0f); // fix the weird rotation applied to x and z axis
        //--------------------------------------------------------------------------------------------------------------------------------------

        if (controls.boost)
        {
            rb.AddForce(ship.transform.forward * -stats.GetBoostSpeed());
        }

        rb.AddForce(ship.transform.forward * (controls.zAxis * -stats.GetMainThrust()) * Time.deltaTime);

        rb.AddForce(ship.transform.right * (controls.xAxis * -stats.GetMainThrust()) * Time.deltaTime);

        rb.AddTorque(ship.transform.up * (controls.yawAxis * stats.GetRotSpeed()) * Time.deltaTime);
    }

    private void ToggleSolarPanels()
    {
        foreach (GameObject go in SolarPanels)
        {
            go.GetComponent<SolarPanel>().ToggleState();
        }
    }

    private void UpdateOveralValues()
    {
        float tempCargo = 0f;
        float tempFuel = 0f;
        float tempCharge = 0f;

        foreach (GameObject go in cargos)
        {
            if (go.GetComponent<Cargo>() != null)
                tempCargo += go.GetComponent<Cargo>().GetCargoAmount();
            if (go.GetComponent<SmallCargo>() != null)
                tempCargo += go.GetComponent<SmallCargo>().GetCargoAmount();
        }
        foreach (GameObject go in fuelTanks)
        {
            if (go.GetComponent<FuelTank>() != null)
                tempFuel += go.GetComponent<FuelTank>().GetFuelAmount();
            if (go.GetComponent<SmallFuelTank>() != null)
                tempFuel += go.GetComponent<SmallFuelTank>().GetFuelAmount();
        }
        foreach (GameObject go in batteries)
        {
            tempCharge += go.GetComponent<Battery>().GetChargeAmount();
        }

        overallCargo = tempCargo;
        overallFuel = tempFuel;
        overallPower = tempCharge;
    }

    // BUGG IN THE FUNCTION!!!! (infinite loop problem)
    private void RemoveFuel(float amount)
    {
        float fuelToRemove = Mathf.Abs(amount);
        float fuelLeftInTanks = 0f;

        int safetyBrakeoutCounter = 0;

        foreach (GameObject go in fuelTanks)
        {
            if (go.GetComponent<FuelTank>() != null)
                fuelLeftInTanks += go.GetComponent<FuelTank>().GetFuelAmount();
            if (go.GetComponent<SmallFuelTank>() != null)
                fuelLeftInTanks += go.GetComponent<SmallFuelTank>().GetFuelAmount();
        }

        if (fuelToRemove >= fuelLeftInTanks)
            fuelToRemove = fuelLeftInTanks;


        int notEmptyTanks;

        do
        {
            notEmptyTanks = 0;

            notEmptyTanks = countNotEmptyFuelTanks();


            foreach (GameObject go in fuelTanks)
            {
                float temp = 0f;

                if (go.GetComponent<FuelTank>() != null && notEmptyTanks > 0)
                {
                    temp = go.GetComponent<FuelTank>().GetFuelAmount();

                    if (temp < fuelToRemove / notEmptyTanks)
                    {
                        go.GetComponent<FuelTank>().RemoveFuel(temp);
                        fuelToRemove -= temp;
                    }
                    else
                    {
                        go.GetComponent<FuelTank>().RemoveFuel(fuelToRemove / notEmptyTanks);
                        fuelToRemove -= (fuelToRemove / notEmptyTanks);
                    }
                }

                if (go.GetComponent<SmallFuelTank>() != null && notEmptyTanks > 0)
                {
                    temp = go.GetComponent<SmallFuelTank>().GetFuelAmount();

                    if (temp < fuelToRemove / notEmptyTanks)
                    {
                        go.GetComponent<SmallFuelTank>().RemoveFuel(temp);
                        fuelToRemove -= temp;
                    }
                    else
                    {
                        go.GetComponent<SmallFuelTank>().RemoveFuel(fuelToRemove / notEmptyTanks);
                        fuelToRemove -= (fuelToRemove / notEmptyTanks);
                    }
                }
            }
            safetyBrakeoutCounter++;
            Debug.Log(fuelToRemove);
        }
        while (fuelToRemove > 0 && safetyBrakeoutCounter > 100);
    }

    private int countNotEmptyFuelTanks()
    {
        int temp = 0;

        foreach (GameObject go in fuelTanks)
        {
            if (go.GetComponent<FuelTank>() != null)
                if (go.GetComponent<FuelTank>().GetFuelAmount() > 0)
                    temp++;

            if (go.GetComponent<SmallFuelTank>() != null)
                if (go.GetComponent<SmallFuelTank>().GetFuelAmount() > 0)
                    temp++;
        }

        return temp;
    }



    // THRUSTER ANIMATION -------------------------------------------------------------------------------------------
    private void InitaliseThrusters()
    {
        foreach (GameObject go in thrusters)
        {
            go.GetComponent<Engine>().ToggleThruster(false);
        }
    }
    private void AnimateThrust()
    {
        // ----------------- Forward Thrusters -------------------------------
        bool fThrusters = (controls.zAxis > 0);
        thrusters[4].GetComponent<Engine>().ToggleThruster(fThrusters);
        thrusters[5].GetComponent<Engine>().ToggleThruster(fThrusters);
        thrusters[6].GetComponent<Engine>().ToggleThruster(fThrusters);

        // ----------------- Backward Thrusters -------------------------------
        thrusters[2].GetComponent<Engine>().ToggleThruster(controls.zAxis < 0);
        thrusters[3].GetComponent<Engine>().ToggleThruster(controls.zAxis < 0);

        // ----------------- Left & Right Thrusters -----------------------------------
        thrusters[0].GetComponent<Engine>().ToggleThruster(controls.yawAxis > 0 || controls.xAxis > 0);
        thrusters[7].GetComponent<Engine>().ToggleThruster(controls.yawAxis < 0 || controls.xAxis > 0);
        thrusters[1].GetComponent<Engine>().ToggleThruster(controls.yawAxis < 0 || controls.xAxis < 0);
        thrusters[8].GetComponent<Engine>().ToggleThruster(controls.yawAxis > 0 || controls.xAxis < 0);
    }

}
