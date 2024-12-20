﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text missileCounter;

    private bool displayMenu;                       // toggle to display menu
    [SerializeField] private GameObject menuPanel;  // menu object
    [SerializeField] private GameObject continueButton;

    [SerializeField] private Text[] gameStats = new Text[3];                // score, enemy left, wave counter
    [SerializeField] private GameObject[] HUDs = new GameObject[3];
    [SerializeField] private GameObject[] statBars = new GameObject[2];     // helth, boost
    [SerializeField] private GameObject[] indicators = new GameObject[5];   // shield on, shield off, single, tri, missile

    private bool displayHints;          // toggle hint object
    private int displayHintIndex;       // index of showing a specific hint object
    [SerializeField] private GameObject[] hintPanels = new GameObject[4]; // controls, tips, objective

    private bool displayPopUpMessages;  // toggle pop up display on and off
    private int messageIndex;           // index of showing a specific pop up
    [SerializeField] private GameObject[] popUpMessages = new GameObject[3]; // gameOver, Paused, BoundaryWarning

    private bool escButtonPressed;      // esc button toggle
    private bool gameOver;

    private float helthFlashingTimer = 0;
    private bool hpActive = true;
    [SerializeField] private GameObject healthIndicator;
    [SerializeField] private GameObject healthVignette;

    private GameManager gm = null;
	void Start () // Use this for initialization
    {
        messageIndex = 1;
        displayHintIndex = 0;
        escButtonPressed = false;
        displayMenu = false;
        displayPopUpMessages = false;
        gameOver = false;

        displayHints = (PlayerPrefs.GetInt("showHints") == 1);

        if (displayHints)
        { 
            UpdateHintsPanel();
            menuPanel.SetActive(false);
            
            for (int i = 0; i < HUDs.Length; i++)
            {
                HUDs[i].SetActive(!displayHints);
            }
            
        }
    }
    private void OnEnable()
    {
        SceneLoader.Loaded += AttachGameManager;
    }
    private void OnDisable()
    {
        SceneLoader.Loaded -= AttachGameManager;
    }
	void Update () // Update is called once per frame
    {
        if (!displayHints)
            UpdateMenu();
    }
    public void AttachGameManager()
    {
        gm = GameManager.instance;
    }
    public void IncrementDHIndex()
    {
        if (!GetMenuState())
        {
            displayHintIndex++;
            displayHints = true;
            UpdateHintsPanel();
        }
    }
    public void ToggleEscState()
    {
        if (!GetHintsState())
        {
            escButtonPressed = !escButtonPressed;
        }
    }
    public bool GetMenuState()
    {
        return displayMenu;
    }
    public bool GetHintsState()
    {
        return displayHints;
    }
    public void SetGameOverState(bool g)
    {
        gameOver = g;
    }

    public void UpdateGameStats(int score, int NoEnemy, int Wave)
    {
        gameStats[0].text = score.ToString();
        gameStats[1].text = NoEnemy.ToString();
        gameStats[2].text = Wave.ToString();
    }
    public void UpdateShipStats(float boost, bool shield, float health, int missile, int wType)
	{
        missileCounter.text = "X " + missile;

        Vector3 temp;
        temp = statBars[0].transform.localScale;
        temp.x = (2.5f / 100) * health;
        statBars[0].transform.localScale = temp;

        temp = statBars[1].transform.localScale;
        temp.x = (2.5f / 100) * boost;
        statBars[1].transform.localScale = temp;


        indicators[0].SetActive(shield);
        indicators[1].SetActive(!shield);

        switch (wType)
        {
            case 0: indicators[2].SetActive(true); indicators[3].SetActive(false); indicators[4].SetActive(false); break;
            case 1: indicators[2].SetActive(false); indicators[3].SetActive(true); indicators[4].SetActive(false); break;
            case 2: indicators[2].SetActive(false); indicators[3].SetActive(false); indicators[4].SetActive(true); break;
            default: Debug.Log("You f*cked up the index of the weapon type!"); break;
        }

        if (health < 50)
        {
            Color tempCol = healthVignette.GetComponent<RawImage>().color;
            tempCol.a = -((health / 50) * 0.7f) + 0.7f;
            healthVignette.GetComponent<RawImage>().color = tempCol;


            if (helthFlashingTimer < Time.time)
            {
                hpActive = !hpActive;
                healthIndicator.SetActive(hpActive);
                helthFlashingTimer = Time.time + 0.2f;
                
            }
        }
        else
        {
            Color tempCol = healthVignette.GetComponent<RawImage>().color;
            tempCol.a = 0;
            healthVignette.GetComponent<RawImage>().color = tempCol;

            hpActive = true;
            healthIndicator.SetActive(hpActive);
        }
        



    }
    private void UpdateMenu()
    {
        if (!gameOver)
        {
            SetMessage(1);

            if (gm.GetComponent<BoundaryManager>().GetState())
            {
                SetMessage(2);
                displayPopUpMessages = true;
            }
            if (escButtonPressed)
            {
                escButtonPressed = false;
                SetMessage(1);
                displayMenu = !displayMenu;
            }

            for (int i = 0; i < HUDs.Length; i++)
            {
                HUDs[i].SetActive(!displayMenu);
            }
        }
        else
        {
            SetMessage(0);
            continueButton.SetActive(false);
            displayMenu = true;
            displayPopUpMessages = true;

            HUDs[0].SetActive(true);
            for (int i = 1; i < HUDs.Length; i++)
            {
                HUDs[i].SetActive(!displayMenu);
            }

        }
        if (messageIndex == 1) displayPopUpMessages = displayMenu;

        for (int i = 0; i < popUpMessages.Length; i++)
        {
            if (i == messageIndex) popUpMessages[i].SetActive(displayPopUpMessages);
            else popUpMessages[i].SetActive(false);
        }
        menuPanel.SetActive(displayMenu);


    }
    private void UpdateHintsPanel() // toggeled in IncrementDHIndex()
    {
        if (displayHintIndex < hintPanels.Length)
        {
            for (int i = 0; i < hintPanels.Length; i++)
                hintPanels[i].SetActive(displayHintIndex == i);
        }
        else
        {
            PlayerPrefs.SetInt("showHints", 0);
            PlayerPrefs.Save();
            displayHintIndex = 0; // hard reset
            displayHints = false;
            hintPanels[hintPanels.Length-1].SetActive(false); // turn of last panel
        }
    }

    private void SetMessage(int v)
    {
        if (messageIndex < 3)
        {
            messageIndex = v;
        }
    }

    public void ResetButton()
    {
        //GetComponent<GameManager>().RestartGame();
    }
    public void MainMenuButton()
    {
        //GetComponent<GameManager>().LoadMainMenu();
    }

}
