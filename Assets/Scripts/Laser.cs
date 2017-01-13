using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
    [SerializeField] private ShipStats stats;
    [SerializeField] private Inputs controls;
   

    [SerializeField] private GameObject laserGo;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private GameObject target = null;

    [SerializeField] private Material activeLaserColor;
    [SerializeField] private Material idleLaserColor;

	RaycastHit hitInfo;
	Ray detectObject;
	bool hit = false;

    // Use this for initialization
    void Start () 
    {
        laser = transform.gameObject.AddComponent<LineRenderer>();
		laser.SetWidth(stats.GetLaserWidth(), stats.GetLaserWidth());
	}
	
	// Update is called once per frame
    void Update()
    {
        // new Vector3(controls.rightY, 0, controls.yawAxis)
        laser.SetPosition(0, laserGo.transform.position);
        laser.SetPosition(1, (laserGo.transform.position + transform.TransformDirection(new Vector3(controls.rightY, 0, -controls.yawAxis)) * stats.GetLaserRange()));
        laser.GetComponent<Renderer>().material = idleLaserColor;  
    }


    private void DrawLaser()
    {
        if (stats.LaserState) // if the laser is on
        {
            // find target -------------------
            detectObject = new Ray(laserGo.transform.position, laserGo.transform.right * stats.GetLaserRange());
            hit = Physics.Raycast(detectObject, out hitInfo);

            if (hit && Vector3.Distance(laserGo.transform.position, hitInfo.point) < stats.GetLaserRange())
                target = hitInfo.transform.gameObject;
            else
                target = null;
            //---------------------------------
            


            laser.SetPosition(0, laserGo.transform.position);                                                                      // set line start position

            if (target != null)                                                                                                 // if there is something infront
            {
                if (Vector3.Distance(laserGo.transform.position, hitInfo.point) < stats.GetLaserRange())                           // and in range
                {
                    laser.SetPosition(1, hitInfo.point);                                                                        // set the laser to point to that area
                }
                if (target.name == "Asteroid" && stats.ShipCargo < stats.GetMaxCargoSpace())                                    // if the object is an asteroid and there is free cargo space
                {
                    target.GetComponent<Asteroid>().MineOre(stats.GetLaserSpeed());                                             // decrease ore in asteroid
                    stats.ShipCargo = stats.GetLaserSpeed();                                                                    // add ore to cargo space
                    laser.GetComponent<Renderer>().material = activeLaserColor;
                }
                else if (target.name == "Generator")
                {
                    target.gameObject.GetComponent<Generator>().TakeDamage(20f * Time.deltaTime);
                    laser.GetComponent<Renderer>().material = activeLaserColor;
                }
                else if (target.name == "GeneratorShield")
                {
                    target.gameObject.GetComponentInParent<Generator>().TakeDamage(50f * Time.deltaTime);
                    laser.GetComponent<Renderer>().material = activeLaserColor;
                }
                else if (target.gameObject.GetComponentInParent<EnemyAI>() != null)
                {//then target is an enemy ship
                    target.gameObject.GetComponentInParent<EnemyAI>().TakeDamage(40f * Time.deltaTime);
                    laser.GetComponent<Renderer>().material = activeLaserColor;
                }
                else
                {
                    laser.GetComponent<Renderer>().material = idleLaserColor;
                }
            }
            else                                                                                                                // if there is nothing in front
            {
                Vector3 locVel = transform.InverseTransformDirection(new Vector3(controls.rightY, 0, controls.yawAxis));
                locVel = new Vector3(controls.zAxis * stats.GetMainThrust(), 0, 0);


                //laser.SetPosition(1, (laserGo.transform.position + laserGo.transform.right * stats.GetLaserRange()));                   // set the laser to max range
                laser.SetPosition(1, (laserGo.transform.position + new Vector3(controls.rightY, 0, controls.yawAxis) * stats.GetLaserRange()));
                laser.GetComponent<Renderer>().material = idleLaserColor;                                                       // and change color to idle
            }

        }
        else                                                                                                                    // if the laser is off, turn off the line
        {
            laser.SetPosition(0, laserGo.transform.position);
            laser.SetPosition(1, laserGo.transform.position);
        }
    }
}
