using UnityEngine;
using System.Collections;

public class BoundaryManager : MonoBehaviour {

    [SerializeField] private GameObject boundaryz;
    [SerializeField] private GameObject boundaryx;

    [SerializeField] private GameObject ship; // player's ship

    private const int SBOUND = 600;
    private const int HBOUND = SBOUND + 70;

    // Use this for initialization
    void Start () {
	
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
            transform.GetComponent<UI>().BoundaryWarning = true;//enable warning text
            boundaryx.GetComponent<BoundaryLine>().drawstate = true;
            boundaryz.GetComponent<BoundaryLine>().drawstate = true;
        }
        else
        {
            transform.GetComponent<UI>().BoundaryWarning = false; ;//hide warning text
            boundaryx.GetComponent<BoundaryLine>().drawstate = false;
            boundaryz.GetComponent<BoundaryLine>().drawstate = false;
        }

        if (Mathf.Abs(ship.transform.position.x) > HBOUND || Mathf.Abs(ship.transform.position.z) > HBOUND)
        {
            ship.GetComponent<ShipStats>().takeDamage(10.0f * Time.deltaTime); //every sec ship takes 5% damage
        }
    }

}
