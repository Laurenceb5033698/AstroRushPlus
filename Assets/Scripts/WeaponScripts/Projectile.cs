using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float damage;
    public float lifetime = 5f;//lifetime in seconds
    public string ownertag; //(eg player)
    public float speed;//forward speed
    public GameObject psImpactPrefab;//particleSystem prefab


    // Use this for initialization
    void Start () {
        //damage = 10f;
        //speed = 10f;
	    //test self collision
        ///ownertag = "PlayerShip";
	}
    public void SetupValues(float dmg, float spd,string str)
    {
        ownertag = str;
        damage = dmg;
        speed = spd;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {//deal damage to target
        //Debug.Log("Entity hit: " + other.gameObject.name);

        if ((other != null) && (other.gameObject.tag != ownertag) && (other.gameObject.GetComponent<Projectile>() == null))
        {//successful collision that wasnt with shooter
            //Debug.Log("other Entity: " + other.gameObject.tag);
            bool hit = false;
            switch (other.gameObject.tag)
            {
                case "PlayerShip":
                    other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(transform.position, damage);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    hit = true;
                    break;
                case "EnemyShip":
                    other.gameObject.GetComponentInParent<NewBasicAI>().TakeDamage(transform.position, damage);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    hit = true;
                    break;
                case "Asteroid":
                    other.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                    applyImpulse(other.GetComponent<Rigidbody>());
                    hit = true;
                    break;
                case "shard":
                    Destroy(other.transform.gameObject);
                    hit = true;
                    break;
                default:
                    Debug.Log("Unknown entity. " + other.gameObject.tag);

                    break;
            }
            
            if ( hit ) DestroySelf();

        }
    }

    protected virtual void applyImpulse(Rigidbody body)
    {
        //Vector3 direction = transform.position - body.transform.position;
        body.AddForce(transform.forward * ((damage/2)+(speed/(2+body.mass))), ForceMode.Impulse);
    }

    // Update is called once per frame
    protected virtual void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            DestroySelf();
	}

    protected virtual void DestroySelf()
    {// perhaps spawn a particle? like missile does
        Instantiate(psImpactPrefab, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
}
