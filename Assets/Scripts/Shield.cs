using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    private Stats stats;
    private MeshRenderer mr;

    
	void Start () 
    {
	    stats = transform.gameObject.GetComponentInParent<Stats>();
        mr = GetComponent<MeshRenderer>();
	}


    void Update () 
    {
        
        mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, 0.2f * (stats.ShipShield / stats.GetShieldMax()));
        //mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, (strength/100));
        mr.enabled = (stats.ShipShield < 0.1f) ? false : true;
    }
}
