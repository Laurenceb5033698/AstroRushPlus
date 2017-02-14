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
    

	// Use this for initialization
	void Awake () 
    {
        spawnPlayer();
        spawnPointer();
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
        pointer.GetComponent<Pointer>().SetNewTarget(ClosestEnemy); // set pointer dir

        laser.SetPosition(0, playerShip.transform.position);
        laser.SetPosition(1, ClosestEnemy);


        UpdateUI();
        Time.timeScale = (ui.GetMenuState() || ui.GetHintsState()) ? 0 : 1;

        if (ui.GetComponent<UI>().GetMenuState() == true)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Y)) RestartGame(); // if A controller button or Y keyboard button
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) ui.ToggleEscState(); // B controller button or Escape button
            else if (Input.GetKeyDown(KeyCode.JoystickButton3)) LoadMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) // esape button or Start controller button
        {
            ui.ToggleEscState();
        }
    
    }

    private void UpdateUI()
    {
        ShipStats s = playerShip.GetComponent<ShipStats>();
        ui.UpdateGameStats(currentScore, em.GetTotalShipLeft(), wm.GetWave());

        if (Input.GetKeyDown(KeyCode.JoystickButton6)) ui.IncrementDHIndex();
        if (s.IsAlive()) ui.UpdateShipStats(s.GetBoostFuelAmount(), s.ShipShield, s.ShipHealth, 100, s.GetNoMissiles());
        else ui.SetGameOverState(true);
    }

    public void AddScore(int s)
    {
        currentScore += s;
    }

    private void spawnPlayer()
    {
        playerShip = (GameObject)Instantiate(playerShipPref,Vector3.zero, Quaternion.identity);
    }
    private void spawnPointer()
    {
        pointer = (GameObject)Instantiate(pointerPref, Vector3.zero, Quaternion.identity);
        pointer.GetComponent<Pointer>().SetFollowTarget(playerShip);
    }
    public GameObject GetShipRef()
    {
        return playerShip;
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
