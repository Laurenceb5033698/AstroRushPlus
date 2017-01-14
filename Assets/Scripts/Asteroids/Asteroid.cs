using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float hp = 70;

    public void TakeDamage(float amount)
    {//receives a positive value as damage
        hp -= amount;
        if (hp < 0)
        {
            Destroy(transform.gameObject);
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.magnitude > 5f)
        {
            Destroy(transform.gameObject);
        }
    }
}
