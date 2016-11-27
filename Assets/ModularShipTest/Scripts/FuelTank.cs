using UnityEngine;
using System.Collections;

public class FuelTank : MonoBehaviour {

    public GameObject fuelTank;
    public GameObject Indicator;
    public Rigidbody rb;

    public float fuelAmount = 100f; // %
    [Range(0, 1000)]
    public float fuel = 1000f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float temp = fuel / 1000 * 100;
        fuelAmount = temp;
        rb.mass = 5 / 100 * temp + 1;

        Indicator.transform.localScale = new Vector3(1f,1f,1f/100*fuelAmount);

	}
}
