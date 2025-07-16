using UnityEngine;

public class HomingComponent : BaseMissileComponent
{
    Transform m_target;
    public override void OnInit()
    {
        m_target = ServicesManager.Instance.HomingService.FindTarget(this.transform);
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
        return bestTarget ? bestTarget.transform : null;
    }

    public override void PerFixed()
    {
        if (m_target)
        {
            //turn towards target.
            Vector3 direction = (m_target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), 240 * Time.deltaTime);
        }
    }

    public override void OnCollide()
    {
        if(m_target)
            ServicesManager.Instance.HomingService.ReleaseTarget(m_target);
    }
}
