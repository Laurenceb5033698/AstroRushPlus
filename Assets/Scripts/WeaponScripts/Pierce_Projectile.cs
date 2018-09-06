using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierce_Projectile : Projectile { 

    //pierce projectile should pass through objects
    //perhaps play a particle on entry and exit

	//ontrigger enter should only proc once per target
    protected override void OnTriggerEnter(Collider other)
    {
        if ((other != null) && (other.gameObject.tag != ownertag) && (other.gameObject.GetComponent<Projectile>() == null))
        {//successful collision that wasnt with shooter
            //Debug.Log("other Entity: " + other.gameObject.tag);
            switch (other.gameObject.tag)
            {
                case "PlayerShip":
                    other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(transform.position, damage);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    break;
                case "EnemyShip":
                    other.gameObject.GetComponentInParent<NewBasicAI>().TakeDamage(transform.position, damage);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    break;
                case "Asteroid":
                    other.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
                    applyImpulse(other.GetComponent<Rigidbody>());
                    break;
                case "shard":
                    Destroy(other.transform.gameObject);
                    break;
                default:
                    Debug.Log("Unknown entity. " + other.gameObject.tag);

                    break;
            }
            //play entry particle here
            Instantiate(psImpactPrefab, transform.position, transform.rotation);

        }
    }

    //play exit particle here
    //private void OnTriggerExit(Collider other)
    //{
    //    
    //}

}
