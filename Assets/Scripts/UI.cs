using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text missileCounter;

    private bool displayMenu;                       // toggle to display menu
    [SerializeField] private GameObject menuPanel;  // menu object
    

    [SerializeField] private Text[] gameStats = new Text[3];                // score, enemy left, wave counter
    [SerializeField] private GameObject[] HUDs = new GameObject[3];
    [SerializeField] private GameObject[] statBars = new GameObject[2];     // helth, boost
    [SerializeField] private GameObject[] indicators = new GameObject[5];   // shield on, shield off, single, tri, missile

    private bool displayHints;          // toggle hint object
    private int displayHintIndex;       // index of showing a specific hint object
    [SerializeField] private GameObject[] hintPanels = new GameObject[4]; // controls, tips, objective

    private float popUpMTimer;          // time limit to display a specific pop up
    private bool displayPopUpMessages;  // toggle pop up display on and off
    private int messageIndex;           // index of showing a specific pop up
    [SerializeField] private GameObject[] popUpMessages = new GameObject[6]; // win, gameOver, Paused, generatorActivated, WarpGateActivated, BoundaryWarning

    private bool displayBoundary;       // display boundary toggle
    private bool escButtonPressed;      // esc button toggle
    private bool gameOver;

    private float helthFlashingTimer = 0;
    private bool hpActive = true;
    [SerializeField] private GameObject healthIndicator;


	void Start () // Use this for initialization
    {
        messageIndex = 2;
        displayHintIndex = 0;
        escButtonPressed = false;
        displayBoundary = false;
        displayMenu = false;
        displayPopUpMessages = false;
        gameOver = false;
        displayHints = true;
        popUpMTimer = Time.time;

        if (displayHints) { 
            UpdateHintsPanel();
            
            for (int i = 0; i < HUDs.Length; i++)
            {
                HUDs[i].SetActive(!displayHints);
            }
        }
    }		
	void Update () // Update is called once per frame
    {
        UpdateMenu();
    }

    public void IncrementDHIndex()
    {
        displayHintIndex++;
        UpdateHintsPanel();
    }
    public void ToggleEscState()
    {
        escButtonPressed = !escButtonPressed;
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
            if (helthFlashingTimer < Time.time)
            {
                hpActive = !hpActive;
                healthIndicator.SetActive(hpActive);
                helthFlashingTimer = Time.time + 0.2f;
                
            }
        }
        else
        {
            hpActive = true;
            healthIndicator.SetActive(hpActive);
        }



    }
    private void UpdateMenu()
    {
        // KEEP THE UPDATE ORDER!!!
        // 1: boundary message
        // 2: Escape button
        // 3: ship health
        // 4: switch
        // 5: display panels

        // SET LOGIC

        // update local state flag
        if (displayBoundary != transform.GetComponent<BoundaryManager>().GetState())
        {
            displayBoundary = transform.GetComponent<BoundaryManager>().GetState();
            if (displayBoundary == false) SetMessage(2);
            else SetMessage(5);
        }

        if (escButtonPressed && (messageIndex != 0 || messageIndex != 1))
        {
            escButtonPressed = false; // only have escButtonPressed true till a single frame
            //Debug.Log("ESC Pressed");
            SetMessage(2);
            displayMenu = !displayMenu;
        }

        if (gameOver == true)
        {
            SetMessage(1);
        }

        // PROCESS LOGIC
        switch (messageIndex)
        {
            case 0:
            case 1: {
                    displayMenu = true;  // force menu to be on
                    displayPopUpMessages = true;
                } break;

            case 2: {
                    displayPopUpMessages = displayMenu;         
                } break;

            case 3:
            case 4: {
                    if (Time.time < popUpMTimer)
                    {
                        displayPopUpMessages = true;
                    }
                    else
                    {
                        displayPopUpMessages = false;
                        SetMessage(2);
                    }
                } break;

            case 5: {
                    displayPopUpMessages = true;
                } break;

        }
        
        // DRAW PANELS
        popUpMessages[messageIndex].SetActive(displayPopUpMessages);

        menuPanel.SetActive(displayMenu);
        for (int i = 0; i < HUDs.Length; i++)
        {
            HUDs[i].SetActive(!displayMenu);
        }
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
            displayHintIndex = 0; // hard reset
            displayHints = false;
            hintPanels[hintPanels.Length-1].SetActive(false); // turn of last panel
        }
    }

    public void SetMessage(int v)
    {
        messageIndex = v;
        

        if (messageIndex > 5)
        {
            messageIndex = 2;
            //Debug.Log("out of index message");
        }
        else
        {
            popUpMTimer = Time.time + 2f;
        }

        for (int i = 0; i < popUpMessages.Length; i++)
        {
            popUpMessages[i].SetActive(false);
        } 
    }

    public void ResetButton()
    {
        GetComponent<GameManager>().RestartGame();
    }
    public void MainMenuButton()
    {
        GetComponent<GameManager>().LoadMainMenu();
    }

}
