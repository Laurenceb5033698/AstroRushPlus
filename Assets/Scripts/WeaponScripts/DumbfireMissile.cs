using UnityEngine;

public class DumbfireMissile : Missile
{
    //protected Rigidbody rb;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        //lifetime = Time.time + 5f;
    }

    protected override void Update()
    {
        m_Stats.lifetime -= Time.deltaTime;
        if (m_Stats.lifetime < 0)
        {
            DestroySelf();
        }
        else
        {
            //rb.AddForce(transform.forward * 3000 * Time.deltaTime, ForceMode.Force);
        }
    }

    //protected override void OnTriggerEnter(Collider _c)
    //{
    //    if ((_c.gameObject.GetComponent<Projectile>() == null) && (_c.gameObject.GetComponent<PickupItem>() == null) && !(_c.CompareTag(ownertag)))
    //    {
    //        Damageable otherDamageable = _c.GetComponent<Damageable>();
    //        if (otherDamageable)
    //        {
    //            otherDamageable.TakeDamage(transform.position, CalcDamage());
    //            applyImpulse(otherDamageable.GetRigidbody());
    //        }

    //        DestroySelf();
    //    }
    //}

    protected override void DestroySelf()
    {
        SpawnHitVisuals();
        Destroy(transform.gameObject);
    }
}
