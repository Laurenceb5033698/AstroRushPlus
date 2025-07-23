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

    public override void Update()
    {
        HandleSubmit();
        HandleCancel();
        //up/down nav
        HandleNavigateUp();
        HandleNavigateDown();
    }

    protected override void Cancel()
    {
        Button_OptionsReturnPressed();
    }

    public void Button_OptionsReturnPressed()
    {//WARNING: returning to previous screen requires some changes
        if (GameManager.instance != null)//if there is a game active
            UIManager.instance.Pause(false);//return to pause menu
        else
            if (MainMenu.instance != null)
            {//if there is a main menu active
                //MainMenu.instance.UI_OnOptionsCall(false);
                UIManager.instance.ReturnToMenu();//return to menu
            }
    }

    //public void ProcessInputs()
    //{
    //    if (controls.LAnalogueYDown || (controls.DpadYPressed && controls.DpadDown)) AdvanceSelector();
    //    if (controls.LAnalogueYUp || (controls.DpadYPressed && controls.DpadUp)) RetreatSelector();

    //    if (SelectableList[selector].GetComponent<Slider>() != null)
    //    {//if obj is a slider: Dont do .submit
    //        //do lefty-righty on slider bar
    //        if (controls.LAnalogueXLeft || (controls.DpadXPressed && controls.DpadLeft))
    //            IncreaseSliderValue(SelectableList[selector].GetComponent<Slider>()); //decrease slider value
    //        if (controls.LAnalogueXRight || (controls.DpadXPressed && controls.DpadRight))
    //            DecreaseSliderValue(SelectableList[selector].GetComponent<Slider>()); //increase slider value
    //    }
    //    else
    //    {
    //        if (SelectableList[selector].GetComponent<Toggle>() != null)
    //        {   //toggle button
    //            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))// if A controller button or Y keyboard button
    //                SelectableList[selector].GetComponent<Toggle>().isOn = !SelectableList[selector].GetComponent<Toggle>().isOn;
    //        }
    //    }
    //    //test if object is a button slider or toggle
    //    //TestSelectable();
    //}

    private void IncreaseSliderValue(Slider sl)
    {   //increase + 5% of max
        float max = sl.maxValue;
        float amount = max / 20;
        sl.value = ((sl.value + amount) >= max) ? max : sl.value + amount;
    }
    private void DecreaseSliderValue(Slider sl)
    {   //decrase - 5% of max
        float max = sl.maxValue;
        float min = sl.minValue;
        float amount = max / 20;
        sl.value = ((sl.value - amount) <= min) ? min : sl.value - amount;

    }
    public void TestSelectable()
    {
        Selectable ButtonItem = null;
        ButtonItem = SelectableList[selector].GetComponent<Slider>();
        if ( ButtonItem != null)
        {   //true if object is a slider type selectable
            //deal with slider inputs
            //ButtonItem.Select();
            //((Slider)ButtonItem).value

        }
        else
        {   //else could be a toggle button
            ButtonItem = SelectableList[selector].GetComponent<Toggle>();
            if( ButtonItem != null)
            {   //item is a Toggle;
                //deal with toggle inputs
                //ButtonItem.Select();
                //((Toggle)ButtonItem).on

            }

        }

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
