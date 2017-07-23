using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void UIEvent(bool active);
    public static event UIEvent Options;
    public static event UIEvent MusicvolumeChanged;
    public static event UIEvent GamevolumeChanged;
    public delegate void ScreenEvent(ScreenElement screen);
    public static event ScreenEvent ScreenChanged;



    //public static event GameEvent OnExitGame;

    public enum Screens { TitleMenu, LoadingScreen, GameScreen, PauseScreen, NumScreens }

    private ScreenElement[] mScreens;
    private Screens mCurrentScreen;

    //optionspanel
    [SerializeField] private GameObject optionsPanel;
    private bool optionPanelActive =false;
    [SerializeField] private Slider MusicvolumeSlider;
    [SerializeField] private Slider GamevolumeSlider;



    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);

        SceneLoader.Loaded += LoadingComplete;

        if (PlayerPrefs.HasKey("musicVolume"))
            MusicvolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        else{
            MusicvolumeSlider.value = 1;
            PlayerPrefs.SetFloat("musicVolume", MusicvolumeSlider.value);
        }
        if (PlayerPrefs.HasKey("gameVolume"))
            GamevolumeSlider.value = PlayerPrefs.GetFloat("gameVolume");
        else
        {
            GamevolumeSlider.value = 0.5f;
            PlayerPrefs.SetFloat("gameVolume", GamevolumeSlider.value);
        }
        PlayerPrefs.Save();

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
        optionsPanel.SetActive(false);

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
    public void StartGame()
    {
        SceneLoader.LoadLevel(0);
        SceneTransitionTo(Screens.GameScreen);
    }

    public void EndLevel()
    {
        SceneLoader.LoadTitleScene();

        SceneTransitionTo(Screens.TitleMenu);
        OnScreenChanged(Screens.GameScreen);//resets gamemanager reference

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

    //Static Functions for script-UI interaction from other scenes
    public static void StartGamePressed(){
        UIManager.instance.StartGame();
    }
    public static void MenuQuitPressed(){
        UIManager.instance.MenuQuit();
    }
    public static void PressedOptionsButton(){
        UIManager.instance.OptionsButton();
    }
    //Event - options panel
    public void OptionsButton()
    {//as long as the event "Options" has subscribers
        if (Options != null)
        {//Trigger the Event for all subscribers
            optionPanelActive = !optionPanelActive;
            Options(optionPanelActive);
            optionsPanel.SetActive(optionPanelActive);
        }//!!! NOTE - MAKE SURE TO UNSUBSCRIBE BEFORE UNLOADING SCENE !!!

    }
    //Event - volume changed
    public void musicVolumeOnValueChanged()
    {
        PlayerPrefs.SetFloat("musicVolume", MusicvolumeSlider.value);
        if (MusicvolumeChanged != null)
            MusicvolumeChanged(true);
        PlayerPrefs.Save();
    }
    public void gameVolumeOnValueChanged()
    {
        PlayerPrefs.SetFloat("gameVolume", GamevolumeSlider.value);
        if (GamevolumeChanged != null)
            GamevolumeChanged(true);
        PlayerPrefs.Save();
    }
}
