using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {


	public Inputs controls;
    private bool laserIsOn = false;
    const float laserRange = 50f;
    public GameObject turret;
    public GameObject TGun;
    private LineRenderer laser;


	private GameObject target;

	RaycastHit hitInfo;
	Ray detectObject;
	bool hit = false;


	// Use this for initialization
	void Start () 
    {
        laser = transform.gameObject.AddComponent<LineRenderer>();
        laser.SetWidth(0.2f, 0.2f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckInputs();
	}

    private void CheckInputs()
    {
		if (controls.GLaser)
        {
            DrawLaser(0);
            laserIsOn = true;
        }
		else if (controls.RLaser)
        {
            DrawLaser(1);
            laserIsOn = true;
        }
        else
        {
            DrawLaser(2);
            laserIsOn = false;
        }
    }

    private void DrawLaser(int button)
    {
		/*
        if (button == 0)
            laser.GetComponent<Renderer>().material.color = Color.green;
        else if (button == 1)
            laser.GetComponent<Renderer>().material.color = Color.red;
		*/

		laser.SetPosition(0, TGun.transform.position);
		FindObject ();


        if (laserIsOn)
        {
			if (target != null && Vector3.Distance (TGun.transform.position, hitInfo.point) < laserRange)
            {
				laser.SetPosition(1, hitInfo.point);
				if (target.name == "Asteroid")
					laser.GetComponent<Renderer> ().material.color = Color.green;
				else {
					laser.GetComponent<Renderer>().material.color = Color.red;
				}
            }
            else
            {
                laser.SetPosition(1, TGun.transform.position + -TGun.transform.up * laserRange);
				laser.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        else
        {
            laser.SetPosition(1, TGun.transform.position);
        }
    }

	private void FindObject()
	{
		detectObject = new Ray(TGun.transform.position, -TGun.transform.up * laserRange);
		hit = Physics.Raycast(detectObject, out hitInfo);

		if (laserIsOn) 
		{
			if (hit && Vector3.Distance (TGun.transform.position, hitInfo.point) < laserRange) {
				target = hitInfo.transform.gameObject;
			}
		}
	}

	public GameObject GetTarget()
	{
		FindObject ();
		return target;
	}

}
