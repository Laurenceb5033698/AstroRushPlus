using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	public GameObject ship;
    private float minDistance = 0f;

    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float acceleration = 1f;

    void Start()
    {
        minDistance = Vector3.Distance(transform.position, ship.transform.position);
        currentSpeed = (Vector3.Distance(transform.position, ship.transform.position) - minDistance) * Time.deltaTime;
        targetSpeed = currentSpeed;
    }

	void Update ()
    {

        //transform.Translate(new Vector3(ship.transform.position.x, 0f, ship.transform.position.z - 22.5f)*Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position,new Vector3(ship.transform.position.x,45f,ship.transform.position.z-22.5f),speed);


        targetSpeed = (Vector3.Distance(transform.position, ship.transform.position) - minDistance) * Time.deltaTime;

        if (currentSpeed < targetSpeed) currentSpeed += acceleration*Time.deltaTime;
        else currentSpeed -= acceleration*Time.deltaTime;

        if (currentSpeed > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(ship.transform.position.x, 45f, ship.transform.position.z - 22.5f), targetSpeed);
            //transform.Translate((transform.position - new Vector3(ship.transform.position.x, 0f, ship.transform.position.z - 22.5f)).normalized*currentSpeed); // camera alternative. a bit buggy at the momemet
        }

        transform.LookAt(ship.transform.position);
    }
}
