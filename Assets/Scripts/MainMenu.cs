using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject planet;
    public GameObject station;

    private Vector3 shipStartPos;
    //private Quaternion shipStartRot;

    private float timeToChangeDir;

    private Vector3 targetDir;
    private Vector3 targetRot;

	// Use this for initialization
	void Start () {
        shipStartPos = transform.position;
        Time.timeScale = 1;
        //shipStartRot = ship.transform.rotation;

        timeToChangeDir = Time.time + 3f;
        float temp = 3f;
        targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
    }
	
	// Update is called once per frame
	void Update ()
    {
        


        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) StartButton(); // if A controller button or Enter keyboard button
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) QuitButton(); // B controller button or Escape button

        RotateSaturn();
        MoveShip();
        RotateStation();

    }

    private void RotateSaturn()
    {
        //Debug.Log(planet.gameObject.name);
        planet.transform.Rotate(Vector3.up * -1*Time.deltaTime);
    }

    private void RotateStation()
    {
        //station.transform.RotateAround(saturn.transform.position,saturn.transform.up,-2f * Time.deltaTime);
        station.transform.localEulerAngles = (new Vector3(station.transform.localEulerAngles.x,station.transform.localEulerAngles.y,station.transform.localEulerAngles.z + 10f * Time.deltaTime));
    }

    private void MoveShip()
    {
        if (Time.time > timeToChangeDir)
        {
            timeToChangeDir = Time.time + 3f;
            float temp = 3f;
            targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
        }

        transform.position = Vector3.MoveTowards(transform.position, targetDir, 0.1f * Time.deltaTime);      
    }

    public void StartButton()
    {
        //Application.LoadLevel(1);
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
