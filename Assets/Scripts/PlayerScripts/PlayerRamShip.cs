using UnityEngine;
using System.Collections;

using XInputDotNetPure; // for controller rumble

public class PlayerRamShip : PlayerController
{
        
    //private bool UsingAbility = false;
    private bool Ramming = false;
    private float RamCharging = 0.0f;
    [SerializeField]
    private float RamChargeMax = 1.0f;
    private Vector3 chargeDir;


    new void Awake()
    {
        //Call Base class Awake.
        base.Awake();
        //SubClass implementation:
        Debug.Log("Player RamShip");
        chargeDir = Vector3.zero;
    }

    new void Start()
    {
        base.Start();
        //SubClass implementation:

    }

    new void Update()
    {
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
        ////check DpadY for input
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
        ////Equipment########
        //if (!usedEquipment && (Input.GetAxis("RightTrigger") > 0.1f || Input.GetKeyDown(KeyCode.Space)) && equipment.HasAmmo()) // right bumper
        //{
        //    usedEquipment = true;
        //    //Instantiate(mPreF, ship.transform.position + direction * 8f, Quaternion.LookRotation(direction, Vector3.up));
        //    equipment.UseOrdinance(direction);
        //    //stats.DecreaseMissileAmount();
        //}
        //if (Input.GetAxis("RightTrigger") < 0.1f)
        //    usedEquipment = false;
        //Ability##########
        //if (controls.ability && stats.ShipFuel > 0.1f)
        //{
        //    if (UsingAbility == false)//set RamCharging jsut once.
        //        RamCharging = Time.time + RamChargeMax;
        //    UsingAbility = true;
        //} else {
        //    UsingAbility = false;
        //    Ramming = false;
        //    chargeDir = Vector3.zero;
        //}
        
        //Movement#########
        //if (stats.IsAlive()) {
        //    if (UsingAbility)
        //    {   //if usingAbility: ramming move
        //        RamMove();
        //    }
        //    else
        //    {   //else normal move
        //        MoveShip();
        //    }

        //}
        //else { rumbleTimer = 0; GamePad.SetVibration(playerIndex, 0, 0); }

    }

    protected override void InputLeftAnalog()
    {
        if ( stats.IsAlive() && controls.LeftAnalogueInUse )
        {
            if (UsingAbility)
            {   //if usingAbility: ramming move
                RamMove();
            }
            else
            {   //else normal move
                MoveShip();
            }

        }
        else { rumbleTimer = 0; GamePad.SetVibration(playerIndex, 0, 0); }
    }

    protected override void InputLeftTrigger()
    {
        if (controls.LTriggerInUse && stats.ShipFuel > 0.1f)
        {
            if (UsingAbility == false)//set RamCharging jsut once.
                RamCharging = Time.time + RamChargeMax;
            UsingAbility = true;
        }
        else
        {
            UsingAbility = false;
            Ramming = false;
            chargeDir = Vector3.zero;
        }
    }


    //Only called while UsingAbility is true
    private void RamMove()
    {
        //direction locked at start of ram
        //short delay before Ram kicks in
        //can be stopped early by releasing button once in-motion
        float currentSpeed = 0.0f;
        if (!Ramming)
        {
            chargeDir = new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y);
            if (RamCharging <= Time.time)
            {
                Ramming = true;
                //hack some shield hp back onto shield...
                stats.ShipShield = 30;
            }
            //only rotate while aiming the ram
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(chargeDir) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);
        }
        else
        {
            currentSpeed = stats.GetSpecial();
            chargeDir.Normalize();
            rb.linearVelocity = new Vector3(chargeDir.x * currentSpeed, 0, chargeDir.z * currentSpeed);
            SpendShipFuel();
        }

        rb.angularVelocity = new Vector3(0, 0, 0);

    }

    override protected void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();


        //if (Mathf.Abs(controls.LeftStick.x) > 0.01f || Mathf.Abs(controls.LeftStick.y) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);


        }

        rb.linearVelocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    //events
    override protected void OnCollisionEnter(Collision c)
    {
        //no collision damage for player Ramship
        if (c.gameObject.tag == "EnemyShip")
        {
            if (Ramming)
                c.gameObject.GetComponent<AICore>().TakeDamage(gameObject.GetComponent<EventSource>(), transform.position, 200);
            else
                c.gameObject.GetComponent<AICore>().TakeDamage(gameObject.GetComponent<EventSource>(), transform.position, 50);
        }
    }

    override public void TakeDamage(Vector3 otherpos, float amount)
    {
        //if (stats.ShipShield > 0)
        //    Shield_effect(otherpos);
        ////reduce incoming damage to 33%
        if (Ramming) amount /= 3;
        //stats.TakeDamage(amount);
        //rumbleTimer = Time.time + 0.3f;

        base.TakeDamage(otherpos, amount);
    }
    public override bool AlternateShipVFX()
    {
        return false;
    }
}
