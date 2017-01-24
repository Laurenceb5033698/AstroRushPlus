using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

    public GameObject ship;   //This pirate ship
    public Rigidbody rb; 	// ship's rigid body
    public ShipStats stats;
    //public AnimateThrusters thrusters;
    public Vector3 Destination;
    public Vector3 PatrolSourcePoint;

    public GameObject Target;

    private float rotFix = 0f;
    private float scanCooldown = 0.0f;

    private bool passive = true;

    // Use this for initialization
    void Start()
    {
        PatrolSourcePoint = ship.transform.position;
        Target = null;
        Destination = ship.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //check state
        if (passive)
        {
            //~every 1s look for player
            //  if found passive=false (aggressive)
            //while passive
            //  gotoDestination()
            //  
            if (scanCooldown < Time.time)
            {
                Target = findTarget();//this needs to happen less often
                scanCooldown = Time.time + 2f;//every 2 seconds
            }
            if (Target == null)
            {
                Debug.Log("No Target Found, am still null");
                //Find random nearby position to goto
                if (Vector3.Distance(Destination, ship.transform.position) < 5f)//close enough
                {
                    createDestination();
                    //should pick next point from patrol array instead of random
                }
            }
            else
            {
                Destination = Target.transform.position;
                passive = false;
            }
        }
        //aggressive
        else    //assume target = player's ship
        {
            if (Vector3.Distance(Destination, ship.transform.position) < 150f)
            {//still within chase range?
                Destination = Target.transform.position;
            }
            else
            {
                //too far away. so go passive
                Destination = PatrolSourcePoint;
                passive = true;
                Target = null;
            }
        }

        //the perform move
        if (Destination != ship.transform.position)
        {
            GotoDestination();
        }
    }

    void GotoDestination()
    {//Controls ship movement
        //--------------------------------------------------
        //CorrectShipTransforms();

        if (Target != null)
            Destination = Target.transform.position;


        //First aims the pirate ship at the destination (within a few degrees of accuracy)
        Vector3 targetDir = Destination - ship.transform.position;
        Vector3 controlDir = targetDir.normalized;//use ControlDir to move the ship towards the destination(instead of controller input)
        //Quaternion mRotation = Quaternion.LookRotation(targetDir);

        //ship.transform.rotation = Quaternion.RotateTowards(ship.transform.rotation, mRotation, Time.deltaTime * 42f);
       

        float angle = Vector3.Angle(controlDir, ship.transform.right);
        Vector3 cross = Vector3.Cross(controlDir, ship.transform.right);
        if (cross.y < 0) angle = -angle;
        angle = angle / -180;
        //Debug.Log(rb.angularVelocity.magnitude);
        rb.AddTorque(Vector3.up * ((angle) * stats.GetRotSpeed()) * Time.deltaTime);

        //then causes the pirate ship to fly forwards until destination reached.
        //if (Quaternion.Dot(mRotation, ship.transform.rotation) > 0.2)
        //{
            if (targetDir.magnitude <= 30)
            {
                rb.AddForce(ship.transform.forward * stats.GetMainThrust() * Time.deltaTime);
            }
            else
            {
                rb.AddForce(ship.transform.right * stats.GetMainThrust() * Time.deltaTime);
            }
        //}
        //once destination is (approximately) reached, set destination = pirateshipo.transform.position.
        if (targetDir.magnitude < 2f)
        {
            //destination reached. threfore unset destination 
            Destination = ship.transform.position;
        }
        //if (targetDir.magnitude > 100)
        //{
        //    Target = null;  //too far away, lose target.
        //}
    }

    private void createDestination()
    {
        //creates a vector3 destination for the ai to travel to.
        //TODO? //should not be occupied by another object.
        Destination = PatrolSourcePoint + new Vector3(Random.Range(-1, 1) * Random.Range(8f, 20f), 0f, Random.Range(-1, 1) * Random.Range(8f, 20f));
    }
    public void createDestination(Vector3 pos)
    {
        Destination = pos;
    }

    GameObject findTarget() //Looks for certain objects nearby
    {
        Collider[] targetColliders = Physics.OverlapSphere(ship.transform.position, 20f);
        foreach (Collider col in targetColliders)
        {
            Debug.Log(col.gameObject.name);
            if (col.gameObject.name == "NewShip")
                return col.gameObject;
        }
        return null;
    }
    private void CorrectShipTransforms()
    {
        // Reset unwanted xyz rotation and velocity --------------------------------------------------------------------------------------------
        rb.velocity = new Vector3(rb.velocity.x, 0.00f, rb.velocity.z);
        rb.angularVelocity = new Vector3(0.00f, rb.angularVelocity.y, 0.00f);

        if (rb.velocity.magnitude < 2f && Time.time > rotFix)
        {
            rotFix = Time.time + 1f;
            // THIS CAUSES THE SHIP TO JITTER (<---)
            ship.transform.position = new Vector3(ship.transform.position.x, 0.00000f, ship.transform.position.z); //   <---------
            ship.transform.eulerAngles = new Vector3(0f, ship.transform.eulerAngles.y, 0f); // fix the weird rotation applied to x and z axis   // <--------------
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}
