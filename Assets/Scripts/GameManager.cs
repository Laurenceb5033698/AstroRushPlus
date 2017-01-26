using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject playerShip;
    private int currentScore = 0;
    //private int highestScore = 0;

    [SerializeField] private GameObject playerShipPref;
    [SerializeField] private CameraScript cam;
    private UI ui;
    private EnemyManager em;
    private WaveManager wm;
    

	// Use this for initialization
	void Awake () 
    {
        spawnPlayer();
        Time.timeScale = 1;
	}
    
    void Start()
    {
        ui = GetComponent<UI>();
        em = GetComponent<EnemyManager>();
        wm = GetComponent<WaveManager>();
        cam.SetTarget(playerShip); // give the player ship reference to the camera
    }
	
	// Update is called once per frame
	void Update () 
    {
        UpdateUI();
        Time.timeScale = (ui.GetMenuState() || ui.GetHintsState()) ? 0 : 1;
    }

    private void UpdateUI()
    {
        ShipStats s = playerShip.GetComponent<ShipStats>();
        ui.UpdateGameStats(currentScore, em.GetTotalShipLeft(), wm.GetWave());

        if (Input.GetKeyDown(KeyCode.JoystickButton0)) ui.IncrementDHIndex();
        if (Input.GetKeyDown(KeyCode.Escape)) ui.ToggleEscState();
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
