using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour 
{
    // http://wiki.unity3d.com/index.php?title=Xbox360Controller


	public Vector2 LeftStick = new Vector2(0,0);
	public Vector2 RightStick = new Vector2(0,0);

	public bool boost = false;
	public bool rocket = false;
	public bool trishot = false;
	public bool shield = false;
	public bool RLaser = false;
	public bool shoot = false;
	public bool targeting = false;
	public bool reset = false;

    
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

        boost = Input.GetAxis("LeftTrigger") > 0.1f || Input.GetKey(KeyCode.LeftShift);
        reset = Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Y);
    }

    private void CheckKeyboard()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) LeftStick.y = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) LeftStick.y = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) LeftStick.x = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) LeftStick.x = 1;   
    }
}
