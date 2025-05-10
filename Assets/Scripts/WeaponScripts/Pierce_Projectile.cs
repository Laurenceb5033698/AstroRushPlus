using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierce_Projectile : Projectile { 

    //pierce projectile should pass through objects
    //perhaps play a particle on entry and exit

	//ontrigger enter should only proc once per target
    protected override void OnTriggerEnter(Collider other)
    {
        //if ((other != null) && (other.gameObject.tag != ownertag) && (other.gameObject.GetComponent<Projectile>() == null))
        //{//successful collision that wasnt with shooter
        //    //Debug.Log("other Entity: " + other.gameObject.tag);
        //    bool hit = false;
        //    switch (other.gameObject.tag)
        //    {
        //        case "PlayerShip":
        //            other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(transform.position, damage);
        //            applyImpulse(other.GetComponentInParent<Rigidbody>());
        //            hit = true;
        //            break;
        //        case "EnemyShip":
        //            other.gameObject.GetComponentInParent<AICore>().TakeDamage(transform.position, m_Stats.damage);
        //            applyImpulse(other.GetComponentInParent<Rigidbody>());
        //            hit = true;
        //            break;
        //        case "Asteroid":
        //            other.gameObject.GetComponent<Asteroid>().TakeDamage(m_Stats.damage);
        //            applyImpulse(other.GetComponent<Rigidbody>());
        //            hit = true;
        //            break;
        //        case "shard":
        //            Destroy(other.transform.gameObject);
        //            hit = true;
        //            break;
        //        default:
        //            Debug.Log("Unknown entity. " + other.gameObject.tag);

        //            break;
        //    }
        //    //play entry particle here
        //    if (hit)
        //        Instantiate(psImpactPrefab, transform.position, transform.rotation);

        //}
    }

    //play exit particle here
    //private void OnTriggerExit(Collider other)
    //{
    //    
    //}

}
