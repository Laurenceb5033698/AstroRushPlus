using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour 
{
    // http://wiki.unity3d.com/index.php?title=Xbox360Controller

    //Analogue Sticks
	public Vector2 LeftStick = new Vector2(0,0);
	public Vector2 RightStick = new Vector2(0,0);
    public bool LeftAnalogueInUse = false;
    public bool RightAnalogueInUse = false;

    public bool LAnalogueYUp;
    public bool LAnalogueYDown;
    public bool LAnalogueXLeft;
    public bool LAnalogueXRight;

    //Dpad
    private Vector2 Dpad = new Vector2(0, 0);
    public bool DpadXaxisInUse = false;
    public bool DpadYaxisInUse = false;
    public bool DpadXPressed = false;
    public bool DpadYPressed = false;

    public bool DpadUp;
    public bool DpadDown;
    public bool DpadLeft;
    public bool DpadRight;

    //Triggers
    public float RTrigger = 0.0f;
    public float LTrigger = 0.0f;
    public bool RTriggerInUse = false;
    public bool RTriggerPressed = false;
    public bool LTriggerInUse = false;
    public bool LTriggerPressed = false;

    public bool ability = false;
	public bool trishot = false;
	public bool shield = false;
	public bool RLaser = false;
	public bool shoot = false;
	public bool targeting = false;
	public bool reset = false;
    
    void Update()
    {
        DpadXPressed = false;
        DpadYPressed = false;
        LAnalogueYUp = false;
        LAnalogueYDown = false;
        LAnalogueXLeft = false;
        LAnalogueXRight = false;
        RTriggerPressed = false;
        LTriggerPressed = false;
        CheckInputs();
    }

	private void CheckInputs()
	{
        LeftStick.x = Input.GetAxis("LeftStickX");
        LeftStick.y = Input.GetAxis("LeftStickY");
        RightStick.x = Input.GetAxis("RightStickX");
        RightStick.y = Input.GetAxis("RightStickY");
        
        
        if (!LeftAnalogueInUse && LeftStick.y > 0.02f)
        {
            LAnalogueYUp = true;
        }
        if (!LeftAnalogueInUse && LeftStick.y < -0.02f)
        {
            LAnalogueYDown = true;
        }
        if (!LeftAnalogueInUse && LeftStick.x > 0.02f)
        {
            LAnalogueXLeft = true;
        }
        if (!LeftAnalogueInUse && LeftStick.x < -0.02f)
        {
            LAnalogueXRight = true;
        }

        if (Mathf.Abs(LeftStick.x) > 0.02f || Mathf.Abs(LeftStick.y) > 0.02f)
            LeftAnalogueInUse = true;
        else
            LeftAnalogueInUse = false;

        if (Mathf.Abs(RightStick.x) > 0.02f || Mathf.Abs(RightStick.y) > 0.02f)
            RightAnalogueInUse = true;
        else
            RightAnalogueInUse = false;

        DirectionalPad();
        TriggerAxis();
        CheckKeyboard();

        reset = Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Y);
    }

    private void CheckKeyboard()
    {
        LTriggerPressed = LTriggerPressed || Input.GetKeyDown(KeyCode.LeftShift);
        LTriggerInUse = LTriggerInUse || Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { LeftStick.y = 1; LeftAnalogueInUse = true; }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { LeftStick.y = -1; LeftAnalogueInUse = true; }
            

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { LeftStick.x = -1; LeftAnalogueInUse = true; }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { LeftStick.x = 1; LeftAnalogueInUse = true; }
    }

    private void DirectionalPad()
    {
        Dpad.x = Input.GetAxis("XboxDpadHorizontal");
        Dpad.y = Input.GetAxis("XboxDpadVertical");
        DpadUp = Dpad.y > 0;
        DpadDown = Dpad.y < 0;
        DpadRight = Dpad.x < 0;
        DpadLeft = Dpad.x > 0;

        //Dpad on-pressed logic
        if (Dpad.x != 0 && !DpadXaxisInUse)
        {
            DpadXPressed = true;
            DpadXaxisInUse = true;
        }
        if (Dpad.x == 0)
            DpadXaxisInUse = false;
        if (Dpad.y != 0 && !DpadYaxisInUse)
        {
            DpadYPressed = true;
            DpadYaxisInUse = true;
        }
        if (Dpad.y == 0)
            DpadYaxisInUse = false;
    }   

    private void TriggerAxis()
    {
        RTrigger = Input.GetAxis("RightTrigger");
        if (RTrigger != 0 && !RTriggerInUse)
        {
            RTriggerPressed = true;
            RTriggerInUse = true;
        }
        if (RTrigger == 0)
            RTriggerInUse = false;

        LTrigger = Input.GetAxis("LeftTrigger");
        if (LTrigger != 0 && !LTriggerInUse)
        {
            LTriggerPressed = true;
            LTriggerInUse = true;
        }
        if (LTrigger == 0)
            LTriggerInUse = false;
    }
}