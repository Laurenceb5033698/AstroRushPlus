using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    private ShipStats stats;
    private MeshRenderer mr;
    private float shield;
    [Range(0, 100)]
    public float strength;

	// Use this for initialization
	void Start () 
    {
	    stats = transform.gameObject.GetComponentInParent<ShipStats>();
        mr = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        shield = stats.ShipShield;
        mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, 0.5f * (shield / 100));
        //mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, (strength/100));
        mr.enabled = (shield < 0.1f) ? false : true;
    }
}
