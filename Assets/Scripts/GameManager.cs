using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject playerShip;
    private int currentScore = 0;
    //private int highestScore = 0;

    [SerializeField] private GameObject playerShipPref;
    [SerializeField] private GameObject pointerPref;
    [SerializeField] private CameraScript cam;
    private ScreenElement ui=null;
    private AIManager aiMngr;
    private AsteroidManager asm;
    private BoundaryManager bdrym;
    private GameObject pointer;
    
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

        //scene needs to have finished loading before we instantiate anything
        SceneLoader.Loaded += SL_OnLoadingComplete;
        UIManager.ScreenChanged += ScreenChanged;
        ui = UIManager.GetGameUiObject();
        //((UI_Game)ui).AttachGameManager();
        
        Time.timeScale = 1;
	}
    
    void Start()
    {
        
        playerShip = (GameObject)Instantiate(playerShipPref, Vector3.zero, Quaternion.identity);
        pointer = (GameObject)Instantiate(pointerPref, Vector3.zero, Quaternion.identity);
        pointer.GetComponent<Pointer>().SetFollowTarget(playerShip);
        cam.SetTarget(playerShip); // give the player ship reference to the camera


        aiMngr = GetComponent<AIManager>();
        asm = GetComponent<AsteroidManager>();
        bdrym = GetComponent<BoundaryManager>();
        aiMngr.enabled = true;
        asm.enabled = true;
        bdrym.enabled = true;
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
        
        ShipStats s = playerShip.GetComponent<ShipStats>();
        
        switch (ui.name)
        {
            //case "OptionsScreen":
            //break;
            case "GameScreen":
                //GameScreen ui controls/ updates
                Time.timeScale = 1;
                //UI_Game mUIg = ((UI_Game)ui);
                ((UI_Game)ui).UpdateGameStats(currentScore, aiMngr.GetTotalShipLeft(), aiMngr.GetWaveNumber());
                ((UI_Game)ui).UpdateShipStats(
                    s.GetBoostFuelAmount(),
                    s.GetShieldPUState(),
                    s.ShipHealth,
                    playerShip.GetComponentInChildren<Equipment>().GetAmmoCount(),
                    playerShip.GetComponent<PlayerController>().GetWeaponType());
                if (!s.IsAlive())
                    ((UI_Game)ui).Button_PausePressed(true);
                else
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))// B controller button or Escape button
                    ((UI_Game)ui).Button_PausePressed(false);
                break;
            case "OptionsScreen":
                //return from options menu
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) { ((UI_Options)ui).Button_OptionsReturnPressed(); }
                break;
            case "PauseScreen":
            default:
                //PauseScreen ui controls / updates
                Time.timeScale = 0;
                //UI_Pause mUIp = (UI_Pause)ui;
                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Y))// if A controller button or Y keyboard button
                {
                    ((UI_Pause)ui).Button_RestartPressed();
                }
                else if (s.IsAlive() && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)))// B controller button or Escape button
                    ((UI_Pause)ui).Button_ContinuePressed();
                else if (Input.GetKeyDown(KeyCode.JoystickButton3))
                    ((UI_Pause)ui).Button_QuitToMenuPressed();
                //if (s.IsAlive() && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))) // esape button or Start controller button
                //{
                //    UIManager.instance.Resume();
                //}
                break;
        }
        
        //ShipStats s = playerShip.GetComponent<ShipStats>();
        //ui.UpdateGameStats(currentScore, em.GetTotalShipLeft(), wm.GetWave());
        //ui.UpdateShipStats(s.GetBoostFuelAmount(), s.GetShieldPUState(), s.ShipHealth, s.GetNoMissiles(), playerShip.GetComponent<ShipController>().GetWeaponType());
        //if (!s.IsAlive()) ui.SetGameOverState(true);

        //Time.timeScale = (ui.GetMenuState() || ui.GetHintsState()) ? 0 : 1;

        //if (ui.GetMenuState())
        //{
        //    if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Y)) RestartGame(); // if A controller button or Y keyboard button
        //    else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) ToggleUIState(); // B controller button or Escape button
        //    else if (Input.GetKeyDown(KeyCode.JoystickButton3)) LoadMainMenu();
        //}
        //else
        //{
        //    if (ui.GetHintsState() && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)))
        //    {
        //        ui.IncrementDHIndex();
        //    }
        //    else
        //    {
        //        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) // esape button or Start controller button
        //        {
        //            ui.ToggleEscState();
        //        }
        //    }
        //}

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
