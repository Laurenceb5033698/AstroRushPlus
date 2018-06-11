using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

    [SerializeField] private int type = 0;
    [SerializeField] private GameObject item;
    [SerializeField] private bool stayOnScene = false;
    [SerializeField]
    private PickupManager pm;

    private float speed = 1f;
    private bool directionUp = true;
    private float KillTimer;

    public int refillval = 5;

	// Use this for initialization
	void Start () 
    {
        KillTimer = Time.time + 15;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (KillTimer < Time.time)
        {
            Destroy(transform.gameObject);
        }


        item.transform.Rotate(transform.forward * 30f * Time.deltaTime);

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

    public void Init(GameObject sm)
    {
        pm = sm.GetComponent<PickupManager>();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.gameObject.tag == "PlayerShip")
        {
            switch (type)
            {
				case 1: c.transform.gameObject.GetComponentInParent<ShipStats>().SetFuel(); //Debug.Log("Boost Recharged"); 
                    break;
				case 2: c.transform.gameObject.GetComponentInParent<ShipStats> ().SetShieldPU(); //Debug.Log("Shield Recharged"); 
                    break;
				case 3: c.transform.gameObject.GetComponentInParent<ShipStats>().SetHealth(); //Debug.Log("Ship Repaired"); 
                    break;
				case 4: //c.transform.gameObject.GetComponentInParent<ShipStats>().SetMissiles(); //Debug.Log("Missile Reloaded"); 
                    //c.gameObject.GetComponentInParent<Equipment>().AddAmmo(refillval);
                    c.gameObject.GetComponentInParent<PlayerController>().gameObject.GetComponentInChildren<Equipment>().AddAmmo(refillval);
                    break;
                default: //Debug.Log("WARNING! Wrong pickup type"); 
                    break;
            }

            pm.playPickupSound();
            
            if (!stayOnScene)
            Destroy(transform.gameObject);
        }

    }
}
