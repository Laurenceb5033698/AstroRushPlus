using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

	//---- CAMERAS ---------------
	public Camera FPV;
	public Camera ThirdP;
	//-------------------------------------

	public bool forward;
	public bool backward;

	public bool left;
	public bool right;

	public bool up;
	public bool down;


	public bool rollLeft;
	public bool rollRight;

	public bool PitchUp;
	public bool PitchDown;

	public bool yawLeft;
	public bool yawRight;

	public bool IncThrust;
	public bool DecThrust;
	public float ThrustLevel = 0f;

	public bool boost;
	public bool rocket;
	public bool GLaser;
	public bool RLaser;

	public bool targeting;
		
	// Use this for initialization
	void Start () 
	{
		FPV.enabled = false;
		ThirdP.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		CheckInputs ();
	}


	private void CheckInputs()
	{
		forward = Input.GetKey (KeyCode.I);
		backward = Input.GetKey (KeyCode.K);

		left = Input.GetKey (KeyCode.J);
		right = Input.GetKey (KeyCode.L);

		up = Input.GetKey (KeyCode.F);
		down = Input.GetKey (KeyCode.V);

		rollLeft = Input.GetKey (KeyCode.Q);
		rollRight = Input.GetKey (KeyCode.E);

		PitchUp = Input.GetKey (KeyCode.S);
		PitchDown = Input.GetKey (KeyCode.W);

		yawLeft = Input.GetKey (KeyCode.A);
		yawRight = Input.GetKey (KeyCode.D);

		IncThrust = Input.GetKey (KeyCode.LeftShift);
		DecThrust = Input.GetKey (KeyCode.LeftControl);

		rocket = Input.GetKeyDown (KeyCode.Space);


		targeting = Input.GetMouseButtonDown (0);
		//GLaser = Input.GetMouseButton(0);
		GLaser = false;
		RLaser = Input.GetMouseButton(1);

		if (IncThrust && ThrustLevel <= 99.5f) {
			ThrustLevel += 0.5f;
		}
		if (DecThrust && ThrustLevel >= 0.5f) {
			ThrustLevel -= 0.5f;
		}
		if (Input.GetKeyDown (KeyCode.X))
			ThrustLevel = 0f;

		boost = Input.GetKeyDown (KeyCode.B);

		if (Input.GetKeyDown (KeyCode.C)) {
			FPV.enabled = !FPV.enabled;
			ThirdP.enabled = !ThirdP.enabled;
		}
	}
}
