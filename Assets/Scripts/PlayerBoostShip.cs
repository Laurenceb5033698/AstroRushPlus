using UnityEngine;
using System.Collections;

using XInputDotNetPure; // for controller rumble

public class PlayerBoostShip : PlayerController {


    [SerializeField] private GameObject mPreF; // missile prefab

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
        //SubClass Implementation:

        //aiming = (Mathf.Abs(controls.RightStick.x) > 0.1f || Mathf.Abs(controls.RightStick.y) > 0.1f);

        //if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetMouseButtonDown(1)) // left bumper
        //{
        //    weaponType = arsenal.ChangeGun(-1);

        //}
        //if (Input.GetKeyDown(KeyCode.JoystickButton5))
        //{
        //    weaponType = arsenal.ChangeGun(1);
        //}
        //check DpadY for input
        //if (controls.DpadYPressed)
        //{
        //    if (controls.DpadUp || Input.GetKeyDown(KeyCode.Q)) // DPad up
        //    {
        //        equipment.ChangeGun(1);
        //    }
        //    if (controls.DpadDown) // DPad down
        //    {//perhaps else for optimization
        //        equipment.ChangeGun(-1);
        //    }
        //}

        //Vector3 direction = Vector3.zero;

        //if (aiming)
        //{
        //    direction = new Vector3(controls.RightStick.x, 0, controls.RightStick.y).normalized;
        //    arsenal.FireWeapon(direction);
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    Plane playerPlane = new Plane(Vector3.up, transform.position);
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    float hitdist = 0.0f;
        //    if (playerPlane.Raycast(ray, out hitdist))
        //    {
        //        Vector3 targetPoint = ray.GetPoint(hitdist);
        //        direction = (targetPoint - transform.position).normalized;


        //        arsenal.FireWeapon(direction);
        //        //Debug.Log(direction);
        //    }


        //}
        //else direction = transform.right;

        //if (!usedEquipment && (Input.GetAxis("RightTrigger") > 0.1f || Input.GetKeyDown(KeyCode.Space)) && equipment.HasAmmo()) // right bumper
        //{
        //    usedEquipment = true;
        //    equipment.UseOrdinance(direction);
        //    //stats.DecreaseMissileAmount();
        //}
        //if (Input.GetAxis("RightTrigger") < 0.1f)
        //    usedEquipment = false;

        //if (stats.IsAlive()) MoveShip();
        //else { rumbleTimer = 0; GamePad.SetVibration(playerIndex, 0, 0); }

    }
    
    override protected void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();


        if (controls.LeftAnalogueInUse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

            if (UsingAbility && stats.ShipFuel > 0.1f)
            {
                currentSpeed = stats.GetBoostSpeed();
                stats.ShipFuel = -25 * Time.deltaTime;
            }
        }

        rb.velocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }
}
