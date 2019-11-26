using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpProjectile : Projectile {

    public float EMPDuration = 1.0f;

    protected override void OnTriggerEnter(Collider other)
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
                    other.gameObject.GetComponentInParent<Stats>().SetDisable(EMPDuration/2);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    hit = true;
                    break;
                case "EnemyShip":
                    other.gameObject.GetComponentInParent<AICore>().TakeDamage(transform.position, damage);
                    other.gameObject.GetComponentInParent<Stats>().SetDisable(EMPDuration);
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

            if (hit) DestroySelf();

        }
    }



}
