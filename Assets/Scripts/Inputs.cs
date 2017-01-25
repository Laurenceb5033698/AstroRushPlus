using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour 
{
	public float zAxis;
	public float xAxis;
	public float yawAxis;
    public float rightY;

	public bool boost;
	public bool rocket;
	public bool RLaser;

	public bool targeting;

    public bool reset;


    void Update()
    {
        CheckInputs();
    }

	private void CheckInputs()
	{
		yawAxis = Input.GetAxis ("RightStickX");
        rightY = Input.GetAxis("RightStickY");
		zAxis = Input.GetAxis ("LeftStickY");
		xAxis = Input.GetAxis ("LeftStickX");

        CheckKeyboard();


        rocket = Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.R);
		targeting = Input.GetMouseButtonDown (0);
        RLaser = Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.F);
        boost = Input.GetAxis("LeftTrigger") > 0.1f || Input.GetKey(KeyCode.Space);
        reset = Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Y);
    }

    private void CheckKeyboard()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) zAxis = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) zAxis = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) xAxis = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) xAxis = 1;

        if (Input.GetKey(KeyCode.Q)) yawAxis = -1;
        else if (Input.GetKey(KeyCode.E)) yawAxis = 1;
    }
}
