using UnityEngine;

public class GOEHeaderComponent : MonoBehaviour
{
    //reusable component for header bar on menu ui.
    //must be initialised with which screen its currently on
    //  this is so rb lb inputs will navigate to correct screen

    //but once created, this wont change, so does not need to be set dynamically. can be set in inspector

    //HeaderTabIndex used to select correct UIScreen from set HeaderScreens list
    private enum HeaderTabIndex { SHIP, CONFIG, SHOP, LOADOUT, LAUNCH, NUM };
    [SerializeField] private HeaderTabIndex m_tabIndex = HeaderTabIndex.NUM;

    //mapped 0-n screen target to header button
    private UIManager.Screens[] m_HeaderScreens = {
        UIManager.Screens.ShipSelection,
        UIManager.Screens.ShipConfig,
        UIManager.Screens.Shop,
        UIManager.Screens.Loadout,
        UIManager.Screens.Launch
    };

    //has button input for every entry on header.
    //however rb lb only goes next/prev.
    //if on ends of header, rb/lb donot do anything.

    public void HandleRightBumper()
    {
        if (Script_UI_InputManager.instance.RightBumperInput)
        {
            SubmitRightBumper();
        }
    }
    public void HandleLeftBumper()
    {
        if (Script_UI_InputManager.instance.LeftBumperInput)
        {
            SubmitLeftBumper();
        }
    }

    public void SubmitRightBumper()
    {
        if (m_tabIndex < HeaderTabIndex.LAUNCH)
        {
            UIManager.Screens targetScreen = m_HeaderScreens[(int)m_tabIndex + 1];
            UIManager.instance.ScreenTransition(targetScreen);
        }
    }

    public void SubmitLeftBumper() 
    {
        if (m_tabIndex > HeaderTabIndex.SHIP)
        {
            UIManager.Screens targetScreen = m_HeaderScreens[(int)m_tabIndex - 1];
            UIManager.instance.ScreenTransition(targetScreen);
        }
    }

    //callback for clickable buttons on header
    public void ButtonHeaderScreen(int _targetScreen)
    {
        if(_targetScreen >= 0 && _targetScreen < m_HeaderScreens.Length)
        {
            UIManager.Screens targetScreen = m_HeaderScreens[_targetScreen];
            UIManager.instance.ScreenTransition(targetScreen);
        }
    }


}
