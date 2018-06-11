using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    private ShipStats stats;
    private MeshRenderer mr;
    private float MaxShield;

	// Use this for initialization
	void Start () 
    {
	    stats = transform.gameObject.GetComponentInParent<ShipStats>();
        mr = GetComponent<MeshRenderer>();
        MaxShield = stats.GetShieldMax();
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, 0.2f * (stats.ShipShield / MaxShield));
        //mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, (strength/100));
        mr.enabled = (stats.ShipShield < 0.1f) ? false : true;
    }
}
