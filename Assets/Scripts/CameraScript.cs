using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	public GameObject ship;
    private float minDistance = 35f;

    private float speed = 0f;
    public float verticalD = 45f;
    public float horizontalD = 22.5f;

    void Start()
    {
        minDistance = Vector3.Distance(transform.position, ship.transform.position)-15f;
    }

	void Update ()
    {
        speed = (Vector3.Distance(transform.position, ship.transform.position) - minDistance) * Time.deltaTime;

        if (speed > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(ship.transform.position.x, verticalD, ship.transform.position.z - horizontalD), speed);
        }
        //else if (speed < -1)
        //{
        //    //transform.position = Vector3.MoveTowards(transform.position, new Vector3(ship.transform.position.x, verticalD, ship.transform.position.z - horizontalD), speed);
        //    //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(ship.transform.position.x + horizontalD, verticalD, ship.transform.position.z + horizontalD), speed, speed);
        //}

        Quaternion targetRotation = Quaternion.LookRotation(ship.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
        //transform.LookAt(ship.transform.position);
    }
}
