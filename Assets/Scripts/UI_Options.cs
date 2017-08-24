using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Options : ScreenElement {

    [SerializeField] private Slider MusicvolumeSlider;
    [SerializeField] private Slider GamevolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
            MusicvolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        else
        {
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
    }
    public void Button_OptionsReturnPressed()
    {//WARNING: returning to previous screen requires some changes
        if (GameManager.instance != null)//if there is a game active
            UIManager.instance.Pause(false);//return to pause menu
        else
            if (MainMenu.instance != null)//if there is a main menu active
            UIManager.instance.ReturnToMenu();//return to menu
    }
    

    public void musicVolumeOnValueChanged()
    {
        PlayerPrefs.SetFloat("musicVolume", MusicvolumeSlider.value);
        PlayerPrefs.Save();

        UIManager.instance.musicVolumeOnValueChanged();
    }
    public void gameVolumeOnValueChanged()
    {
        PlayerPrefs.SetFloat("gameVolume", GamevolumeSlider.value);
        PlayerPrefs.Save();
        AudioListener.volume = PlayerPrefs.GetFloat("gameVolume");

        UIManager.instance.gameVolumeOnValueChanged();

    }
}
