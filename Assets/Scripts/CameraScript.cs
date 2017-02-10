using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GameObject target;
    private Vector3 offset = new Vector3(0,70,-40);
    private float offsetDistance = 0;
    private float cameraSpeed;

    [SerializeField] private float cameraSpeedMultiplier = 1;

    void Start()
    {
        offsetDistance = Vector3.Distance(target.transform.position + offset, target.transform.position);
    }

	void Update ()
    {
        if (target != null)
        {
            //cameraSpeed = Vector3.Distance(transform.position, target.transform.position) - offsetDistance + 5;
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, cameraSpeed * cameraSpeedMultiplier * Time.deltaTime);

            transform.position = target.transform.position + offset; // camera fix - instant camera move 
            transform.LookAt(target.transform.position);
        }
    }

    public void SetTarget(GameObject ps)
    {
        if (ps.GetComponent<ShipStats>() != null)
        {
            target = ps;
            //cameraSpeed = ps.GetComponent<ShipStats>().GetMainThrust() + 100;
        }
        else
        {
            target = null;
            cameraSpeed = 0;
        }
    }
}
