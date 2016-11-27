using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour {

    public GameObject battery;
    public GameObject i25;
    public GameObject i50;
    public GameObject i75;
    public GameObject i100;

    [Range(0,100)]
    public float charge = 100;

    public Material positive;
    public Material negative;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        updateBatteryState();
    }

    public float GetChargeAmount()
    {
        return charge;
    }

    private void updateBatteryState()
    {
        if (charge > 75)
        {
            i25.GetComponent<Renderer>().material = positive;
            i50.GetComponent<Renderer>().material = positive;
            i75.GetComponent<Renderer>().material = positive;
            i100.GetComponent<Renderer>().material = positive;
        }
        else if (charge > 50)
        {
            i25.GetComponent<Renderer>().material = positive;
            i50.GetComponent<Renderer>().material = positive;
            i75.GetComponent<Renderer>().material = positive;
            i100.GetComponent<Renderer>().material = negative;
        }
        else if (charge > 25)
        {
            i25.GetComponent<Renderer>().material = positive;
            i50.GetComponent<Renderer>().material = positive;
            i75.GetComponent<Renderer>().material = negative;
            i100.GetComponent<Renderer>().material = negative;
        }
        else if (charge > 0)
        {
            i25.GetComponent<Renderer>().material = positive;
            i50.GetComponent<Renderer>().material = negative;
            i75.GetComponent<Renderer>().material = negative;
            i100.GetComponent<Renderer>().material = negative;
        }
        else
        {
            i25.GetComponent<Renderer>().material = negative;
            i50.GetComponent<Renderer>().material = negative;
            i75.GetComponent<Renderer>().material = negative;
            i100.GetComponent<Renderer>().material = negative;
        }

    }
}
