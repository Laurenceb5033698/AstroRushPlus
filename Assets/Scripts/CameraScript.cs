using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    [SerializeField]
	private GameObject ship;
    [SerializeField]
    private GameObject refPos;
    [SerializeField]
    private float distance = 0.0f;

    void Start()
    {
        refPos.transform.localPosition = new Vector3(-30f, 50f, 0); // horizontal and vertical offset
    }

	void Update ()
    {
        distance = Vector3.Distance(transform.position, refPos.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, refPos.transform.position, distance * Time.deltaTime);
        transform.LookAt(ship.transform.position);

        // prevoius camera version - more cinematic
        //distance = Vector3.Distance(transform.position, ship.transform.position); 
        //targetPosition.x = ship.transform.position.x;
        //targetPosition.z = ship.transform.position.z - horizontalD;
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, (distance - minDistance) * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ship.transform.position - transform.position), Time.deltaTime);
    }
}
