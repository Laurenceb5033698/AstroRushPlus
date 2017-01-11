using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

    [SerializeField] private int type = 0;


	// Use this for initialization
	void Start () 
    {
        type = Random.Range(1,5);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public int getType() 
    {
        return type;
    }
    public string getPickup()
    {
        switch (type)
        {
            case 1: return "fuel";
            case 2: return "shield";
            case 3: return "health";
            case 4: return "rocket";
            default: return "wrongType";
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.gameObject.name == "NewShip")
        {
            switch (type)
            {
                case 1: c.transform.gameObject.GetComponentInParent<ShipStats>().ShipFuel = 100f; Debug.Log("Boost Recharged"); break;
                case 2: /*recharge shield*/; Debug.Log("Shield Recharged"); break;
                case 3: c.transform.gameObject.GetComponentInParent<ShipStats>().ShipDamage = -100; Debug.Log("Ship Repaired"); break;
                case 4: c.transform.gameObject.GetComponentInParent<ShipStats>().addMissile(20); Debug.Log("Missile Reloaded"); break;
                default: Debug.Log("WARNING! Wrong pickup type"); break;
            }

            Destroy(transform.gameObject);
        }

    }
}
