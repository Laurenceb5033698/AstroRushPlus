using UnityEngine;

public class DumbfireMissile : Projectile
{
    protected Rigidbody rb;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        lifetime = Time.time + 5f;
    }

    protected override void Update()
    {
        if (Time.time > lifetime)
        {
            DestroySelf();
        }
        else
        {
            rb.AddForce(transform.forward * 3000 * Time.deltaTime, ForceMode.Force);
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.GetComponent<Projectile>() == null) && (collision.gameObject.GetComponent<PickupItem>() == null))
        {
            if (collision.gameObject.tag == "Asteroid")
            {
                collision.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                applyImpulse(collision.GetComponent<Rigidbody>());
            }
            else if (collision.gameObject.tag == "EnemyShip")
            {
                collision.gameObject.GetComponentInParent<AICore>().TakeDamage(transform.position, damage);
                applyImpulse(collision.GetComponentInParent<Rigidbody>());
            }

            DestroySelf();
        }
    }

    protected override void DestroySelf()
    {
        Instantiate(psImpactPrefab, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
}
