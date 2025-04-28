using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    
    [SerializeField] private GameObject planet;
    [SerializeField] private AudioSource music;
    private ScreenElement ui = null;
    public Inputs GlobalInputs;

    // option menu
    private bool mOptionPanelActive = false;
    private bool shipMoved = false;
    private bool MoveShipBack = false;
    private Vector3 shipMovePos = new Vector3(-28,-15,-30);
    private Vector3 shipMoveBackPos = new Vector3(53,17,35);
    //[SerializeField] private Toggle fullScreenToggleGO;

    // ship
    [SerializeField] private GameObject ship;
    private Vector3 shipStartPos;
    private float timeToChangeDir;
    private Vector3 targetDir;
    private Vector3 targetRot;

    //  selectShip
    public GameObject selectTray;
    private Vector3 trayStartPos;
    public bool movingCamera = false;
    private int trayindex = 0;

    [SerializeField] private Transform DefaultCameraPos;
    [SerializeField] private Transform SelectShipCameraPos;


    //
    public static MainMenu instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        GlobalInputs = GetComponent<Inputs>();

        UIManager.ScreenChanged += ScreenChanged;
        ui = UIManager.GetGameUiObject();
        //((UI_MainMenu)ui).AttachMainMenuManager();

        if (PlayerPrefs.HasKey("musicVolume"))
            music.volume = PlayerPrefs.GetFloat("musicVolume");
        else
            PlayerPrefs.SetFloat("musicVolume", 1);
        if (PlayerPrefs.HasKey("gameVolume"))
            AudioListener.volume = PlayerPrefs.GetFloat("gameVolume");
        else
            PlayerPrefs.SetFloat("gameVolume", 0.5f);

        PlayerPrefs.SetInt("showHints", 1);
        PlayerPrefs.Save();

        Screen.fullScreen = true;
        //fullScreenToggleGO.isOn = true;

        
    }
    private void OnDestroy()
    {
        UIManager.ScreenChanged -= ScreenChanged;
        //UIManager.instance.ReturnToMenu();
        //((UI_MainMenu)ui).RemoveMainMenuManager();

    }
    // Use this for initialization
    void Start () {
        shipStartPos = ship.transform.position;
        trayStartPos = selectTray.transform.position;
        targetRot = ship.transform.eulerAngles;
        Time.timeScale = 1;
        timeToChangeDir = Time.time + 3f;
        float temp = 3f;
        targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
    }
    private void OnEnable(){
        //UIManager.Options += UI_OnOptionsCall;//subscribe to options toggle event
        UIManager.MusicvolumeChanged += UI_OnVolumeChanged;
    }
    private void OnDisable(){
        //UIManager.Options -= UI_OnOptionsCall;//unsubscribe
        UIManager.MusicvolumeChanged -= UI_OnVolumeChanged;
    }
    // Update is called once per frame
    void Update()
    {
        switch (ui.name)
        {
            case "OptionsScreen":
                //return from options menu
                //((UI_Options)ui).ProcessInputs();
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) { ((UI_Options)ui).Button_OptionsReturnPressed(); UI_OnOptionsCall(false); }
                break;
            case "ShipSelectionScreen":
                //on enter shipSelect: moves camera to ShipSelCamPos
                //left/right change ship && change shipTray
                //if (Input.GetKeyDown(KeyCode.D) || GlobalInputs.ScrollUp || GlobalInputs.LAnalogueXLeft || (GlobalInputs.DpadXPressed && GlobalInputs.DpadLeft)) ((UI_ShipSelect)ui).AdvanceSelector();//override moves ship tray +1
                //if (Input.GetKeyDown(KeyCode.A) ||GlobalInputs.ScrollDown || GlobalInputs.LAnalogueXRight || (GlobalInputs.DpadXPressed && GlobalInputs.DpadRight)) ((UI_ShipSelect)ui).RetreatSelector();//override mores ship tray -1
                ////A/enter submit && goto levelSelect
                //if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return))// if A controller button or Y keyboard button
                //{
                //    ui.SubmitSelection();
                //    trayindex = 0;
                //}
                ////back button
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
                //{
                //    ((UI_ShipSelect)ui).Button_ShipSelectReturnPressed();
                //    trayindex = 0;
                //}
                //MoveTray();


                break;
            case "LevelSelect":
                //if (GlobalInputs.LAnalogueYDown || (GlobalInputs.DpadYPressed && GlobalInputs.DpadDown )) ui.AdvanceSelector();
                //if ( GlobalInputs.LAnalogueYUp || (GlobalInputs.DpadYPressed && GlobalInputs.DpadUp)) ui.RetreatSelector();

                //if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return))// if A controller button or Y keyboard button
                //{
                //    ui.SubmitSelection();
                //}
                ////back button
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) ((UI_LevelSelect)ui).Button_LevelSelectReturnPressed();
                //else if (Input.GetKeyDown(KeyCode.JoystickButton0)) ((UI_LevelSelect)ui).Button_AlphaLevelPressed(); //A button
                //else if (Input.GetKeyDown(KeyCode.JoystickButton2)) ((UI_LevelSelect)ui).Button_BetaLevelPressed(); //X button
                //else if (Input.GetKeyDown(KeyCode.JoystickButton3)) ((UI_LevelSelect)ui).Button_GammaLevelPressed(); //Y button
                break;
            case "TitleScreen":
            default:
                //if (GlobalInputs.LAnalogueYDown || (GlobalInputs.DpadYPressed && GlobalInputs.DpadDown)) ui.AdvanceSelector();
                //if (GlobalInputs.LAnalogueYUp || (GlobalInputs.DpadYPressed && GlobalInputs.DpadUp)) ui.RetreatSelector();

                //if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return))// if A controller button or Y keyboard button
                //{
                //    ui.SubmitSelection();
                //}

                //if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) ((UI_MainMenu)ui).Button_StartGamePressed();  // if A controller button or Enter keyboard button
                //else if (Input.GetKeyDown(KeyCode.JoystickButton3)) ((UI_MainMenu)ui).Button_OptionsPressed(); 
                //else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) ((UI_MainMenu)ui).Button_MenuQuitPressed(); // B controller button or Escape button
                break;
        }
        if (!mOptionPanelActive)
        {
            if (Vector3.Distance(ship.transform.position, shipStartPos) > 5)
            {
                MoveShipBack = true;
            }
            else
            {
                shipMoved = false;
                MoveShip();
            }  
        }
        else if (!shipMoved)
        {
            ship.transform.position = Vector3.MoveTowards(ship.transform.position, shipMovePos, 20 * Time.deltaTime);

            if (Vector3.Distance(ship.transform.position, shipStartPos) > 40)
            {
                shipMoved = true;
                ship.transform.position = shipMoveBackPos;
            }
        }

        if (MoveShipBack)
        {
            ship.transform.position = Vector3.MoveTowards(ship.transform.position,shipStartPos,20 * Time.deltaTime);
            if (Vector3.Distance(ship.transform.position, shipStartPos) < 0.5f)
            {
                ship.transform.position = shipStartPos;
                MoveShipBack = false;
            }
        }

        RotatePlanet();

        if (movingCamera)//moving camera is set from UI_ShipSelect screen
        {   //resets bool to false when anim finished
            if (!Camera.main.GetComponent<Animation>().isPlaying)
                movingCamera = false;
        }


    }

    private void RotatePlanet()
    {
        planet.transform.Rotate(Vector3.up * -1*Time.deltaTime);
    }

    private void MoveShip()
    {
        if (Time.time > timeToChangeDir)
        {
            timeToChangeDir = Time.time + 3f;
            float temp = 3f;
            targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
        }

        ship.transform.position = Vector3.MoveTowards(ship.transform.position, targetDir, 0.1f * Time.deltaTime);      
    }


    //this is called by an event on button press
    public void UI_OnOptionsCall(bool optionPanelActive)
    {
        mOptionPanelActive = optionPanelActive;
        if (!shipMoved && !optionPanelActive)
        {
            ship.transform.position = shipMoveBackPos;
            shipMoved = true;
            MoveShipBack = true;
        }

        
    }

    //ShipSelect
    public void UI_SwappedToShipSelectScreen(bool towards)
    {   //call once on screen swap
        if (towards)
            Camera.main.GetComponent<Animation>().Play("CameraSelectShipPingpong");
        else
            Camera.main.GetComponent<Animation>().Play("CameraSelectShippong");
    }
    //move tray
    public void setTrayMoveTo(int posIndex)
    {   //called once by ship select advance/retreat selector
        trayindex = posIndex;
    }
    public void MoveTray()
    {//call every update
        selectTray.transform.position = Vector3.MoveTowards(selectTray.transform.position, trayStartPos - new Vector3(0,0,trayindex*5), 100 * Time.deltaTime);

    }

    public void UI_OnVolumeChanged(bool temp)
    {
        music.volume = PlayerPrefs.GetFloat("musicVolume");

    }




    public void ScreenChanged(ScreenElement newScreen)
    {
        ui = newScreen;
    }

}
