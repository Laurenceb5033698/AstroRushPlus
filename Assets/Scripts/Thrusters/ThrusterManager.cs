using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrusterManager : MonoBehaviour {

    [SerializeField] private Thruster[] thrusters = new Thruster[4];
    [SerializeField] private GameObject sm; // scene manager
    [SerializeField] private Rigidbody rb;

    private bool state; // master state
    private float deadzone = 0.1f;

	// Use this for initialization
	void Start ()
    {
        state = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateThrusters();
	}

    private void UpdateThrusters()
    {
        state = (Mathf.Abs(sm.GetComponent<Inputs>().xAxis) > deadzone || sm.GetComponent<Inputs>().zAxis > deadzone);
        if (state)
        {
            thrusters[0].SetState(sm.GetComponent<Inputs>().xAxis < -deadzone); // right
            thrusters[1].SetState(sm.GetComponent<Inputs>().xAxis > deadzone);  // left

            thrusters[2].SetState(sm.GetComponent<Inputs>().zAxis > deadzone); // rear
            thrusters[3].SetState(sm.GetComponent<Inputs>().zAxis > deadzone); // rear
        }
        else
        {
            foreach (Thruster t in thrusters)
            {
                t.SetState(false);
            }
        }
    }

    
}
