using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    [SerializeField]
	private GameObject ship;

    private Vector3 offset = new Vector3(0f,50f,-50f);
    private float cameraSpeed;

    void Start()
    {
        cameraSpeed = ship.transform.GetComponent<ShipStats>().GetMainThrust() + 100f;
    }

	void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, ship.transform.position + offset, cameraSpeed * Time.deltaTime);
        transform.LookAt(ship.transform.position);
    }
}
