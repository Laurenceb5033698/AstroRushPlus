using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : ScreenElement
{
    //private MainMenu mm = null;

    //public void AttachMainMenuManager() { mm = MainMenu.instance; }
    //public void RemoveMainMenuManager() { mm = null; }

    // nav controls/player inputs each frame
    override public void Update()
    {
        HandleSubmit();
        HandleCancel();
        //menu set out in vertical style.
        HandleNavigateUp();
        HandleNavigateDown();
    }

    public void Button_StartGamePressed()
    {
        UIManager.instance.ScreenTransition(UIManager.Screens.ShipSelection);
    }
    public void Button_OptionsPressed()
    {
        UIManager.instance.OptionsButton();
        //MainMenu.instance.UI_OnOptionsCall(true);
    }
    public void Button_MenuQuitPressed()
    {
        UIManager.instance.MenuQuit();
    }
}