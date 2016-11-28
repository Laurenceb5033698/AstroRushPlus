using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	public GameObject ship;
    private float minDistance = 0f;

    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float acceleration = 1f;
    public float verticalD = 45f;
    public float horizontalD = 22.5f;

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
            Vector3 target = new Vector3(ship.transform.position.x, verticalD, ship.transform.position.z - horizontalD);
            float smoothTime = 0.3f;
            //Vector3 velocity = ship.GetComponent<Rigidbody>().velocity;
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, target,ref velocity, smoothTime);
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(ship.transform.position.x, verticalD, ship.transform.position.z - horizontalD), targetSpeed);
            //transform.Translate((transform.position - new Vector3(ship.transform.position.x, 0f, ship.transform.position.z - 22.5f)).normalized*currentSpeed); // camera alternative. a bit buggy at the momemet
        }

        Quaternion targetRotation = Quaternion.LookRotation(ship.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);

        //transform.LookAt(ship.transform.position);
    }
}
