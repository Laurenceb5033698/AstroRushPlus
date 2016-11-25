using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour 
{
	public float zAxis;
	public float xAxis;
	public float yawAxis;

	public bool boost;
	public bool rocket;
	public bool RLaser;

	public bool targeting;
		
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckInputs ();
	}


	private void CheckInputs()
	{
		yawAxis = Input.GetAxis ("RightStickX");
		zAxis = Input.GetAxis ("LeftStickY");
		xAxis = Input.GetAxis ("LeftStickX");

		rocket = Input.GetKeyDown (KeyCode.JoystickButton0);
		targeting = Input.GetMouseButtonDown (0);
		RLaser = Input.GetKey(KeyCode.JoystickButton4);
		boost = Input.GetKeyDown (KeyCode.JoystickButton8);
	}
}
