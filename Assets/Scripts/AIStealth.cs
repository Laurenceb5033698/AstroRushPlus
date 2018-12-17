using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStealth : AICore {

    private bool usingAbility;
    [SerializeField] private MeshRenderer m_MeshRendr;
    [SerializeField] private Collider m_Collider;
    Material[] RegularMats = new Material[3];
    [SerializeField] private Material mRegularShipMatDark;
    [SerializeField] private Material mRegularShipMatLight;
    Material[] StealthedMats = new Material[3];

    [SerializeField] private Material mSShipMatDark;
    [SerializeField] private Material mSShipMatLight;

    [SerializeField] private GameObject mShipModel;
    private void Start()
    {
        RegularMats = m_MeshRendr.materials;
        StealthedMats[0] = mSShipMatDark;
        StealthedMats[1] = mSShipMatLight;
        StealthedMats[2] = mSShipMatLight;

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

            //mShipModel.GetComponent<Renderer>().materials = RegularMats

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

        }
        usingAbility = false;

    }
}
