using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    private float health;


    [SerializeField] private bool shieldActive;
    [SerializeField] private GameObject shield;
    




	// Use this for initialization
	void Start ()
    {
        health = 200;
        shieldActive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void FixedUpdate()
    {
        shield.SetActive(shieldActive);
    }

    public void TakeDamage(float val)
    {
        health -= Mathf.Abs(val);

        if (health < -0.1f)
        {
            shieldActive = false;
            health = 0.0f;
        }

        //Debug.Log("Shield Health: " + health);
    }

    public bool GetShieldStatus()
    {
        return shieldActive;
    }
}
