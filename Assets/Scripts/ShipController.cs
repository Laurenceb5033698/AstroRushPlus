using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShipController : MonoBehaviour
{
    private GameObject ship;  // ship gameobject
    private Inputs controls;
    [SerializeField] private GameObject mPreF; // missile prefab
    [SerializeField] private GameObject turret; // missile prefab
    [SerializeField] private Weapon gun;
    private int weaponType = 0;

    private Rigidbody rb; 	// ship's rigid body
    private ShipStats stats;
    //private Shield shield;

    // Mains --------------------------------------------------------------------------------------------------------
    void Start() // Use this for initialization
    {
        ship = transform.gameObject;
        controls = ship.GetComponent<Inputs>();
        rb = ship.GetComponent<Rigidbody>();
        stats = ship.GetComponent<ShipStats>();
        //shield = ship.GetComponentInChildren<Shield>();
        gun = turret.GetComponent<Weapon>();
    }

    private Vector3 dir;
    void Update() // Update is called once per frame
    {

        if (controls.shield)
        {
            stats.ActivateShieldPU();
        }

        if (Mathf.Abs(controls.RightStick.x) > 0.1f || Mathf.Abs(controls.RightStick.y) > 0.1f)//shooting
        {
            dir = new Vector3(controls.RightStick.x, 0, controls.RightStick.y).normalized;

            if (controls.rocket && stats.LoadMissile())
            {
                weaponType = 2;
                Instantiate(mPreF, ship.transform.position + dir * 8f, Quaternion.LookRotation(dir, Vector3.up));
                stats.DecreaseMissileAmount();
            }
            if (controls.trishot)
            {
                weaponType = 1;
                gun.changeType("tri");
                gun.Shoot(dir);
            }
            else
            {
                weaponType = 0;
                gun.changeType("pew");
                gun.Shoot(dir);
            }
        }
        else
        {
            weaponType = 0;
        }

        if (stats.IsAlive())
        {
            MoveShip();
        }
    }

    // FUNCTIONS --------------------------------------------------------------------------------------------------------	

    private void MoveShip()
    {
        float currentSpeed = 0.0f;

        if (controls.boost && !stats.bco)
        {
            if (stats.ShipFuel < 0.2f)
            {
                stats.bco = true;
            }
            currentSpeed = stats.GetBoostSpeed();
            stats.ShipFuel = -25 * Time.deltaTime;
        }
        else
        {
            if (stats.ShipFuel > 20f)
            {
                stats.bco = false;
            }
            currentSpeed = stats.GetMainThrust();
        }


        //rb.velocity = transform.TransformDirection(new Vector3(controls.zAxis * stats.GetMainThrust(), 0, -controls.xAxis * stats.GetMainThrust())) * boostMultiplier;
        rb.velocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0,0,0);

        //if (Mathf.Abs(controls.yawAxis) > 0.1f || Mathf.Abs(controls.rightY) > 0.1f)
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(-controls.rightY, 0, controls.yawAxis)), 10);
        if (Mathf.Abs(controls.LeftStick.x) > 0.1f || Mathf.Abs(controls.LeftStick.y) > 0.1f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);
    }

    // EVENT HANDLERS-------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision c)
    {
        TakeDamage(c.relativeVelocity.magnitude / 4);
    }

    public void TakeDamage(float amount)
    {
        stats.TakeDamage(amount);
    }
    public int GetWeaponType()
    {
        return weaponType;
    }
}
