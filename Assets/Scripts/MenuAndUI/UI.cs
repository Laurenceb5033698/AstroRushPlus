using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    [SerializeField] private ShipStats stats;

    [SerializeField] private Text missileCounter;

    private bool displayMenu = false;
    public GameObject menuPanel;

    [SerializeField] private Text[] statsText = new Text[4]; // boost, shield, health, laser
    [SerializeField] private GameObject[] statBars = new GameObject[4]; // boost, shield, health, laser

    private int displayHintIndex = 0;
    [SerializeField] private bool displayHints = true;
    [SerializeField] private GameObject[] hintPanels = new GameObject[3]; // controls, tips, objective

    private bool displayPopUpMessages = false;
    [SerializeField] private int messageIndex = 2;
    [SerializeField] private GameObject[] popUpMessages = new GameObject[6]; // win, gameOver, Paused, generatorActivated, WarpGateActivated, BoundaryWarning
    private float popUpMTimer;
    private bool displayBoundary = false;

	void Start () // Use this for initialization
    {
        displayHintIndex = 0;
        displayPopUpMessages = false;
        Time.timeScale = 1;
        popUpMTimer = Time.time;
    }		
	void Update () // Update is called once per frame
    {
        if (displayHints) Displayhints();
        UpdateShipStats(stats.ShipFuel, stats.ShipShield, stats.ShipHealth, 100.0f);
        UpdateMenu();
    }

    private void Displayhints()
    {
        if (displayHintIndex < 3)
        {
            Time.timeScale = 0;

            for (int i = 0; i < 3; i++)
            {
                if (displayHintIndex != i) hintPanels[i].SetActive(false);
                else hintPanels[displayHintIndex].SetActive(true);
            }
        }
        else
        {
            displayHintIndex = 0; // hard reset
            displayHints = false;
            hintPanels[hintPanels.Length-1].SetActive(false); // turn of last panel
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0)) displayHintIndex++;
    }
	public void UpdateShipStats(float b, float s, float h, float l)
	{
        missileCounter.text = "X " + stats.GetNoMissiles();

		statsText[0].text = "Boost: " + b.ToString ("N0");
        statsText[1].text = "Shield: " + s.ToString("N0");
        statsText[2].text = "Health: " + h.ToString("N0");
        statsText[3].text = "Laser: " + l.ToString("N0");

        Vector3 temp;

        temp = statBars[0].transform.localScale;
        temp.x = (0.45f / 100) * b;
        statBars[0].transform.localScale = temp;

        temp = statBars[1].transform.localScale;
        temp.x = (0.68f / 100) * s;
        statBars[1].transform.localScale = temp;

        temp = statBars[2].transform.localScale;
        temp.x = (0.9f / 100) * h;
        statBars[2].transform.localScale = temp;

        temp = statBars[3].transform.localScale;
        temp.x = (1.3f / 100) * l;
        statBars[3].transform.localScale = temp;
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
            if (displayBoundary == false) setMessage(2);
            else setMessage(5);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && (messageIndex != 0 || messageIndex != 1))
        {
            Debug.Log("ESC Pressed");
            setMessage(2);
            displayMenu = !displayMenu;
        }

        if (stats.IsShipWorking() == false)
        {
            setMessage(1);
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
                        setMessage(2);
                    }
                } break;

            case 5: {
                    displayPopUpMessages = true;
                } break;

        }
        
        // DRAW PANELS
        popUpMessages[messageIndex].SetActive(displayPopUpMessages);
        if (displayMenu)
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void setMessage(int v)
    {
        messageIndex = v;
        

        if (messageIndex > 5)
        {
            messageIndex = 2;
            Debug.Log("out of index message");
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
        SceneManager.LoadScene(1);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

}
