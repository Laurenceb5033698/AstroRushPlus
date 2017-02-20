using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using XInputDotNetPure; // for controller rumble

public class ShipController : MonoBehaviour
{
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

        // Detect if a button was pressed this frame
        //if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)


    private GameObject ship;  // ship gameobject
    private Inputs controls;
    [SerializeField] private GameObject mPreF; // missile prefab
    [SerializeField] private GameObject turret; // missile prefab
    [SerializeField] private Weapon gun;
    private int weaponType = 0;
    private bool aiming = false;
    private float rumbleTimer = 0;

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

    void Update() // Update is called once per frame
    {
        UpdateController();

        if (controls.shield) stats.ActivateShieldPU();

        aiming = (Mathf.Abs(controls.RightStick.x) > 0.1f || Mathf.Abs(controls.RightStick.y) > 0.1f);

        if (aiming) UpdateWeapons(new Vector3(controls.RightStick.x, 0, controls.RightStick.y).normalized);
        else weaponType = 0;

        if (stats.IsAlive()) MoveShip();
        else
        {
            rumbleTimer = 0;
            GamePad.SetVibration(playerIndex, 0, 0);
        }
    }

    void FixedUpdate()
    {
        //GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
        if (rumbleTimer > Time.time)
            GamePad.SetVibration(playerIndex, 1, 1);
        else
            GamePad.SetVibration(playerIndex, 0, 0);
    }


    // FUNCTIONS --------------------------------------------------------------------------------------------------------	
    private void UpdateController()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }
    private void UpdateWeapons(Vector3 direction)
    {
        if (controls.rocket && stats.LoadMissile())
        {
            weaponType = 2;
            Instantiate(mPreF, ship.transform.position + direction * 8f, Quaternion.LookRotation(direction, Vector3.up));
            stats.DecreaseMissileAmount();
        }

        if (controls.trishot)
        {
            weaponType = 1;
            gun.changeType("tri");
        }
        else
        {
            weaponType = 0;
            gun.changeType("pew");
        }

        gun.Shoot(direction);
    }

    private void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();


        if (Mathf.Abs(controls.LeftStick.x) > 0.1f || Mathf.Abs(controls.LeftStick.y) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

            if (controls.boost && stats.ShipFuel > 0.1f)
            {
                currentSpeed = stats.GetBoostSpeed();
                stats.ShipFuel = -25 * Time.deltaTime;
            }
        }

        rb.velocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    // EVENT HANDLERS-------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision c)
    {
        TakeDamage(c.relativeVelocity.magnitude / 4);
        if (c.gameObject.tag == "EnemyShip")
        {
            c.gameObject.GetComponent<NewBasicAI>().TakeDamage(50);
        }
    }

    public void TakeDamage(float amount)
    {
        stats.TakeDamage(amount);
        rumbleTimer = Time.time + 0.3f;
    }
    public int GetWeaponType()
    {
        return weaponType;
    }

    void OnApplicationQuit()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }
}
