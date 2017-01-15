using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour
{
    [SerializeField] private GameObject ship;  // ship gameobject
    [SerializeField] private GameObject mPreF; // missile prefab
    [SerializeField] private Inputs controls;
    [SerializeField] private Rigidbody rb; 	// ship's rigid body
    [SerializeField] private ShipStats stats;
    [SerializeField] private Shield shield;

    private float rotFix = 0f;

    // Mains --------------------------------------------------------------------------------------------------------
    void Start() // Use this for initialization
    {

    }
    void Update() // Update is called once per frame
    {
        controls.UpdateInputs();
        stats.UpdateStats();
        shield.ShieldSphereOpacity();
        CheckInputs();

        if (stats.IsShipWorking())
        {
            MoveShip();
        }
    }

    // FUNCTIONS --------------------------------------------------------------------------------------------------------	
    private void CheckInputs()
    {
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
        //CorrectShipTransforms();

        float forward = 0.0f;
        //if (controls.zAxis > 0.1f)
            forward = controls.zAxis * stats.GetMainThrust();

        rb.velocity = transform.TransformDirection(new Vector3(forward, 0, 0));
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, controls.xAxis * stats.GetRotSpeed() * Time.deltaTime, rb.angularVelocity.z);
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
    private void SpawnMissile()
    {
        Vector3 shipPos = ship.transform.position;
        Vector3 shipDirection = ship.transform.right;
        Quaternion shipRotation = ship.transform.rotation;
        Vector3 RocketRot = shipPos + (-shipDirection);


        GameObject temp = (GameObject)Instantiate(mPreF, shipPos + (shipDirection * 6f), shipRotation);
        temp.GetComponent<Rigidbody>().AddForce(ship.transform.position + ship.transform.right * (rb.velocity.magnitude * 5f));
        temp.transform.LookAt(RocketRot);
        temp.transform.Rotate(-90, 0, 0);
    }

    // EVENT HANDLERS-------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision c)
    {
        stats.takeDamage(c.relativeVelocity.magnitude / 2);
    }

    public void TakeDamage(float amount)
    {
        stats.takeDamage(amount);
    }

}
