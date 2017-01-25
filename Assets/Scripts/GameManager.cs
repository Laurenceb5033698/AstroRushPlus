using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameObject playerShip;
    private int currentScore = 0;
    private int highestScore = 0;

    [SerializeField] private GameObject playerShipPref;
    [SerializeField] private CameraScript cam;
    private UI ui;
    private EnemyManager em;
    private WaveManager wm;
    

	// Use this for initialization
	void Awake () 
    {
        spawnPlayer();
        //Time.timeScale = 1;
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
        //Time.timeScale = (ui.GetMenuState()) ? 0 : 1;
    }

    private void UpdateUI()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0)) ui.IncrementDHIndex();
        //if (Input.GetKeyDown(KeyCode.Escape)) ui.ToggleEscState();

        ui.updateGameStats(currentScore, em.GetTotalShipLeft(), wm.GetWave());
        
        if (playerShip != null)
        {
            ShipStats s = playerShip.GetComponent<ShipStats>();
            ui.UpdateShipStats(s.GetBoostFuelAmount(), s.ShipShield, s.ShipHealth, 100);
        }
    }

    private void spawnPlayer()
    {
        playerShip = (GameObject)Instantiate(playerShipPref,Vector3.zero, Quaternion.identity);
    }

    public GameObject GetShipRef()
    {
        return playerShip;
    }



}
