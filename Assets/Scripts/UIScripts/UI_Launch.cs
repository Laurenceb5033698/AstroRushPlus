using UnityEngine;

public class UI_Launch : ScreenElement
{
    [SerializeField] private GOEHeaderComponent Header;

    //two buttons, submit and cancel
    //submit starts game
    //cancel goes back a step
    public override void Update()
    {
        HandleSubmit();
        HandleCancel();
        //header nav
        Header.HandleRightBumper();
        Header.HandleLeftBumper();

    }

}
