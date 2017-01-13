using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	public GameObject SSIpanel;
	public Text units;
	public Text fuel;
	public Text cargo;
	public Text damage;
    public Text BoundaryText;

    public GameObject menuPanel;
    public GameObject BoundryPanel;
    public Text menuPanelT;

    [SerializeField] private GameObject[] hintPanels = new GameObject[3];
    private int displayHintIndex = 0;


    private bool displayMenu = false;
    private bool displayBoundary = false;
    [SerializeField] private bool displayHints = true;

	// Use this for initialization
	void Start ()
    {
        SSIpanel.SetActive(false);
        setMessage(2);
        menu = displayMenu;
        BoundaryWarning = displayBoundary;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (displayHints)
        {
            //Debug.Log("display hint");
            if (displayHintIndex < 3)
            {
                Time.timeScale = 0;

                //Debug.Log("index is less then length");
                for (int i = 0; i < 3; i++)
                {
                    if (displayHintIndex != i)
                    {
                        //Debug.Log("turn panels off");
                        hintPanels[i].SetActive(false);
                    }
                    else
                    {
                        //Debug.Log("activate current panel");
                        hintPanels[displayHintIndex].SetActive(true);
                    }
                }
            }
            else
            {
                //Debug.Log("turn off display");
                displayHintIndex = 0; // hard reset
                displayHints = false;
                hintPanels[2].SetActive(false);
                Time.timeScale = 1;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                //Debug.Log("increment index");
                displayHintIndex++;
            }
        }
        else
        {
            Time.timeScale = 1;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC Pressed");
            menu = !displayMenu;
        }
    }

	public void UpdateShipStats(int u, float f, string c, float d)
	{
		units.text = "Units: " + u;
		fuel.text = "Boost: " + f.ToString ("N0");
		cargo.text = "Cargo: " + c;
		damage.text = "Health: " + d.ToString("N0");
	}

    public void UpdateStationPanelToggle(bool s)
    {
        SSIpanel.SetActive(s);
    }

    public bool menu
    {
        get
        {
            return displayMenu;
        }
        set
        {
            displayMenu = value;
            menuPanel.SetActive(displayMenu);

            if (displayMenu)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    public bool BoundaryWarning
    {
        get
        {
            return displayBoundary;
        }
        set
        {
            displayBoundary = value;
            BoundryPanel.SetActive(displayBoundary);
        }
    }

    public void setMessage(int v)
    {
        if (v == 0) menuPanelT.text = "GAME OVER!";
        else if (v == 1) menuPanelT.text = "LEVEL COMPLETE!";
        else if (v == 2) menuPanelT.text = "";
    }


    public void ResetButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    
    public void HideBoundaryWarning()
    {

    }


}
