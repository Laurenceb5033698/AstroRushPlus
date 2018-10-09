using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStealth : AICore {

    private bool usingAbility;
    [SerializeField] private Collider m_Collider;

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

        }
        usingAbility = true;
    }

    void DeactivatePhase()
    {
        if (usingAbility == true)
        {   //enable collider
            m_Collider.enabled = true;
            //play exit phase animation effect
        }
        usingAbility = false;

    }
}
