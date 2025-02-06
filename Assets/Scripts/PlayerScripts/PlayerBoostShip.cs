using UnityEngine;
using System.Collections;

using XInputDotNetPure;
using Unity.Services.Analytics; // for controller rumble

public class PlayerBoostShip : PlayerController {
    
    new void Awake() {
        //Call Base class Awake.
        base.Awake();
        //SubClass implementation:
        Debug.Log("Player BoostShip");

    }

    new protected void Start () {
        base.Start();
        //SubClass implementation:

    }

    new void Update () {
        base.Update();
        

    }
    
    override protected void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();


        if (controls.LeftAnalogueInUse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

            if (UsingAbility && stats.ShipFuel > 0.01f)
            {
                currentSpeed = stats.GetSpecial();
                SpendShipFuel();
            }
        }

        rb.linearVelocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    //Event Handlers
    override public bool AlternateShipVFX()
    {
        //play boost trail vfx on thruster
        return UsingAbility;

    }
}