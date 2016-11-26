using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	public GameObject ship;
    private float minDistance = 0f;

    void Start()
    {
        minDistance = Vector3.Distance(transform.position, ship.transform.position);
    }

	void Update ()
    {

        //transform.Translate(new Vector3(ship.transform.position.x, 0f, ship.transform.position.z - 22.5f)*Time.deltaTime);


        //transform.position = new Vector3 (ship.transform.position.x, 45f, ship.transform.position.z - 22.5f);
        transform.LookAt(ship.transform.position);
        float speed = (Vector3.Distance(transform.position, ship.transform.position) - minDistance) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(ship.transform.position.x,45f,ship.transform.position.z-22.5f),speed);
    }
}
