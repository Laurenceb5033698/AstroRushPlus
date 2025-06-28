using UnityEngine;

public class NukeComponent : BaseMissileComponent
{

    public override void OnCollide()
    {
        string ownertag = GetComponent<Projectile>().ownertag;

        //constant for scaling damage with distance.
        float DamageScalerFactor = (1/(4* bulletStats.aoeRadius));

        //do big aoe damage
        Collider[] inRange = Physics.OverlapSphere(transform.position, bulletStats.aoeRadius);
        foreach (Collider victim in inRange)
        {
            if (victim.CompareTag(ownertag))
            {
                //do not damage owner.
                continue;
            }

            float dist = Vector3.Distance(victim.transform.position, transform.position);
            Damageable otherDamageable = victim.GetComponent<Damageable>();
            if (otherDamageable)
            {
                //dmg decreases over distance, to a minmum of 0.5x at the edge.
                float damageOverDistance = bulletStats.aoeDamage - (bulletStats.aoeDamage * (dist * DamageScalerFactor));
                otherDamageable.TakeDamage(transform.position, damageOverDistance);
                GetComponent<Projectile>().applyImpulse(otherDamageable.GetRigidbody());
            }

            //deal lower damage to player ship?
            //also destroy projectiles? ->maybe save that effect for emp
        }
    }

    public override void PerUpdate()
    {
        //flashing vfx
        //  changes over time
    }
}
