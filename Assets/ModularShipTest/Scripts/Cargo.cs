using UnityEngine;
using System.Collections;

public class Cargo : MonoBehaviour {


    public GameObject cargoGo;
    //public Rigidbody rb;

    private const float maxCargo = 1000f;
    [Range(0,1000)]
    public float cargo = 0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float temp = cargo / 1000 * 100;
        //rb.mass = 5 / 100 * temp + 1;
    }

    public float GetCargoAmount()
    {
        return cargo;
    }
}
