using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseShip : MonoBehaviour {

    [SerializeField] private int shipIndex;

    private void Start()
    {

    }

    private void OnMouseDown()
    {
        CallScreenChange();
    }
    
    public void CallScreenChange()
    {
        //get reference to ui element and pass data to UI_shipselect. this changes screen
        //UI_ShipSelect shipScreen = (UI_ShipSelect)UIManager.GetShipUiObject();
        //if (shipScreen.enabled)
        //    shipScreen.Button_ShipGenericPressed(shipIndex);

    }
}
