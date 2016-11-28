using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour 
{
	public float zAxis;
	public float xAxis;
	public float yawAxis;

	public bool boost;
	public bool rocket;
	public bool RLaser;

	public bool targeting;

    public bool reset;

    public UI ui;
	

    public void UpdateInputs()
    {
        CheckInputs();
    }


	private void CheckInputs()
	{
		yawAxis = Input.GetAxis ("RightStickX");
		zAxis = Input.GetAxis ("LeftStickY");
		xAxis = Input.GetAxis ("LeftStickX");

        CheckKeyboard();


        rocket = Input.GetKeyDown (KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.R);
		targeting = Input.GetMouseButtonDown (0);
        RLaser = Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.F);
		boost = Input.GetKeyDown (KeyCode.JoystickButton8) || Input.GetKeyDown(KeyCode.Space);
        reset = Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Y);
    }

    private void CheckKeyboard()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            zAxis = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            zAxis = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            xAxis = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            xAxis = 1;

        if (Input.GetKey(KeyCode.Q))
            yawAxis = -1;
        else if (Input.GetKey(KeyCode.E))
            yawAxis = 1;
    }
}
