using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject playerShip;
    public int currentScore = 0;
    //private int highestScore = 0;

    [SerializeField] private GameObject[] playerShipPref = new GameObject[4];
    [SerializeField] private GameObject pointerPref;
    [SerializeField] private CameraScript cam;
    private ScreenElement ui=null;
    private AIManager aiMngr;
    private AsteroidManager asm;
    private GameObject pointer;
    public Inputs GlobalInputs;
    
    [SerializeField] private AudioSource music;

    public static GameManager instance = null;
    // Use this for initialization
    void Awake () 
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        music.volume = 0.2f * PlayerPrefs.GetFloat("musicVolume");
        AudioListener.volume = PlayerPrefs.GetFloat("gameVolume");
        GlobalInputs = GetComponent<Inputs>();

        //scene needs to have finished loading before we instantiate anything
        SceneLoader.Loaded += SL_OnLoadingComplete;
        UIManager.ScreenChanged += ScreenChanged;
        ui = UIManager.GetGameUiObject();
        //((UI_Game)ui).AttachGameManager();
        
        Time.timeScale = 1;
	}
    
    void Start()
    {
        //playerfactory type stuff
        playerShip = (GameObject)Instantiate(playerShipPref[UIManager.instance.ShipSelectValue], Vector3.zero, Quaternion.identity);
        playerShip.GetComponent<PlayerController>().SetInputs(GlobalInputs);
        //restarting should get correct object to upgrade module.
        UpgradePoolManager.instance.AssignPlayerStatsToUpgradeManager(playerShip);

        pointer = (GameObject)Instantiate(pointerPref, Vector3.zero, Quaternion.identity);
        pointer.GetComponent<Pointer>().SetFollowTarget(playerShip);
        cam.SetTarget(playerShip); // give the player ship reference to the camera


        aiMngr = GetComponent<AIManager>();
        asm = GetComponent<AsteroidManager>();

        aiMngr.enabled = true;
        asm.enabled = true;

        //set instantiated objects for hud
        //UIManager.instance.setPlayershipObject(playerShip);
    }
    private void OnEnable()
    {
        UIManager.MusicvolumeChanged += UI_OnVolumeChanged;
    }
    private void OnDisable()
    {
        UIManager.MusicvolumeChanged -= UI_OnVolumeChanged;

    }
    private void SL_OnLoadingComplete()
    {//ensures instances are created in the right scene
        this.enabled = true;
    }
    void OnDestroy(){//clean events
        SceneLoader.Loaded -= SL_OnLoadingComplete;
        UIManager.ScreenChanged -= ScreenChanged;

        //((UI_Game)ui).RemoveGameManager();
    }
	// Update is called once per frame
	void Update () 
    {
        Vector3 ClosestEnemy = aiMngr.GetClosestShipPos(playerShip.transform.position);
        pointer.GetComponent<Pointer>().SetNewTarget(ClosestEnemy);
        
        Stats s = playerShip.GetComponent<Stats>();

        switch (ui.name)
        {
            //case "OptionsScreen":
            //break;
            case "GameScreen":
                //GameScreen ui controls/ updates
                Time.timeScale = 1;
                //UI_Game mUIg = ((UI_Game)ui);
                //((UI_Game)ui).UpdateGameStats(currentScore, aiMngr.GetTotalShipLeft(), aiMngr.GetWaveNumber());
                ((UI_Game)ui).UpdateShipStats(s);
                if (!s.IsAlive())
                    ((UI_Game)ui).Button_PausePressed(true);
                else
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))// B controller button or Escape button
                //    ((UI_Game)ui).Button_PausePressed(false);
                if (aiMngr.EndOfWave == true)
                {
                    UIManager.instance.UpgradeScreen();
                    aiMngr.NewWave();
                }
                break;
            case "UpgradeScreen":
                //do upgrades
                Time.timeScale = 0;

                //((UI_Upgrade)ui).ProcessInputs();
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) {  ((UI_Upgrade)ui).Button_ConfirmUpgrades();}
                //if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))// if A controller button or Y keyboard button
                //{
                //    ui.SubmitSelection();
                //}
                break;
            case "OptionsScreen":
                //return from options menu
                //((UI_Options)ui).ProcessInputs();
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) { ((UI_Options)ui).Button_OptionsReturnPressed(); }
                break;
            case "PauseScreen":
            default:
                //PauseScreen ui controls / updates
                Time.timeScale = 0;
                //UI_Pause mUIp = (UI_Pause)ui;
                //if (GlobalInputs.LAnalogueYDown || (GlobalInputs.DpadYPressed && GlobalInputs.DpadDown)) ui.AdvanceSelector();
                //if (GlobalInputs.LAnalogueYUp || (GlobalInputs.DpadYPressed && GlobalInputs.DpadUp)) ui.RetreatSelector();

                //if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))// if A controller button or Y keyboard button
                //{
                //    ui.SubmitSelection();
                //}
                //else if (s.IsAlive() && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)))// B controller button or Escape button
                //    ((UI_Pause)ui).Button_ContinuePressed();
                
                break;
        }
        

    }
    public void UI_OnVolumeChanged(bool temp)
    {
        music.volume = 0.2f * PlayerPrefs.GetFloat("musicVolume");

    }
    public void AddScore(int s)
    {
        currentScore += s;
    }
    public GameObject GetShipRef()
    {
        return playerShip;
    }
    public void ScreenChanged(ScreenElement newScreen)
    {
        ui = newScreen;
    }


}
