using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_Launch : ScreenElement
{
    [SerializeField] private GOEHeaderComponent Header;

    [SerializeField] private Image[] LaunchBars;

    new void OnEnable()
    {
        //connect onhold action
        if(Script_UI_InputManager.instance != null)
            Script_UI_InputManager.instance._HoldAction.performed += OnHoldPerformed;
        base.OnEnable();
    }

    new void OnDisable()
    {
        Script_UI_InputManager.instance._HoldAction.performed -= OnHoldPerformed;
        base.OnDisable();
    }

    //two buttons, submit and cancel
    //submit starts game
    //cancel goes back a step
    public override void Update()
    {
        //HandleSubmit();
        //HandleCancel();
        DemoHandleCancel();
        //header nav
        //Header.HandleRightBumper();
        Header.HandleLeftBumper();

        HoldToLaunch();
    }

    public void HoldToLaunch()
    {
        //while held, increase progress bar visuals
        if (Script_UI_InputManager.instance.HoldActionInput)
        {
            foreach (Image image in LaunchBars)
            {
                image.fillAmount = Script_UI_InputManager.instance._HoldAction.GetTimeoutCompletionPercentage();
            }
        }
        else
        {
            foreach (Image image in LaunchBars)
            {
                image.fillAmount = Mathf.Clamp01( image.fillAmount-Time.unscaledDeltaTime);
            }
        }
    }

    public void OnHoldPerformed( InputAction.CallbackContext _callback)
    {
        Debug.Log("UI_Launch: Game Starting.");
        UIManager.instance.StartGame(0);
    }
    public void DemoHandleCancel()
    {
        //demo screen press 'cancel'(B) to continue
        if (Script_UI_InputManager.instance.CancelInput)
        {
            Header.SubmitLeftBumper();
        }
    }
}
