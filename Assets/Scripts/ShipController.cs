using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private GameObject ship;  // ship gameobject
    [SerializeField]
    private GameObject mPreF; // missile prefab
    [SerializeField]
    private Inputs controls;
    [SerializeField]
    private AnimateThrusters thrusters;
    [SerializeField]
    private Rigidbody rb; 	// ship's rigid body
    [SerializeField]
    private Laser shipLaser;
    [SerializeField]
    private ShipStats stats;
    [SerializeField]
    private UI ui;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject boundaryz;
    [SerializeField]
    private GameObject boundaryx;
    [SerializeField]
    private GameObject shieldSphere = null;

    private const int SBOUND = 600;
    private const int HBOUND = SBOUND + 70;
    private float rotFix = 0f;

    private bool left = false;
    private bool right = false;
    private bool quickstop;

    // Mains --------------------------------------------------------------------------------------------------------
    void Start() // Use this for initialization
    {

    }
    void Update() // Update is called once per frame
    {
        controls.UpdateInputs();
        thrusters.UpdateThrusters();
        stats.regenerateShield();
        ShieldSphereOpacity();

        CheckInputs();

        if (stats.IsShipWorking())
        {
            thrusters.SetThrusterState(true);
            MoveShip2();
        }
        else
        {
            thrusters.SetThrusterState(false);
            ui.setMessage(0);
            ui.menu = true;
        }

        UpdateUI();
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

        stats.LaserState = controls.RLaser;

        if (controls.rocket)
        {
            if (stats.LoadMissile())
            {
                SpawnMissile();
                stats.DecreaseMissileAmount();
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
        //rb.AddForce(ship.transform.right * (controls.zAxis * stats.GetMainThrust()) * Time.deltaTime);

        // left and right
        if (controls.xAxis > 0.1f)
        {
            //rb.AddForce(ship.transform.forward * (Mathf.Abs(controls.xAxis) * -stats.GetMainThrust()) * Time.deltaTime);
        }


        if (controls.xAxis < -0.1f)
        {
            //rb.AddForce(ship.transform.forward * (-Mathf.Abs(controls.xAxis) * -stats.GetMainThrust()) * Time.deltaTime);
        }


        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        locVel = new Vector3(controls.zAxis * stats.GetMainThrust(), 0, -controls.xAxis * stats.GetMainThrust());
        rb.velocity = transform.TransformDirection(locVel);

        //rb.velocity = new Vector3((controls.xAxis * stats.GetMainThrust()),0,(controls.zAxis * stats.GetMainThrust()));


        if (!left && !right) dampenSidewaysMotion();
        //rotate
        //rb.AddTorque(Vector3.up * (controls.yawAxis * stats.GetRotSpeed()) * Time.deltaTime);

        rb.angularVelocity = new Vector3(rb.angularVelocity.x, controls.yawAxis * stats.GetRotSpeed() * Time.deltaTime, rb.angularVelocity.z);
    }
    private void MoveShip2()
    {
        CorrectShipTransforms();


        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        float temp = Mathf.Sqrt(controls.yawAxis * controls.yawAxis + controls.rightY * controls.rightY);
        bool rightstick = (temp > 0.7f);
        if (!(controls.boost && rightstick) && (controls.boost || rightstick))//controls.boost || controls.yawAxis != 0 || controls.rightY != 0)// && !stats.bco && stats.GetBoostFuelAmount() > 0.0f
        {//hypermove with strafe
            //if (stats.GetBoostFuelAmount() < 0.5f)
            //{
            //    stats.bco = true;
            //}
            locVel = new Vector3(controls.zAxis * stats.GetMainThrust(), 0, -controls.xAxis * stats.GetMainThrust());
            rb.velocity = transform.TransformDirection(locVel);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
            //stats.ShipFuel = -20 * Time.deltaTime;
            
        }
        else
        {//normal move with turning
            

            locVel = new Vector3(controls.zAxis * stats.GetMainThrust(), 0, 0);
            rb.velocity = transform.TransformDirection(locVel);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, controls.xAxis * stats.GetRotSpeed() * Time.deltaTime, rb.angularVelocity.z);
            //if (stats.GetBoostFuelAmount() >= 20f)
            //{
            //    stats.bco = false;
            //}
            //stats.ShipFuel = 12 * Time.deltaTime;
        }
    }
    private void dampenSidewaysMotion()
    {
        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        //Debug.Log(locVel.z);

        locVel.z += -(locVel.z / 100 * 80) * Time.deltaTime;
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

    private void SpawnMissile()
    {
        Vector3 shipPos = ship.transform.position;
        Vector3 shipDirection = ship.transform.right;
        Quaternion shipRotation = ship.transform.rotation;
        Vector3 RocketRot = shipPos + (-shipDirection);

        //Debug.Log(rb.velocity.magnitude);
        GameObject temp = (GameObject)Instantiate(mPreF, shipPos + (shipDirection * 6f), shipRotation);
        temp.GetComponent<Rigidbody>().velocity = (ship.transform.right * (rb.velocity.magnitude));
        temp.GetComponent<Rigidbody>().AddForce(RocketRot.normalized * (rb.velocity.magnitude * 6f));//
        temp.transform.LookAt(RocketRot);
        temp.transform.Rotate(-90, 0, 0);
    }


    private void UpdateUI()
    {
        ui.UpdateShipStats(stats.ShipFuel, stats.ShipHealth);
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


    // EVENT HANDLERS-------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.GetComponent<Missile>() == null)
        {//if we collide with anything but a missile
            stats.takeDamage(c.relativeVelocity.magnitude / 2);
            ShieldSphereOpacity();
        }
    }

    public void TakeDamage(float amount)
    {
        stats.takeDamage(amount);
        ShieldSphereOpacity();

    }

}
