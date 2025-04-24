using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShipSelect : ScreenElement {
    //#################
    //  there are several ships to choose from
    //  this UI element lets the player pick one from a list
    //  when chosen, the nexty screen is level select
    //  on submition of ship selection, the playerprefs are updated with the choice index
    //  gameManagers have access to all player ships and will spawn the appropriate ship on loading
    //  this includes MainMenu which needs to display the different ships
    //#################

    new public void OnEnable()
    {
        base.OnEnable();
        //sets value and starts moving camera in spcified direction
        MainMenu.instance.movingCamera = true;
        MainMenu.instance.UI_SwappedToShipSelectScreen(true);
        //movingCamera bool hits false at end of animation
    }
    new public void OnDisable()
    {
        base.OnDisable();
        //sets value and starts moving camera in spcified direction
        MainMenu.instance.movingCamera = true;
        MainMenu.instance.UI_SwappedToShipSelectScreen(false);
    }

    public override void Update()
    {
        HandleSubmit();
        HandleCancel();
        //left/right nav
        HandleNavigateLeft();
        HandleNavigateRight();

        HandleInworldSelector(selector);
    }

    //ship select communicates with main menu to move ship tray around for ship selection
    private void HandleInworldSelector(int _selector)
    {   
        //do this after normal navigation
        MainMenu.instance.setTrayMoveTo(_selector);
    }

    override public void AdvanceSelector()
    {
        base.AdvanceSelector();
        MainMenu.instance.setTrayMoveTo(selector);
    }

    override public void RetreatSelector()
    {
        base.RetreatSelector();
        MainMenu.instance.setTrayMoveTo(selector);
    }
    //back to Title
    public void Button_ShipSelectReturnPressed()
    {
        UIManager.instance.ReturnToMenu();
    }
    
    public void Button_ShipGenericPressed(int shipVal)
    {
        Func_GotoLevelSelect(shipVal);

    }
    
    //adding more ships requires adding button with data to UI scene, and adding choose ship index to mainmenu scene
    public void Func_GotoLevelSelect(int shipval)
    {
        UIManager.instance.ShipSelectValue = shipval;
        UIManager.instance.LevelSelectButton();
    }

}
