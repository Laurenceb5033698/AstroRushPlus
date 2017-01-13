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

    private bool displayMenu = false;
    private bool displayBoundary = false;

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
