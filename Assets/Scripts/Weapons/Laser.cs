using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
    [SerializeField] private ShipStats stats;
    [SerializeField] private Inputs controls;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private GameObject target = null;

    [SerializeField] private Material activeLaserColor;
    [SerializeField] private Material idleLaserColor;

	RaycastHit hitInfo;
	Ray detectObject;
	bool hit = false;

    private Vector3 targetPos;

    // Use this for initialization
    void Start () 
    {
		laser.SetWidth(stats.GetLaserWidth(), stats.GetLaserWidth());
        laser.GetComponent<Renderer>().material = idleLaserColor;
    }
	
	// Update is called once per frame
    void Update()
    {
        DrawLaser();
    }


    private void DrawLaser()
    {
        //targetPos = transform.position + transform.TransformDirection(new Vector3(controls.rightY, controls.yawAxis, 0)) * stats.GetLaserRange();
        targetPos = transform.position + transform.right * stats.GetLaserRange();
        laser.SetPosition(0, transform.position);

        detectObject = new Ray(transform.position, (targetPos - transform.position).normalized * stats.GetLaserRange());
        hit = Physics.Raycast(detectObject, out hitInfo);

        if (hit && Vector3.Distance(transform.position, hitInfo.point) < Vector3.Distance(transform.position, targetPos) && hitInfo.transform.gameObject.tag != "PlayerShip")
        {
            target = hitInfo.transform.gameObject;
            laser.SetPosition(1, hitInfo.point);

            if (target.tag == "Asteroid")
            {
                target.GetComponent<Asteroid>().TakeDamage(stats.GetLaserDamage() * Time.deltaTime);
                laser.GetComponent<LineRenderer>().material = activeLaserColor;
            }
            else if (target.tag == "GeneratorShield")
            {
                target.gameObject.GetComponentInParent<Generator>().TakeDamage(stats.GetLaserDamage() * Time.deltaTime);
                laser.GetComponent<LineRenderer>().material = activeLaserColor;
            }
            else if (target.tag == "EnemyShip")
            {
                target.gameObject.GetComponentInParent<EnemyAI>().TakeDamage(stats.GetLaserDamage() * Time.deltaTime);
                laser.GetComponent<LineRenderer>().material = activeLaserColor;
            }
            else
            {
                laser.GetComponent<LineRenderer>().material = idleLaserColor;
            }
        }
        else
        {
            target = null;
            laser.SetPosition(1, targetPos);
            laser.GetComponent<LineRenderer>().material = idleLaserColor;
        }
    }
}
