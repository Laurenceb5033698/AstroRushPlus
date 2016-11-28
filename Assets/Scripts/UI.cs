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

    public GameObject menuPanel;
    public Text menuPanelT;

    private int message = 0;
    private bool displayMenu = false;


	// Use this for initialization
	void Start ()
    {
        SSIpanel.SetActive(false);
        setMessage(2);
        menu = displayMenu;
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

	public void UpdateShipStats(int u, float f, string c, int d)
	{
		units.text = "Units: " + u;
		fuel.text = "Fuel: " + f.ToString ("F2") + "%";
		cargo.text = "Cargo: " + c;
		damage.text = "Damage: " + d + "%";
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
        }
    }

    public void setMessage(int v)
    {
        message = v;
        if (message == 0) menuPanelT.text = "GAME OVER!";
        else if (message == 1) menuPanelT.text = "LEVEL COMPLETE!";
        else if (message == 2) menuPanelT.text = "PAUSE GAME";
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
