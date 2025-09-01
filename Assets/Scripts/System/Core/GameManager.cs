using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance = null;

    private GameObject playerShip;
    public int currentScore = 0;

    //warpin effects timer
    private bool m_Warping = true;
     [SerializeField] public float m_WarpinTime = 2f;
    private float m_currentWarp = 0;

    [SerializeField] private GameObject[] playerShipPref = new GameObject[4];
    [SerializeField] private GameObject pointerPref;
    [SerializeField] private CameraScript cam;

    //Temp Stage Management.
    private int m_stageCounter = 0;
    [SerializeField] private List<StageDataScriptable> m_StageDataPool;

    private AIManager aiMngr;
    private AsteroidManager asm;
    public GameObject pointer;
    public Inputs GlobalInputs;

    [SerializeField] private AudioSource music;

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
	}
    
    void Start()
    {
        //playerfactory type stuff
        ShipFactory playerFactory = new ShipFactory();
        playerFactory.SetVaraibles(playerShipPref[0], GlobalInputs);
        playerShip = playerFactory.CreatePlayerShip();


        pointer = (GameObject)Instantiate(pointerPref, Vector3.zero, Quaternion.identity);
        pointer.GetComponent<Pointer>().SetFollowTarget(playerShip);
        cam.SetTarget(playerShip); // give the player ship reference to the camera

        aiMngr = GetComponent<AIManager>();
        asm = GetComponent<AsteroidManager>();

        aiMngr.enabled = true;
        asm.enabled = true;

        //at start of level, set warp in time.
        m_currentWarp = Time.time + m_WarpinTime;
        ServicesManager.Instance.GameStateService.GameState = GameState.WARPIN;
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
    {
        //ensures instances are created in the right scene
        this.enabled = true;
    }
    void OnDestroy()
    {
        //clean events
        SceneLoader.Loaded -= SL_OnLoadingComplete;
    }

	void Update () {
    }

    //#############
    //State machine

    private void WarpIntoLevel()
    {
        //wierd, ui is playing warp effect, maybe warpeffect vfx ends here
        //end warp vfx
        //once ended, transition to main game
        //can pause manually here?
        //somehow need to return to last state from pause
        UpdateGameUI();
        if(Time.time > m_currentWarp && m_Warping)
        {
            aiMngr.NewWave();
            //ResumingGame();
            //trigger ui for new level/objectives/hazards (depends on generated level)
            ServicesManager.Instance.GameStateService.GameState = GameState.MAINGAME;

            m_Warping = false;
        }
    }

    private void MainGame()
    {
        if (!playerShip.GetComponent<Stats>().IsAlive())
        {
            //todo: maybe allow death sequence to play first
            //gamestate pause
            ServicesManager.Instance.GameStateService.GameState = GameState.PAUSE;
            ServicesManager.Instance.PauseService.Pause();
            //change ui to gameover screen.
            UIManager.GetGameUiObject().Button_PausePressed(true);
            return;
        }

        //if(aiMngr.EndoOfWave)
        //{
        //  if(!aiMngr.m_objectiveCompleted)
        //  {       //upgrade }
        //  else {  //evo }
        //}
        if (aiMngr.EndOfWave == true) 
        { 
            UIManager.instance.UpgradeScreen();
            ServicesManager.Instance.GameStateService.GameState = GameState.PICKUPGRADE;
            ServicesManager.Instance.PauseService.Pause();
            aiMngr.NewWave();
        }
        else
        {
            Vector3 ClosestEnemy = aiMngr.GetClosestShipPos(playerShip.transform.position);
            pointer.GetComponent<Pointer>().SetNewTarget(ClosestEnemy);

            UpdateGameUI();
        }
        

    }

    private void PickingUpgrade()
    {
        //currently viewing upgrade screen, or evo screen.
        //game mostly does nothing here, waiting for player. is already paused.

        // can either return to maingame, or warpout, depending on aimngr objective completed
    }

    private void WarpOutofLevel()
    {
        //enters this gamestate section from upgrade screen.
        //start playing warp effect

        UpdateGameUI();
        if (Time.time > m_currentWarp && m_Warping) 
        {
            //playing full warp effect.
            //  warp effect palys, but differnt wave data loaded, bg changes, ui updated, but same scene
            EndLevel();
            ServicesManager.Instance.GameStateService.GameState = GameState.WARPIN;
        }

    }

    private void GamePaused()
    {
        //uh, dont think we do anything here
        //ui is pause screen/settings.
    }



    public void ResumeFromPickUpgrade()
    {
        //called from UI_Upgrade
        if (aiMngr.m_ObjectiveComplete && aiMngr.EndOfWave)
        {
            //boss is beaten, goto warpout
            ServicesManager.Instance.GameStateService.GameState = GameState.WARPOUT;
        }
        else
        {
            //continue waves
            ServicesManager.Instance.GameStateService.GameState = GameState.MAINGAME;
            
        }
    }

    private void UpdateGameUI()
    {
        Stats s = playerShip.GetComponent<Stats>();
        UI_Game gamescreen = UIManager.GetGameUiObject();
        if (gamescreen)
        {
            gamescreen.UpdateShipStats(s);
        }
    }

    public void EndLevel()
    {
        if (m_stageCounter >= m_StageDataPool.Count)
        {
            m_stageCounter = 0;
        }
        aiMngr.NewStage(m_StageDataPool[m_stageCounter]);
        m_stageCounter++;
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


}
