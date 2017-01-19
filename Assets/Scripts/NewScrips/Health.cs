using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    protected float health = 100.0f;

    //void OnCollisionEnter(Collision c)
    //{
    //    if (c.relativeVelocity.magnitude > 5f) TakeDamage(c.relativeVelocity.magnitude);
    //}

    public virtual void TakeDamage(float amount)
    {
        health -= Mathf.Abs(amount);

        if (health <= 0)
        {
            health = 0.0f;    

            //switch (transform.gameObject.tag)
            //{
            //    case "Asteroid": transform.gameObject.GetComponent<ID>().Reset(); break;
            //}
        }
    }
    public bool IsAlive()
    {
        return (health > 0);
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float h)
    {
        health = h;
    }

   
}
