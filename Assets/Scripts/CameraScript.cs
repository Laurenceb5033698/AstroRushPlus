using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GameObject target;
    private Vector3 offset = new Vector3(0,70,-40);
    private float cameraSpeed;

    void Start()
    {

    }

	void Update ()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, cameraSpeed * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
    }

    public void SetTarget(GameObject ps)
    {
        if (ps.GetComponent<ShipStats>() != null)
        {
            target = ps;
            cameraSpeed = ps.GetComponent<ShipStats>().GetMainThrust() + 100;
        }
        else
        {
            target = null;
            cameraSpeed = 0;
        }
    }
}
