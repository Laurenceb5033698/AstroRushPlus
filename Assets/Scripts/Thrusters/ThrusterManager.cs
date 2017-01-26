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
        controls = GetComponentInParent<Inputs>();
        state = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateThrusters();
	}

    private void UpdateThrusters()
    {
        state = (Mathf.Abs(controls.LeftStick.x) > deadzone || controls.LeftStick.y > deadzone);
        if (state)
        {
            thrusters[0].SetState(controls.LeftStick.x < -deadzone); // right
            thrusters[1].SetState(controls.LeftStick.x > deadzone);  // left

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
    }

    
}
