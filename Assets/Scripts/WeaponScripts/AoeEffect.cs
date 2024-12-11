using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeEffect : MonoBehaviour {

    public string ownertag; //(eg player)
    public float lifetime = 5f;//lifetime in seconds

    //large trigger area
    //[SerializeField] float effectRadius = 5.0f;

    //causes an effect to be applied to triggering entity
    [SerializeField] public enum Effect { DAMAGE, EMP, REPULSION, ATTRACTION };
    public Effect mEffect = Effect.DAMAGE;

    [SerializeField] int effectAmount = 1;

    public float PeriodicEffectCooldown = 1.0f;
    private float LastPeriodicEffect = 0;

    void Start () {
        
	}

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            DestroySelf();
    }

    public void SetupValues(int dmg, string str)
    {
        ownertag = str;
        effectAmount = dmg;
    }
    private void OnTriggerStay(Collider other)
    {   //while Triggering
        //check other type
        if ((other.gameObject.tag != ownertag) && (other.gameObject.GetComponent<Projectile>() == null))
        {   //apply effect
            switch (other.gameObject.tag)
            {
                case "PlayerShip":
                    ApplyEffectToPlayer(other.gameObject);
                    break;
                case "EnemyShip":
                    ApplyEffectToAI(other.gameObject);
                    break;
                case "Asteroid":
                    ApplyEffectToAsteroid(other.gameObject, false);
                    break;
                case "shard":
                    ApplyEffectToAsteroid(other.gameObject, true);
                    break;
                default:
                    Debug.Log("Unknown entity. " + other.gameObject.tag);

                    break;
            }
        }
    }
    
    private void ApplyEffectToPlayer(GameObject other)
    {
        switch (mEffect)
        {
            case Effect.DAMAGE:
                if (Time.time - LastPeriodicEffect < PeriodicEffectCooldown)
                    break;
                other.GetComponentInParent<PlayerController>().TakeDamage(transform.position, effectAmount);
                break;
            case Effect.EMP:
                other.GetComponentInParent<Stats>().SetDisable(0.25f/2);
                break;
            case Effect.REPULSION:
                applyImpulse(other.GetComponentInParent<Rigidbody>(), -1);
                break;
            case Effect.ATTRACTION:
                applyImpulse(other.GetComponentInParent<Rigidbody>(), 1);
                break;

        }
    }

    private void ApplyEffectToAI(GameObject other)
    {   
        switch (mEffect)
        {
            case Effect.DAMAGE:
                if (Time.time - LastPeriodicEffect < PeriodicEffectCooldown)
                    break;
                other.GetComponentInParent<AICore>().TakeDamage(transform.position, effectAmount);
                break;
            case Effect.EMP:
                other.GetComponentInParent<Stats>().SetDisable(0.25f);
                break;
            case Effect.REPULSION:
                applyImpulse(other.GetComponentInParent<Rigidbody>() ,-1);
                break;
            case Effect.ATTRACTION:
                applyImpulse(other.GetComponentInParent<Rigidbody>(), 1);
                break;

        }
        
    }
    private void ApplyEffectToAsteroid(GameObject other, bool shard)
    {
        
            switch (mEffect)
            {
                case Effect.DAMAGE:
                if (shard) Destroy(other);
                else
                {
                    if (Time.time - LastPeriodicEffect < PeriodicEffectCooldown)
                        break;
                    other.gameObject.GetComponent<Asteroid>().TakeDamage(effectAmount);
                }
                    break;
                case Effect.EMP:
                    //other.GetComponentInParent<Stats>().SetDisable(0.25f);    //  do nothing
                    break;
                case Effect.REPULSION:
                    applyImpulse(other.GetComponentInParent<Rigidbody>(), -2);
                    break;
                case Effect.ATTRACTION:
                    applyImpulse(other.GetComponentInParent<Rigidbody>(), 2);
                    break;

            }
        
        
    }


    protected virtual void applyImpulse(Rigidbody body, int dir)
    {
        Vector3 vecDir = transform.position - body.transform.position;
        vecDir.Normalize();
        vecDir *= dir;
        
        body.AddForce(vecDir * effectAmount , ForceMode.Force);
    }

    protected virtual void DestroySelf()
    {// perhaps spawn a particle? like missile does
        //Instantiate(psImpactPrefab, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
}
