using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void UIEvent(bool active);
    public static event UIEvent Options;
    public static event UIEvent MusicvolumeChanged;


    //public static event GameEvent OnExitGame;

    public enum Screens { TitleMenu, GameScreen, PauseScreen, NumScreens }

    private Canvas[] mScreens;
    private Screens mCurrentScreen;

    //optionspanel
    [SerializeField] private GameObject optionsPanel;
    private bool optionPanelActive =false;
    [SerializeField] private Slider volumeSlider;


    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("musicVolume"))
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        else{
            volumeSlider.value = 1;
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        }

        //find canvas' in children and assign to apropriate slots
        mScreens = new Canvas[(int)Screens.NumScreens];
        Canvas[] screens = GetComponentsInChildren<Canvas>();
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
            mScreens[screen].enabled = false;
        }
        optionsPanel.SetActive(false);

        mCurrentScreen = Screens.TitleMenu;
    }
    public static UI GetGameUiObject()
    {
        UI ui = null;
        ui = UIManager.instance.GetComponentInChildren<UI>();
        return ui;
    }
    public void StartGame()
    {
        SceneLoader.LoadLevel(0);
        TransitionTo(Screens.GameScreen);
    }

    public void EndLevel()
    {
        SceneLoader.LoadTitleScene();

        TransitionTo(Screens.TitleMenu);
    }

    private void TransitionTo(Screens screen)
    {
        mScreens[(int)mCurrentScreen].enabled = false;
        mScreens[(int)screen].enabled = true;
        mCurrentScreen = screen;
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
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        if (MusicvolumeChanged != null)
            MusicvolumeChanged(true);
        PlayerPrefs.Save();
    }
}
