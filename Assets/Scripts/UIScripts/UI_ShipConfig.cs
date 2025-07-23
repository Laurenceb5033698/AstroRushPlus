using UnityEngine;

public class UI_ShipConfig : ScreenElement
{
    [SerializeField] private GOEHeaderComponent Header;

    public override void Update()
    {
        //HandleSubmit();
        DemoHandleSubmit();
        //HandleCancel();
        DemoHandleCancel();
        //header nav
        Header.HandleRightBumper();
        Header.HandleLeftBumper();
    }

    public void DemoHandleSubmit()
    {
        //demo screen press 'submit'(A) to continue
        if (Script_UI_InputManager.instance.SubmitInput)
        {
            Header.SubmitRightBumper();
        }
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
