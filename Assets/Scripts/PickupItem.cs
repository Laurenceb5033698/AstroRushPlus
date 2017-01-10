using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

    public int type = 0;


	// Use this for initialization
	void Start () 
    {
        type = Random.Range(0,4);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public int getType() 
    {
        return type;
    }
    public string getPickup()
    {
        switch (type)
        {
            case 1: return "fuel";
            case 2: return "shield";
            case 3: return "health";
            case 4: return "rocket";
            default: return "wrongType";
        }
    }
}
