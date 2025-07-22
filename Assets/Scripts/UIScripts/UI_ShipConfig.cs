using UnityEngine;

public class UI_ShipConfig : ScreenElement
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
