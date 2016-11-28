using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject saturn;
    public GameObject ship;
    public GameObject station;

    private Vector3 shipStartPos;
    private Quaternion shipStartRot;

    private float timeToChangeDir;

    private Vector3 targetDir;
    private Vector3 targetRot;

	// Use this for initialization
	void Start () {
        shipStartPos = ship.transform.position;
        shipStartRot = ship.transform.rotation;

        timeToChangeDir = Time.time + 3f;
        float temp = 3f;
        targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
    }
	
	// Update is called once per frame
	void Update ()
    {
        RotateSaturn();
        MoveShip();
        RotateStation();



    }

    private void RotateSaturn()
    {
        saturn.transform.Rotate(Vector3.up * 1*Time.deltaTime);
    }

    private void RotateStation()
    {
        station.transform.RotateAround(saturn.transform.position,saturn.transform.up,2f * Time.deltaTime);
        station.transform.localEulerAngles = (new Vector3(station.transform.localEulerAngles.x,station.transform.localEulerAngles.y,station.transform.localEulerAngles.z + 10f * Time.deltaTime));
    }

    private void MoveShip()
    {
        if (Time.time > timeToChangeDir)
        {
            timeToChangeDir = Time.time + 3f;
            float temp = 3f;
            targetDir = new Vector3(shipStartPos.x + Random.Range(-temp, temp), shipStartPos.y + Random.Range(-temp, temp), shipStartPos.z + Random.Range(-temp, temp));
            //targetRot = new Vector3(ship.transform.localEulerAngles.x + Random.Range(-33, 7), ship.transform.localEulerAngles.y + Random.Range(30, 20), ship.transform.localEulerAngles.z + Random.Range(-12, 13));
        }

        ship.transform.position = Vector3.MoveTowards(ship.transform.position, targetDir, 0.1f * Time.deltaTime);
        //Vector3 temp2 = ship.transform.eulerAngles;
        //float speed = 5f * Time.deltaTime;
        //ship.transform.eulerAngles = new Vector3(temp2.x + speed, temp2.y + speed, temp2.z + speed);
        
    }

    public void StartButton()
    {
        Application.LoadLevel(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
