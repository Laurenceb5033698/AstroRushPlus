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

        if (controls.rocket && stats.LoadMissile())
        {
            Instantiate(mPreF, ship.transform.position + ship.transform.right * 6f, Quaternion.LookRotation(ship.transform.right, Vector3.up));
            stats.DecreaseMissileAmount();
        }
    }

    private void MoveShip()
    {
        float forward = 0.0f;
        //if (controls.zAxis > 0.1f)
            forward = controls.zAxis * stats.GetMainThrust();

        rb.velocity = transform.TransformDirection(new Vector3(forward, 0, 0));
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, controls.xAxis * stats.GetRotSpeed() * Time.deltaTime, rb.angularVelocity.z);
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
