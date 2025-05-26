using UnityEngine;

public class NukeComponent : BaseMissileComponent
{

    public override void OnCollide()
    {
        //get missile aoe stats
        //aoeSize
        //aoeDamage
        //

        string ownertag = GetComponent<Projectile>().ownertag;
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
                float damageOverDistance = (bulletStats.aoeDamage - 2 * dist);
                otherDamageable.TakeDamage(transform.position, damageOverDistance);
                GetComponent<Projectile>().applyImpulse(otherDamageable.GetRigidbody());
            }

            //deal lower damage to player ship
            //also destroy projectiles? ->maybe save that effect for emp
        }
    }

    public override void PerUpdate()
    {
        //flashing vfx
        //changes over time
    }
}
