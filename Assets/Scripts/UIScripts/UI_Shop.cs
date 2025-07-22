using UnityEngine;

public class UI_Shop : ScreenElement
{
    [SerializeField] private GOEHeaderComponent Header;

    public override void Update()
    {
        HandleSubmit();
        HandleCancel();
        //header nav
        Header.HandleRightBumper();
        Header.HandleLeftBumper();
    }
}
