using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    [SerializeField]
	private GameObject ship;
    [SerializeField]
    private float verticalD = 30.0f;
    [SerializeField]
    private float horizontalD = 15.0f;
    [SerializeField]
    private float minDistance = 35.0f;
    [SerializeField]
    private float distance = 0.0f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition.x = ship.transform.position.x;
        targetPosition.y = verticalD;
        targetPosition.z = ship.transform.position.z - horizontalD;
    }

	void Update ()
    {
        distance = (Vector3.Distance(transform.position, ship.transform.position) - minDistance); // mindistance will make sure that the camera slows down when it is close to the ship

        targetPosition.x = ship.transform.position.x;
        targetPosition.z = ship.transform.position.z - horizontalD;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ship.transform.position - transform.position), 1f * Time.deltaTime);
    }
}
