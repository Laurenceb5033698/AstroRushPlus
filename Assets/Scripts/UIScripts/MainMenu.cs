using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Scene manager for main menu screens.
/// should be used for any background animations while in menus.
/// </summary>
public class MainMenu : MonoBehaviour 
{
    [SerializeField] private AudioSource music;
    public Inputs GlobalInputs;

    public static MainMenu instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        GlobalInputs = GetComponent<Inputs>();

        if (PlayerPrefs.HasKey("musicVolume"))
            music.volume = PlayerPrefs.GetFloat("musicVolume");
        else
            PlayerPrefs.SetFloat("musicVolume", 1);
        if (PlayerPrefs.HasKey("gameVolume"))
            AudioListener.volume = PlayerPrefs.GetFloat("gameVolume");
        else
            PlayerPrefs.SetFloat("gameVolume", 0.5f);
        PlayerPrefs.Save();

        Screen.fullScreen = true;
    }
        
    private void OnDestroy()
    {
    }

    void Start () 
    {
    }

    private void OnEnable()
    {
        UIManager.MusicvolumeChanged += UI_OnVolumeChanged;
    }

    private void OnDisable()
    { 
        UIManager.MusicvolumeChanged -= UI_OnVolumeChanged;
    }

    void Update()
    {
    }

    public void UI_OnVolumeChanged(bool temp)
    {
        music.volume = PlayerPrefs.GetFloat("musicVolume");
    }


}
