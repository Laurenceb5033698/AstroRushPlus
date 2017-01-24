using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    //private float health;


    [SerializeField] private bool shieldActive;
    [SerializeField] private GameObject shield;
    [SerializeField] private ShipStats stats;


	// Use this for initialization
	void Start ()
    {
        //stats = new ShipStats();
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
        stats.TakeDamage(val);
        if (!stats.IsAlive())
            shieldActive = false;

        //Debug.Log("Gen Health: " + stats.ShipHealth);
    }

    public bool GetShieldStatus()
    {
        return shieldActive;
    }
}
