using UnityEngine;
using System.Collections;

public class SmallCargo : MonoBehaviour {

    public GameObject cargoGo;
    //public Rigidbody rb;

    private const float maxCargo = 400f;
    [Range(0, 400)]
    public float cargo = 0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //float temp = cargo / 400 * 100;
        //rb.mass = 5 / 100 * temp + 1;
    }

    public float GetCargoAmount()
    {
        return cargo;
    }
}
