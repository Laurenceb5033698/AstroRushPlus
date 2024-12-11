using UnityEngine;
using System.Collections;

public class BoundaryManager : MonoBehaviour {

    [SerializeField] private GameObject boundaryz;
    [SerializeField] private GameObject boundaryx;

    private GameObject ship; // player's ship

    private bool state = false;

    private const int SBOUND = 600;
    private const int HBOUND = SBOUND + 70;

    //damage period
    public float PeriodicDamageCooldown = 2.0f;
    private float lastPeriodicDamage = 0;

    // Use this for initialization
    void Start ()
    {
        
	}
    void OnEnable()
    {
        ship = GetComponent<GameManager>().GetShipRef();
        boundaryx.GetComponent<BoundaryLine>().setShipRef(ship);
        boundaryz.GetComponent<BoundaryLine>().setShipRef(ship);
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateBoundary();

    }

    private void UpdateBoundary()
    {
        if (Mathf.Abs(ship.transform.position.x) > SBOUND || Mathf.Abs(ship.transform.position.z) > SBOUND)
        {
            //transform.GetComponent<UI>().setMessage(5); // show boundary warning
            state = true;
            boundaryx.GetComponent<BoundaryLine>().drawstate = true;
            boundaryz.GetComponent<BoundaryLine>().drawstate = true;
        }
        else
        {
            //transform.GetComponent<UI>().setMessage(2); // show paused state (default)
            state = false;
            boundaryx.GetComponent<BoundaryLine>().drawstate = false;
            boundaryz.GetComponent<BoundaryLine>().drawstate = false;
        }

        if (Mathf.Abs(ship.transform.position.x) > HBOUND || Mathf.Abs(ship.transform.position.z) > HBOUND)
        {
            //periodically deal 1 damage while out of bounds.
            if (Time.time - lastPeriodicDamage < PeriodicDamageCooldown)
                return;
            
            ship.GetComponent<ShipStats>().TakeDamage(1); //every sec ship takes 5% damage
            lastPeriodicDamage = Time.time;
            
        }
    }

    public bool GetState()
    {
        return state;
    }

}
