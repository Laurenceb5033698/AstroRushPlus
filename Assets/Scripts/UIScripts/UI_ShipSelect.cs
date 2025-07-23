using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShipSelect : ScreenElement {
    //#################
    //  there are several ships to choose from
    //  this UI element lets the player pick one from a list
    //  when chosen, the nexty screen is level select
    //  on submition of ship selection, the playerprefs are updated with the choice index
    //  gameManagers have access to all player ships and will spawn the appropriate ship on loading
    //  this includes MainMenu which needs to display the different ships
    //#################
    [SerializeField] private GOEHeaderComponent Header;

    [SerializeField] private Scrollbar SelectionScrollbar;
    new public void OnEnable()
    {
        base.OnEnable();
        ////sets value and starts moving camera in spcified direction
        //MainMenu.instance.movingCamera = true;
        //MainMenu.instance.UI_SwappedToShipSelectScreen(true);
        ////movingCamera bool hits false at end of animation
    }
    new public void OnDisable()
    {
        base.OnDisable();
        ////sets value and starts moving camera in spcified direction
        //MainMenu.instance.movingCamera = true;
        //MainMenu.instance.UI_SwappedToShipSelectScreen(false);
    }

    public override void Update()
    {
        HandleSubmit();
        HandleCancel();
        //left/right nav
        HandleNavigateLeft();
        HandleNavigateRight();

        //header nav
        Header.HandleRightBumper();
        Header.HandleLeftBumper();

        //HandleInworldSelector(selector);
    }

    protected override void Cancel()
    {
        //return to Title Screen
        UIManager.instance.ScreenTransition(UIManager.Screens.TitleMenu);
    }

    protected override void SelectButton()
    {
        //use selector index to more scrollbar
        //this WILL move cards around under the mouse and could be an issue
        if (SelectableList.Length > 0)
        {
            float scrollPercentage = selector / SelectableList.Length;
            if (SelectionScrollbar)
            {
                SelectionScrollbar.value = scrollPercentage;
            }
        }
        base.SelectButton();
    }

    public void SubmitSelectedShip()
    {
        //so selecting a ship is only possible if the ship is unlocked.
        //must validate somehow vs an unlock list.
        //MEANWHILE
        //  we only have a button for first ship
        UIManager.instance.ShipSelectValue = 0;
        UIManager.instance.ScreenTransition(UIManager.Screens.ShipConfig);
    }

    ////ship select communicates with main menu to move ship tray around for ship selection
    //private void HandleInworldSelector(int _selector)
    //{   
    //    //do this after normal navigation
    //    MainMenu.instance.setTrayMoveTo(_selector);
    //    MainMenu.instance.MoveTray();
    //}

    //override public void AdvanceSelector()
    //{
    //    base.AdvanceSelector();
    //    MainMenu.instance.setTrayMoveTo(selector);
    //}

    //override public void RetreatSelector()
    //{
    //    base.RetreatSelector();
    //    MainMenu.instance.setTrayMoveTo(selector);
    //}
    ////back to Title
    //public void Button_ShipSelectReturnPressed()
    //{
    //    UIManager.instance.ReturnToMenu();
    //    MainMenu.instance.setTrayMoveTo(0);
    //}

    ////Selected ship is chosen.
    //public void Button_ShipGenericPressed(int shipVal)
    //{
    //    Func_GotoLevelSelect(shipVal);

    //}

    ////adding more ships requires adding button with data to UI scene, and adding choose ship index to mainmenu scene
    //public void Func_GotoLevelSelect(int shipval)
    //{
    //    UIManager.instance.ShipSelectValue = shipval;
    //    UIManager.instance.ScreenTransition(UIManager.Screens.ShipConfig);
    //    MainMenu.instance.setTrayMoveTo(0);
    //}

}
