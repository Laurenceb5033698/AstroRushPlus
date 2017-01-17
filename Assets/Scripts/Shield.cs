using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    private ShipStats stats;

	// Use this for initialization
	void Start () 
    {
	    stats = transform.gameObject.GetComponentInParent<ShipStats>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        ShieldSphereOpacity();
	}

    public void ShieldSphereOpacity()
    {
        float newalpha = 0.5f * (stats.ShipShield / 100);

        Color oldcol = transform.GetComponent<MeshRenderer>().materials[0].color;
        oldcol = new Color(oldcol.r, oldcol.g, oldcol.b, newalpha);
        transform.GetComponent<MeshRenderer>().materials[0].color = oldcol;

        //Debug.Log("shield amount: " + stats.ShipShield);
        if (stats.ShipShield < 0.1f)
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            transform.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
