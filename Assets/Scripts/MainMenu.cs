using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject planet;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject enemyShip;

    private Vector3 PlayerShipStartPos;
    private Vector3 EnemyShipStartPos;

    private float timeToChangeDir;

    private Vector3 PlayerTargetDir;
    private Vector3 PlayerTargetRot;
    private Vector3 EnemyTargetDir;
    private Vector3 EnemyTargetRot;

	// Use this for initialization
	void Start () {
        PlayerShipStartPos = playerShip.transform.position;
        EnemyShipStartPos = enemyShip.transform.position;

        Time.timeScale = 1;
        //shipStartRot = ship.transform.rotation;

        timeToChangeDir = Time.time + 3f;
        float temp = 3f;
        PlayerTargetDir = new Vector3(PlayerShipStartPos.x + Random.Range(-temp, temp), PlayerShipStartPos.y + Random.Range(-temp, temp), PlayerShipStartPos.z + Random.Range(-temp, temp));
        EnemyTargetDir = new Vector3(EnemyShipStartPos.x + Random.Range(-temp, temp), EnemyShipStartPos.y + Random.Range(-temp, temp), EnemyShipStartPos.z + Random.Range(-temp, temp));

    }
	
	// Update is called once per frame
	void Update ()
    {
        RotateSaturn();
        MoveShip();


        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) StartButton(); // if A controller button or Enter keyboard button
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) QuitButton(); // B controller button or Escape button


    }

    private void RotateSaturn()
    {
        planet.transform.Rotate(Vector3.up * -1*Time.deltaTime);
    }



    private void MoveShip()
    {
        if (Time.time > timeToChangeDir)
        {
            timeToChangeDir = Time.time + 3f;
            float temp = 3f;

            PlayerTargetDir = new Vector3(PlayerShipStartPos.x + Random.Range(-temp, temp), PlayerShipStartPos.y + Random.Range(-temp, temp), PlayerShipStartPos.z + Random.Range(-temp, temp));
            EnemyTargetDir = new Vector3(EnemyShipStartPos.x + Random.Range(-temp, temp), EnemyShipStartPos.y + Random.Range(-temp, temp), EnemyShipStartPos.z + Random.Range(-temp, temp));
        }
        playerShip.transform.position = Vector3.MoveTowards(playerShip.transform.position, PlayerTargetDir, 0.1f * Time.deltaTime);
        enemyShip.transform.position = Vector3.MoveTowards(enemyShip.transform.position, EnemyTargetDir, 0.1f * Time.deltaTime);
        enemyShip.transform.LookAt(playerShip.transform.position);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
