using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [SerializeField] private float damage;
    [SerializeField] private float lifetime = 5f;//lifetime in seconds
    [SerializeField] public string ownertag; //(eg player)
    [SerializeField] private float speed;//forward speed


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
    void OnTriggerEnter(Collider other)
    {//deal damage to target
        //Debug.Log("Entity hit: " + other.gameObject.name);

        if ((other != null) && (other.gameObject.tag != ownertag))
        {//successful collision that wasnt with shooter
            //Debug.Log("other Entity: " + other.gameObject.tag);
            switch (other.gameObject.tag)
            {
                case "PlayerShip":
                    other.gameObject.GetComponentInParent<ShipController>().TakeDamage(damage);
                    break;
                case "EnemyShip":
                    other.gameObject.GetComponentInParent<NewBasicAI>().TakeDamage(damage);
                    break;
                case "Asteroid":
                    other.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                    break;
                default:
                    Debug.Log("Unknown entity. " + other.gameObject.tag);

                    break;
            }
            DestroySelf();
        }
    }
    
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            DestroySelf();
	}

    private void DestroySelf()
    {// perhaps spawn a particle? like missile does
        Destroy(transform.gameObject);
    }
}
