using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStealth : AICore {

    private bool usingAbility;
    [SerializeField] private MeshRenderer m_MeshRendr;
    [SerializeField] private Collider m_Collider;
    Material[] RegularMats = new Material[3];
    Material[] StealthedMats = new Material[3];
    [SerializeField] private ParticleSystem ParticleStealthEffect;
    [SerializeField] private ParticleSystem ParticleStealthTransitEffect;

    [SerializeField] private Material mStealthedShipMat;    //stealth texture (transparent)

    [SerializeField] private GameObject mShipModel;
    new void Start()
    {
        base.Start();
        RegularMats = m_MeshRendr.materials;
        StealthedMats[0] = mStealthedShipMat;
        StealthedMats[1] = mStealthedShipMat;
        StealthedMats[2] = mStealthedShipMat;

    }
    override protected void DistToDestination()
    {
        dist = Vector3.Distance(destination, gameObject.transform.position);
        controlDir = (destination - gameObject.transform.position).normalized;
        AttackDir = controlDir;
        //if dest is too close, fly away
        torqueMul = 1f;

        if (dist < KeepAwayRange)
        {
            //Enable phase ability
            ActivatePhase();
            torqueMul = torqueMultiplier;
        }
        else
        {
            //disable phase ability
            DeactivatePhase();
        }
    }

    void ActivatePhase()
    {
        if (usingAbility == false)//happens first time
        {   //disable collider
            m_Collider.enabled = false;
            //play enter phase animation effect
            m_MeshRendr.materials = StealthedMats;
            //play particle effect once
            ParticleStealthEffect.Play();
            ParticleStealthTransitEffect.Play();
        }
        usingAbility = true;
    }

    void DeactivatePhase()
    {
        if (usingAbility == true)
        {   //enable collider
            m_Collider.enabled = true;
            //play exit phase animation effect
            m_MeshRendr.materials = RegularMats;
            ParticleStealthTransitEffect.Stop();
        }
        usingAbility = false;

    }
}
