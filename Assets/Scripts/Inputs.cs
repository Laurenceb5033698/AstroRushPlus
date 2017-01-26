using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour 
{
    public Vector2 LeftStick;
    public Vector2 RightStick;

	public bool boost;
	public bool rocket;
	public bool RLaser;

    public bool shoot;
	public bool targeting;

    public bool reset;


    void Update()
    {
        CheckInputs();
    }

	private void CheckInputs()
	{
        LeftStick.x = Input.GetAxis("LeftStickX");
        LeftStick.y = Input.GetAxis("LeftStickY");
        RightStick.x = Input.GetAxis("RightStickX");
        RightStick.y = Input.GetAxis("RightStickY");

        CheckKeyboard();


        rocket = Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.R);
		targeting = Input.GetMouseButtonDown (0);
        RLaser = Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.F);
        boost = Input.GetAxis("LeftTrigger") > 0.1f || Input.GetKey(KeyCode.LeftShift);
        shoot = Input.GetAxis("RightTrigger") > 0.1f || Input.GetKey(KeyCode.Space);
        reset = Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Y);
    }

    private void CheckKeyboard()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) LeftStick.y = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) LeftStick.y = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) LeftStick.x = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) LeftStick.x = 1;

        if (Input.GetKey(KeyCode.Q)) RightStick.x = -1;
        else if (Input.GetKey(KeyCode.E)) RightStick.x = 1;
    }
}
