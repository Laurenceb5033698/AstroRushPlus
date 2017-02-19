using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    
    [SerializeField] private GameObject planet;
    [SerializeField] private AudioSource music;


    // option menu
    [SerializeField] private GameObject optionsPanel;
    private bool optionPanelActive = false;
    private bool shipMoved = false;
    private bool MoveShipBack = false;
    private Vector3 shipMovePos = new Vector3(-28,-15,-30);
    private Vector3 shipMoveBackPos = new Vector3(53,17,35);
    [SerializeField] private Slider volumeSlider;

    // ship
    [SerializeField] private GameObject ship;
    private Vector3 shipStartPos;
    private float timeToChangeDir;
    private Vector3 targetDir;
    private Vector3 targetRot;

    void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
            music.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            volumeSlider.value = 1;
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        }


        optionsPanel.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        shipStartPos = ship.transform.position;
        targetRot = ship.transform.eulerAngles;
        Time.timeScale = 1;
        timeToChangeDir = Time.time + 3f;
        float temp = 3f;
        targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) StartButton(); // if A controller button or Enter keyboard button
        else if (Input.GetKeyDown(KeyCode.JoystickButton3)) OptionsButton();
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) QuitButton(); // B controller button or Escape button

        if (!optionPanelActive)
        {
            if (Vector3.Distance(ship.transform.position, shipStartPos) > 5)
            {
                MoveShipBack = true;
            }
            else
            {
                shipMoved = false;
                MoveShip();
            }  
        }
        else if (!shipMoved)
        {
            ship.transform.position = Vector3.MoveTowards(ship.transform.position, shipMovePos, 20 * Time.deltaTime);

            if (Vector3.Distance(ship.transform.position, shipStartPos) > 40)
            {
                shipMoved = true;
                ship.transform.position = shipMoveBackPos;
            }
        }

        if (MoveShipBack)
        {
            ship.transform.position = Vector3.MoveTowards(ship.transform.position,shipStartPos,20 * Time.deltaTime);
            if (Vector3.Distance(ship.transform.position, shipStartPos) < 0.5f)
            {
                ship.transform.position = shipStartPos;
                MoveShipBack = false;
            }
        }

        RotatePlanet();

    }

    private void RotatePlanet()
    {
        planet.transform.Rotate(Vector3.up * -1*Time.deltaTime);
    }

    private void MoveShip()
    {
        if (Time.time > timeToChangeDir)
        {
            timeToChangeDir = Time.time + 3f;
            float temp = 3f;
            targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
        }

        ship.transform.position = Vector3.MoveTowards(ship.transform.position, targetDir, 0.1f * Time.deltaTime);      
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        if (!shipMoved && optionPanelActive)
        {
            ship.transform.position = shipMoveBackPos;
            shipMoved = true;
            MoveShipBack = true;
        }

        optionPanelActive = !optionPanelActive;
        optionsPanel.SetActive(optionPanelActive);
    }




    public void fullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void musicVolumeOnValueChanged()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        music.volume = PlayerPrefs.GetFloat("musicVolume");
        PlayerPrefs.Save();
    }
}
