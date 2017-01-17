using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    private float health;
    private bool dead = false;

    // default constructor
    public Health()
    {
        health = 100.0f;
    }
    public Health(float h)
    {
        health = h;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(float amount)
    {
        health -= Mathf.Abs(amount);

        if (health <= 0)
        {
            health = 0.0f;
            dead = true;       
            Debug.Log("Ship is dead");
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    public float GetHealth()
    {
        return health;
    }

   
}
