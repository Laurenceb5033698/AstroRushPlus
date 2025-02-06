using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStealthShip : PlayerController {

    private Animation mAnimationComp; // reference to shield animation component
    [SerializeField] private Material mStealthedShipMat;
    private Material mRegularShipMat;
    [SerializeField] private GameObject mShipModel;
    [SerializeField] private GameObject mEMPAttack;
    [SerializeField] private ParticleSystem ParticleStealthEffect;
    [SerializeField] private ParticleSystem ParticleStealthTransitEffect;

    new void Awake()
    {
        //Call Base class Awake.
        base.Awake();
        //SubClass implementation:
        Debug.Log("Player StealthShip");
        mAnimationComp = GetComponentInChildren<Animation>();
        mRegularShipMat = mShipModel.GetComponent<Renderer>().material;
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
        ParticleStealthEffect.Play();
        ParticleStealthTransitEffect.Play();
        mAnimationComp.Play("StealthedShieldEffectAnim");
        mShipModel.GetComponent<Renderer>().material = mStealthedShipMat;
        //no collisions?
    }

    void Stealthed()
    {   //do every update

        SpendShipFuel();

    }

    void ExitStealth()
    {
        //gameObject.tag = "PlayerShip";
        GetComponentInChildren<MeshCollider>().gameObject.tag = "PlayerShip";
        //visual effect ends
        ParticleStealthTransitEffect.Stop();
        mAnimationComp.Play("ShieldFlashingAnim");
        mShipModel.GetComponent<Renderer>().material = mRegularShipMat;
        //do aoe emp
        GameObject emp = Instantiate<GameObject>(mEMPAttack, transform.position, transform.rotation);
        emp.GetComponent<AoeEffect>().SetupValues(0, tag);
    }

    override protected void MoveShip()
    {
        float currentSpeed = 0.0f;
        currentSpeed = stats.GetMainThrust();


        if (controls.LeftAnalogueInUse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(controls.LeftStick.x, 0, controls.LeftStick.y)) * Quaternion.Euler(new Vector3(0, -90, 0)), 10);

            
        }

        rb.linearVelocity = new Vector3(controls.LeftStick.x * currentSpeed, 0, controls.LeftStick.y * currentSpeed);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    public override bool AlternateShipVFX()
    {
        return false;
    }
}
