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

    public enum Screens { TitleMenu, LevelSelect, LoadingScreen, GameScreen, PauseScreen, OptionsScreen, NumScreens }

    private ScreenElement[] mScreens;
    private Screens mCurrentScreen;
    

    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);

        SceneLoader.Loaded += LoadingComplete;

        
        

        //find canvas' in children and assign to apropriate slots
        mScreens = new ScreenElement[(int)Screens.NumScreens];
        ScreenElement[] screens = GetComponentsInChildren<ScreenElement>();
        for (int count = 0; count < screens.Length; ++count)
        {
            for (int slot = 0; slot < mScreens.Length; ++slot)
            {
                if (mScreens[slot] == null && ((Screens)slot).ToString() == screens[count].name)
                {
                    mScreens[slot] = screens[count];
                    break;
                }
            }
        }

        for (int screen = 1; screen < mScreens.Length; ++screen)
        {
            //mScreens[screen].SetActive(false);
            mScreens[screen].enabled = false;
        }

        mCurrentScreen = Screens.TitleMenu;
    }

    public void LoadingComplete()
    {
        mScreens[(int)Screens.LoadingScreen].enabled = false;
        mScreens[(int)mCurrentScreen].enabled = true;
    }

    public static ScreenElement GetGameUiObject()
    {
        ScreenElement ui = null;
        ui = UIManager.instance.GetCurrentElement();
        return ui;
    }
    public ScreenElement GetCurrentElement()
    {
        return mScreens[(int)mCurrentScreen];
    }
    private void SceneTransitionTo(Screens screen)
    {//ONLY used when loading new scenes
        
        mScreens[(int)mCurrentScreen].enabled = false;
        mScreens[(int)Screens.LoadingScreen].enabled = true;
        mCurrentScreen = screen;
    }
    private void TransitionTo(Screens screen)
    {//used when a UI screen is swapped in-scene
        mScreens[(int)mCurrentScreen].enabled = false;
        mScreens[(int)screen].enabled = true;
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
        TransitionTo(Screens.GameScreen);
        OnScreenChanged();

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
        TransitionTo(Screens.OptionsScreen);
        OnScreenChanged();
    }
    public void ReturnToMenu()
    {
        TransitionTo(Screens.TitleMenu);
        OnScreenChanged();
    } 
    public void LevelSelectButton()
    {
        TransitionTo(Screens.LevelSelect);
        OnScreenChanged();
    }
    //Static Functions for script-UI interaction from other scenes
  
    
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


}
