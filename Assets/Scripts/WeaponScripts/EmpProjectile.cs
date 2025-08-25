using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpProjectile : Projectile {

    public float EMPDuration = 1.0f;

    protected void OnTriggerEnter(Collider other)
    {
        if ((other != null) && (!other.CompareTag(ownertag)) && (other.gameObject.GetComponent<Projectile>() == null))
        {
            bool hit = false;

            Damageable otherDamageable = other.GetComponent<Damageable>();
            if (otherDamageable)
            {
                otherDamageable.TakeDamage(OwnerEventSource, transform.position, CalcDamage());
                applyImpulse(otherDamageable.GetRigidbody());
                if(otherDamageable is Player_Damageable || otherDamageable is AI_Damageable)
                {
                    other.gameObject.GetComponentInParent<Stats>().SetDisable(EMPDuration);

                }
                hit = true;
            }

            if (hit)
                DestroySelf();
        }
    }

}
