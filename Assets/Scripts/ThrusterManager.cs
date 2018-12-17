using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrusterManager : MonoBehaviour {

    [SerializeField] private Thruster[] thrusters = new Thruster[4];
    private Inputs controls;

    private bool state; // master state
    private const float deadzone = 0.1f;

	// Use this for initialization
	void Start ()
    {
        controls = GameManager.instance.GlobalInputs;
        state = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateThrusters();
	}

    private void UpdateThrusters()
    {
        state = controls.LeftAnalogueInUse;
        thrusters[2].SetState(state); // rear
        thrusters[3].SetState(state); // rear


        /*
        if (state)
        {
            //thrusters[0].SetState(controls.LeftStick.x < -deadzone); // right
            //thrusters[1].SetState(controls.LeftStick.x > deadzone);  // left

            thrusters[2].SetState(controls.LeftStick.y > deadzone); // rear
            thrusters[3].SetState(controls.LeftStick.y > deadzone); // rear
        }
        else
        {
            foreach (Thruster t in thrusters)
            {
                t.SetState(false);
            }
        }
        */
    }

    
}
