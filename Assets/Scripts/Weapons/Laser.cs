using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
    private ShipStats stats;
    private LineRenderer laser;
    private GameObject target;

    private Inputs controls;
    [SerializeField] private Material activeLaserColor;
    [SerializeField] private Material idleLaserColor;

	private RaycastHit hitInfo;
	private Ray detectObject;
	private bool hit;

    private Vector3 targetPos;

    // Use this for initialization
    void Start () 
    {
        stats = GetComponentInParent<ShipStats>();
        laser = GetComponent<LineRenderer>();
        controls = GetComponentInParent<Inputs>();

        target = null;
        hit = false;

        laser.startWidth = stats.GetLaserWidth();
        laser.endWidth = stats.GetLaserWidth();
        laser.GetComponent<Renderer>().material = idleLaserColor;
    }
	
	// Update is called once per frame
    void Update()
    {
        DrawLaser();
    }


    private void DrawLaser()
    {
        //new Vector3(controls.yawAxis, 0, controls.rightY).normalized;
        targetPos = transform.parent.position + new Vector3(controls.RightStick.x, 0, controls.RightStick.y).normalized * stats.GetLaserRange();
        //targetPos = transform.position + transform.TransformDirection(new Vector3(controls.rightY, controls.yawAxis, 0).normalized) * stats.GetLaserRange();
        //targetPos = transform.position + transform.right * stats.GetLaserRange();
        laser.SetPosition(0, transform.position);

        detectObject = new Ray(transform.position, (targetPos - transform.position).normalized * stats.GetLaserRange());
        hit = Physics.Raycast(detectObject, out hitInfo);

        if (hit && Vector3.Distance(transform.position, hitInfo.point) < Vector3.Distance(transform.position, targetPos) && hitInfo.transform.gameObject.tag != "PlayerShip")
        {
            target = hitInfo.transform.gameObject;
            laser.SetPosition(1, hitInfo.point);

            //if (target.tag == "Asteroid")
            //{
            //    target.GetComponent<Asteroid>().TakeDamage(stats.GetLaserDamage() * Time.deltaTime);
            //    laser.GetComponent<LineRenderer>().material = activeLaserColor;
            //}
            //else if (target.tag == "GeneratorShield")
            //{
            //    target.gameObject.GetComponentInParent<Generator>().TakeDamage(stats.GetLaserDamage() * Time.deltaTime);
            //    laser.GetComponent<LineRenderer>().material = activeLaserColor;
            //}
            //else 
            if (target.tag == "EnemyShip")
            {
                //target.gameObject.GetComponentInParent<EnemyAI>().TakeDamage(stats.GetLaserDamage() * Time.deltaTime);
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
