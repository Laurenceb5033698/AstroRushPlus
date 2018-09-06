using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStealthShip : PlayerController {
    new void Awake()
    {
        //Call Base class Awake.
        base.Awake();
        //SubClass implementation:
        Debug.Log("Player StealthShip");

    }

    new protected void Start()
    {
        base.Start();
        //SubClass implementation:

    }

    new void Update()
    {
        base.Update();
        //SubClass Implementation:
        

    }

    protected override void InputLeftTrigger()
    {
        if (controls.LTriggerInUse && stats.ShipFuel > 0.1f)
        {
            if (UsingAbility == false)
            {   //do once
                EnterStealth();
            }
            UsingAbility = true;

            Stealthed();
        }
        else
        {
            if (UsingAbility == true)
            {   //do once
                ExitStealth();
            }
            UsingAbility = false;

        }
    }

    void EnterStealth()
    {
        //gameObject.tag = "PlayerStealth";
        GetComponentInChildren<MeshCollider>().gameObject.tag = "PlayerStealth";
        //visual effect
        //no collisions?
    }

    void Stealthed()
    {   //do every update

        stats.ShipFuel = -25 * Time.deltaTime;

    }

    void ExitStealth()
    {
        //gameObject.tag = "PlayerShip";
        GetComponentInChildren<MeshCollider>().gameObject.tag = "PlayerShip";

        //do aoe emp
        //visual effect ends
    }

    override protected void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();


        if (controls.LeftAnalogueInUse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

            
        }

        rb.velocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }
}
