using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

    [SerializeField] private int type = 0;
    [SerializeField] private GameObject item;
    [SerializeField] private bool stayOnScene = false;

    private float speed = 1f;
    private bool directionUp = true;

    

	// Use this for initialization
	void Start () 
    {
        //type = Random.Range(1,5);
	}
	
	// Update is called once per frame
	void Update () 
    {
        item.transform.Rotate(Vector3.forward * 30f * Time.deltaTime);

        if (directionUp)
        {
            if (item.transform.position.y < 2f)
            {
                item.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
                directionUp = false;
        }
        else
        {
            if (item.transform.position.y >= 0f)
            {
                item.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
            }
            else
                directionUp = true;
        }
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
                case 2: c.transform.gameObject.GetComponentInParent<ShipStats>().ShipShield = 40; Debug.Log("Shield Recharged"); break;
                case 3: c.transform.gameObject.GetComponentInParent<ShipStats>().ShipHealth = 100; Debug.Log("Ship Repaired"); break;
                case 4: c.transform.gameObject.GetComponentInParent<ShipStats>().addMissile(20); Debug.Log("Missile Reloaded"); break;
                default: Debug.Log("WARNING! Wrong pickup type"); break;
            }

            if (!stayOnScene)
            Destroy(transform.gameObject);
        }

    }
}
