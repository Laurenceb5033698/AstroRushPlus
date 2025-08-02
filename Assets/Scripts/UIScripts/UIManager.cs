using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void UIEvent(bool active);
    public static event UIEvent MusicvolumeChanged;
    public static event UIEvent GamevolumeChanged;
    public delegate void ScreenEvent(ScreenElement screen);
    public static event ScreenEvent ScreenChanged;



    //public static event GameEvent OnExitGame;

    public enum Screens { TitleMenu, ShipSelection, ShipConfig, Shop, Loadout, Launch, Loading, GameScreen, UpgradeScreen, PauseScreen, OptionsScreen, NumScreens }

    private ScreenElement[] mScreens;
    private Screens mCurrentScreen;

    public int ShipSelectValue;

    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);

        SceneLoader.Loaded += LoadingComplete;

        ShipSelectValue = 0;



        //find canvas' in children and assign to apropriate slots
        mScreens = new ScreenElement[(int)Screens.NumScreens];
        ScreenElement[] screens = GetComponentsInChildren<ScreenElement>();
        for (int count = 0; count < screens.Length; ++count)
        {
            for (int slot = 0; slot < mScreens.Length; ++slot)
            {
                if (mScreens[slot] == null && ((Screens)slot) == screens[count].ScreenType)
                {
                    mScreens[slot] = screens[count];
                    break;
                }
            }
        }

        //all screens except first (title) make all disabled to hide them.
        for (int screen = 1; screen < mScreens.Length; ++screen)
        {
            //mScreens[screen].SetActive(false);
            mScreens[screen].gameObject.SetActive(false);
        }

    }

    private void Start()
    {
        //set screen and once other game objects have initialized
        mCurrentScreen = Screens.TitleMenu;
        //mScreens[(int)mCurrentScreen].OnScreenOpen();
    }

    
    private void Update()
    {
        
    }




    public void LoadingComplete()
    {
        mScreens[(int)Screens.Loading].gameObject.SetActive(false);
        mScreens[(int)mCurrentScreen].gameObject.SetActive(true);
    }

    public static UI_Game GetGameUiObject()
    {
        ScreenElement ui = null;
        ui = UIManager.instance.GetCurrentElement();
        if( ui is UI_Game)
            return ui as UI_Game;
        else
        {
            return null;
        }
    }
    public ScreenElement GetCurrentElement()
    {
        return mScreens[(int)mCurrentScreen];
    }
    private void SceneTransitionTo(Screens screen)
    {//ONLY used when loading new scenes
        
        mScreens[(int)mCurrentScreen].gameObject.SetActive(false);
        mScreens[(int)Screens.Loading].gameObject.SetActive(true);
        mCurrentScreen = screen;
    }
    private void TransitionTo(Screens screen)
    {//used when a UI screen is swapped in-scene
        mScreens[(int)mCurrentScreen].gameObject.SetActive(false);
        mScreens[(int)screen].gameObject.SetActive(true);
        mCurrentScreen = screen;
    }
    public void StartGame(int lev)
    {
        if (SceneLoader.LoadLevel(lev))
        {
            SceneTransitionTo(Screens.GameScreen);
            OnScreenChanged();
        }
    }

    public void EndLevel()
    {
        SceneLoader.LoadTitleScene();
        SceneTransitionTo(Screens.TitleMenu);
        OnScreenChanged();

    }
    public void RestartLevel()
    {
        SceneLoader.RestartCurrentLevel();
        TransitionTo(Screens.GameScreen);
        OnScreenChanged();
    }


    //returns currently active screen
    private void OnScreenChanged(Screens targetScreen = Screens.NumScreens)
    {
        if (ScreenChanged != null)
        {
            if (targetScreen != Screens.NumScreens)
                ScreenChanged(mScreens[(int)targetScreen]);
            else
                ScreenChanged(mScreens[(int)mCurrentScreen]);
            //mScreens[(int)mCurrentScreen].OnScreenOpen();
        }
    }
    public void Pause(bool isPlayerDead)
    {
        TransitionTo(Screens.PauseScreen);
        ((UI_Pause)mScreens[(int)mCurrentScreen]).setMessage(isPlayerDead);
        OnScreenChanged();
    }
    public void Resume()
    {
        ScreenTransition(Screens.GameScreen);
    }

    public void MenuQuit()
    {
        Application.Quit();
    }
    public void fullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void OptionsButton()
    {
        ScreenTransition(Screens.OptionsScreen);
    }
    public void ReturnToMenu()
    {
        ScreenTransition(Screens.TitleMenu);
    } 
    //public void LevelSelectButton()
    //{
    //    ScreenTransition(Screens.LevelSelect);
    //}
    //public void ShipSelectionButton()
    //{
    //    ScreenTransition(Screens.ShipSelectionScreen);
    //}

    public void UpgradeScreen()
    {
        //new upgrade selection everytime upgrade screen is opened.
        ScreenTransition(Screens.UpgradeScreen);
        (GetCurrentElement() as UI_Upgrade).GetRandomCardsFromPool();
    }

    public void NavigateMenuHeader(int _HeaderScreenIndex)
    {
        //transition to header at given index
    }


    public void ScreenTransition(Screens _target)
    {
        StartCoroutine(TransitionInternal(_target));
    }

    private IEnumerator TransitionInternal(Screens _target)
    {
        //wait one frame to change screen. prevents inputs from being chucked through multiple ui screens in one frame
        yield return null;
        TransitionTo(_target);
        OnScreenChanged();
    }

    //Static Functions for script-UI interaction from other scenes
    public static ScreenElement GetShipUiObject()
    {
        ScreenElement ui = null;
        ui = UIManager.instance.GetShipSelectElement();
        return ui;
    }
    public ScreenElement GetShipSelectElement()
    {
        return mScreens[(int)Screens.ShipSelection];
    }

    //Events - volume changed
    public void musicVolumeOnValueChanged()
    {
        if (MusicvolumeChanged != null)
            MusicvolumeChanged(true);
    }
    public void gameVolumeOnValueChanged()
    {
        if (GamevolumeChanged != null)
            GamevolumeChanged(true);
    }

    //event - gamescene loaded: set object data for uxml hud
    public void setPlayershipObject(GameObject playerShip)
    {
        //get playerhud ui component and set playership object
        //mScreens[(int)Screens.GameScreen].gameObject.GetComponent<GameUIPlayerHealth>().onGameSceneLoaded(playerShip);
    }

    //new object selected, pass to screens
    public void passNewSelectedObject(GameObject _newSelected)
    {
        ScreenElement currentScreen = GetCurrentElement();
        if(currentScreen !=null)
        {
            currentScreen.SetNewSelectedObject(_newSelected);
        }
    }

}
