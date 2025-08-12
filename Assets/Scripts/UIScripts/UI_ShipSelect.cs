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
        UIManager.instance.ScreenTransition(UIManager.Screens.ShipConfig);
    }

}
