using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // for controller rumble

public class PlayerGunship : PlayerController {

    //List of weapons
    //[SerializeField] private List<Weapon> AbilityGuns; //set at start, after arsenal awake
    [SerializeField] private GameObject AbilityGunArray;    //holds all ability weapons

    [SerializeField] private float TurretChargeMax = 1.0f;
    private float turretCharging = 0.0f;
    private bool InTurretMode = false;

    new void Awake() {
        //Call Base class Awake.
        base.Awake();
        //SubClass implementation:
        Debug.Log("Player GunShip");

    }

    new protected void Start() {
        base.Start();
        //SubClass implementation:

        //fetch all weapons attached to abilityGunArray object
        //AbilityGunArray.GetComponentsInChildren<Weapon>(AbilityGuns);
        //foreach( Weapon gun in AbilityGuns)
        //{   //bit of a hack, but each gun's tag = "playerShip".
        //    //This means we can use the Weapon's own Gameobject as the spawn for projectiles (for better feels)
        //    gun.SetShipObject(gun.gameObject);
        //}
        //AbilityGunArray.SetActive(false);
    }
    new void Update() {
        base.Update();
        //SubClass Implementation:
        
        //

    }

    protected override void InputLeftTrigger()
    {   //while held
        if (controls.LTriggerInUse && stats.ShipFuel > 0.01f)
        {
            //ability charge for a second
            //then all guns enabled(not arsenal: separate gunlist)
            if (UsingAbility == false)
                turretCharging = Time.time + TurretChargeMax;
            else
                InTurretMode = (turretCharging <= Time.time);
            UsingAbility = true;
        }
        else
        {
            //end immediately when left trigger released
            //AbilityGunArray.SetActive(false);
            UsingAbility = false;
            InTurretMode = false;
        }
    }

    protected override void InputLeftAnalog()
    {
        if (stats.IsAlive() && controls.LeftAnalogueInUse)
        {
            if (UsingAbility)   //stand still to "charge" turret mode
            {   //if usingAbility: Turret mode movement
                TurretMovement();
            }
            else
            {   //else normal move
                MoveShip();
            }

        }
        else { rumbleTimer = 0; GamePad.SetVibration(playerIndex, 0, 0); }
    }

    protected override void InputRightAnalog()
    {   //shoot normally unless InTurretMode
        if (!InTurretMode)
        {   //normal
            base.InputRightAnalog();
        }
        else
        {   //all guns
            AbilityGunArray.SetActive(true);

            aiming = controls.RightAnalogueInUse;

            //reduce ability fuel.
            // This is called regardless if we're shooting or not, But is only called while fully in turret mode
            SpendShipFuel();

            if (aiming)
            {   //fire weapons
                direction = new Vector3(controls.RightStick.x, 0, controls.RightStick.y).normalized;
                TurretFire(direction);
            }
        }
    }
    protected override void InputLMB()
    {   //shoot normally unless InTurretMode
        if (!InTurretMode)
        {   //normal
            base.InputLMB();
        }
        else
        {   //all guns
            if (Input.GetMouseButton(0) && !aiming)
            {   //fire weapons

                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist))
                {
                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    direction = (targetPoint - transform.position).normalized;
                    TurretFire(direction);

                    //Debug.Log(direction);
                }
            }
        }
    }

    private void TurretFire(Vector3 aimDir)
    {   //shoots like arsenal

        //first, spin AbilityGunarray towards aimDir
        //AbilityGunArray.transform.rotation = Quaternion.RotateTowards(AbilityGunArray.transform.rotation, Quaternion.LookRotation(aimDir) * Quaternion.Euler(new Vector3(0, -90, 0)), 5);

        //fire each
        //foreach( Weapon gun in AbilityGuns)
        //  gun.Shoot(aimDir);
    }

    private void TurretMovement()//movement while in Turret Mode
    {
        if (controls.LeftAnalogueInUse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

        }
        //stationary
        rb.linearVelocity = new Vector3(0,0,0);
        rb.angularVelocity = new Vector3(0, 0, 0);

    }

    override protected void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();

        if (controls.LeftAnalogueInUse) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

        }

        rb.linearVelocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    //events
    public override bool AlternateShipVFX()
    {
        return false;
    }
}
