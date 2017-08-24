using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : ScreenElement
{
    //private MainMenu mm = null;

    //public void AttachMainMenuManager() { mm = MainMenu.instance; }
    //public void RemoveMainMenuManager() { mm = null; }

    public void Button_StartGamePressed()
    {
        UIManager.instance.StartGame();
    }
    public void Button_OptionsPressed()
    {
        UIManager.instance.OptionsButton();
        MainMenu.instance.UI_OnOptionsCall(true);
    }
    public void Button_MenuQuitPressed()
    {
        UIManager.instance.MenuQuit();
    }
}