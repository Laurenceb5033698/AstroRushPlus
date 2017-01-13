using UnityEngine;
using System.Collections;

public class WarpGate : MonoBehaviour {

    [SerializeField] private GameObject Gen1;
    [SerializeField] private GameObject Gen2;
    [SerializeField] private ParticleSystem gate;
    [SerializeField] private GameObject collider;
    [SerializeField] private UI ui;

    private bool GateIsActive;

	// Use this for initialization
	void Start ()
    {
        GateIsActive = false;
        collider.AddComponent<GateCollider>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (collider.GetComponent<GateCollider>().GetState() && GateIsActive)
        {
            //Debug.Log("WIN");
            ui.setMessage(1);
            ui.menu = true;
        }

    }

    void FixedUpdate()
    {
        GateIsActive = !(Gen1.GetComponent<Generator>().GetShieldStatus() || Gen2.GetComponent<Generator>().GetShieldStatus());
        gate.gameObject.SetActive(GateIsActive);
    }
}

public class GateCollider : MonoBehaviour
{

    private bool triggerActive;

    void Start()
    {
        triggerActive = false;
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.name + " at the gate");
        if (col.gameObject.name == "NewShip")
        {
            triggerActive = true;
        }
        else
            triggerActive = false;
    }

    void OnTriggerExit(Collider col)
    {
        triggerActive = false;
    }

    public bool GetState()
    {
        return triggerActive;
    }
}


