using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingComponent : BaseMissileComponent
{
    Transform m_target;
    public override void OnInit()
    {
        m_target = FindTarget();
    }

    private Transform FindTarget()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Collider[] targetColliders = Physics.OverlapSphere(transform.position, 90f);
        foreach (Collider col in targetColliders)
        {
            //Debug.Log(col.gameObject.name); 
            if (col.gameObject.GetComponentInParent<AICore>() != null)
            {
                Vector3 directionToTarget = col.gameObject.transform.position - transform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = col.gameObject;
                }
            }
        }
        return bestTarget.transform;
    }

    public override void PerFixed()
    {
        if (m_target)
        {
            //turn towards target.
            Vector3 direction = (m_target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), 300 * Time.deltaTime);
        }
    }
}
