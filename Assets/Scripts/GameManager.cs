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
    private UI ui;
    private EnemyManager em;
    private WaveManager wm;
    private GameObject pointer;

    private LineRenderer laser;
    [SerializeField] private AudioSource music;

    

	// Use this for initialization
	void Awake () 
    {
        music.volume = 0.2f * PlayerPrefs.GetFloat("musicVolume");

        playerShip = (GameObject)Instantiate(playerShipPref, Vector3.zero, Quaternion.identity);
        pointer = (GameObject)Instantiate(pointerPref, Vector3.zero, Quaternion.identity);
        pointer.GetComponent<Pointer>().SetFollowTarget(playerShip);
        Time.timeScale = 1;
	}
    
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.startWidth = 0.2f;
        laser.endWidth = 0.2f;

        ui = GetComponent<UI>();
        em = GetComponent<EnemyManager>();
        wm = GetComponent<WaveManager>();
        cam.SetTarget(playerShip); // give the player ship reference to the camera
    }


	// Update is called once per frame
	void Update () 
    {
        Vector3 ClosestEnemy = em.getClosestShipPos(playerShip.transform.position);
        pointer.GetComponent<Pointer>().SetNewTarget(ClosestEnemy);

        laser.SetPosition(0, playerShip.transform.position);
        laser.SetPosition(1, ClosestEnemy);


        ShipStats s = playerShip.GetComponent<ShipStats>();
        ui.UpdateGameStats(currentScore, em.GetTotalShipLeft(), wm.GetWave());
        ui.UpdateShipStats(s.GetBoostFuelAmount(), s.GetShieldPUState(), s.ShipHealth, s.GetNoMissiles(), playerShip.GetComponent<ShipController>().GetWeaponType());
        if (!s.IsAlive()) ui.SetGameOverState(true);

        Time.timeScale = (ui.GetMenuState() || ui.GetHintsState()) ? 0 : 1;

        if (ui.GetMenuState())
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Y)) RestartGame(); // if A controller button or Y keyboard button
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) ToggleUIState(); // B controller button or Escape button
            else if (Input.GetKeyDown(KeyCode.JoystickButton3)) LoadMainMenu();
        }
        else if (ui.GetHintsState() && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space)))
        {
            ui.IncrementDHIndex();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) // esape button or Start controller button
        {
            ui.ToggleEscState();
        }
    
    }

    public void AddScore(int s)
    {
        currentScore += s;
    }
    public GameObject GetShipRef()
    {
        return playerShip;
    }

    public void ToggleUIState()
    {
        ui.ToggleEscState();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);     
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }


}
