using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Homing service is used by missile homing Component to find and store currently targeted ships
/// </summary>
[System.Serializable]
public class HomingService : IService
{

    List<Collider> TrackedObjects;
       

    public Transform FindTarget(Transform _sourceTransform)
    {
        int layerMask = LayerMask.GetMask("Default");
        IEnumerable<Collider> workingList = Physics.OverlapSphere(_sourceTransform.position, 90f, layerMask, QueryTriggerInteraction.Ignore);
        
        //create list that has removed any tracked objects.
        IEnumerable<Collider> trimmedColliders = workingList.ToList().Except(TrackedObjects);
        if(trimmedColliders.Count() > 0)
        {
            //use this list if there are untargeted objects. otherwise use regular (focuses closest only)
            workingList = trimmedColliders;
        }

        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider col in workingList)
        {
            //Debug.Log(col.gameObject.name); 
            if (col.gameObject.GetComponentInParent<AICore>() != null)
            {
                Vector3 directionToTarget = col.gameObject.transform.position - _sourceTransform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = col.gameObject;
                }
            }
        }

        if (bestTarget)
            TrackedObjects.Add(bestTarget.GetComponent<Collider>());

        return bestTarget ? bestTarget.transform : null;
    }


    //when missile dies, part of its final action releases an ship from tracking
    //  Essential since a missile can die prematurely
    public void ReleaseTarget(Transform _o)
    {
        TrackedObjects.Remove(_o.gameObject.GetComponent<Collider>());
    }

    //Service Interface
    public void Initiallise()
    {
        TrackedObjects = new List<Collider>();
    }
    public void Reset()
    {
        if (TrackedObjects != null)
        {
            TrackedObjects.Clear();
        }
    }
}
