using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    [SerializeField] private GameObject ship;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 destination;
    [SerializeField] private int state = 0;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool laserIsOn = false;

    private Vector3 origin;
    private float timer;
    // 0 = idle, 1 = searchForTarget, 2 = patrolArond, 3 = gotoDestination, 4 = attackTarget, 5 = returnHome


	// Use this for initialization
	void Start ()
    {
        ship = transform.gameObject;
        rb = ship.gameObject.GetComponent<Rigidbody>();
        destination = ship.transform.position;
        origin = ship.transform.position;
        target = null;
        timer = Time.time;
        state = 0;

        laser = transform.gameObject.GetComponent<LineRenderer>();
        laser.SetWidth(0.2f, 0.2f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        laserIsOn = (state == 4) ? true : false;
        DrawLaser();

        UpdateState();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case 0:
                {
                    if (Time.time > timer)
                    {
                        timer = Time.time + 1f;
                        state = 1;
                        if (Vector3.Distance(origin, ship.transform.position) > 50f) state = 5;
                    }

                }; break;
            case 1:
                {
                    FindTarget();
                    if (target == null) state = 0;
                    else if (target.name == "SpaceShip") state = 4;
                    else
                    {
                        destination = ship.transform.position;
                        state = 2;
                    }
                }; break;
            case 2:
                {
                    if (destination != null)
                    {
                        if (Vector3.Distance(ship.transform.position, destination) > 10f) state = 3;
                        else PatrolArea();
                    }
                    else state = 0;

                }; break;
            case 3:
                {
                    if (destination != null) GoToDestination();
                    else state = 0;

                    FindTarget();
                    if (target != null)
                        if (target.name == "SpaceShip") state = 4;
                }; break;
            case 4:
                {
                    if (Vector3.Distance(target.transform.position, ship.transform.position) > 50f)
                    {
                        target = null;
                        state = 1;
                    }
                    else AttackTarget();
                }; break;
            case 5:
                {
                    destination = origin;
                    state = 3;
                }; break;
        }

    }

    private void FindTarget()
    {
        if (Vector3.Distance(player.transform.position, ship.transform.position) < 50f)
            target = player;
        else
            target = null;
    }

    private void PatrolArea()
    {

    }

    private void GoToDestination()
    {
        ship.transform.position = Vector3.MoveTowards(ship.transform.position,destination, 5f * Time.deltaTime);
        ship.transform.LookAt(destination);
    }

    private void AttackTarget()
    {
        DrawLaser();
        ship.transform.position = Vector3.MoveTowards(ship.transform.position, target.transform.position, 5f * Time.deltaTime);
        ship.transform.LookAt(target.transform.position);
    }






    [SerializeField]
    private GameObject laserGo;
    [SerializeField]
    private LineRenderer laser;
    [SerializeField]
    private Material activeLaserColor;
    [SerializeField]
    private Material idleLaserColor;

    private GameObject laserTarget = null;

    RaycastHit hitInfo;
    Ray detectObject;
    bool hit = false;

    private void DrawLaser()
    {
        if (laserIsOn) // if the laser is on
        {
            // find target -------------------
            detectObject = new Ray(laserGo.transform.position, laserGo.transform.right * 50f);
            hit = Physics.Raycast(detectObject, out hitInfo);

            if (hit && Vector3.Distance(laserGo.transform.position, hitInfo.point) < 50f)
                laserTarget = hitInfo.transform.gameObject;
            else
                laserTarget = null;
            //---------------------------------

            laser.SetPosition(0, laserGo.transform.position);                                                                      // set line start position

            if (laserTarget != null)                                                                                                 // if there is something infront
            {
                if (Vector3.Distance(laserGo.transform.position, hitInfo.point) < 50f)                           // and in range
                {
                    laser.SetPosition(1, hitInfo.point);                                                                        // set the laser to point to that area
                    laser.GetComponent<Renderer>().material = (laserTarget == player) ? activeLaserColor : idleLaserColor;  // set the color of the laser
                }
            }
            else                                                                                                                // if there is nothing in front
            {
                laser.SetPosition(1, (laserGo.transform.position + laserGo.transform.right * 50f));                   // set the laser to max range
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
