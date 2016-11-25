using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{


	public ShipStats stats;
	public Inputs controls;

    public GameObject turret;
    public GameObject TGun;
    private LineRenderer laser;

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
	void Update () 
    {
		DrawLaser();
        if (stats.LaserState) MineAsteroid();
	}

    private void DrawLaser()
    {
		laser.SetPosition(0, TGun.transform.position);

		GameObject temp = FindObject ();
        Vector3 tempPos = Vector3.zero;
        Color tempColor = Color.red;


		if (stats.LaserState)
        {
			if (temp != null && Vector3.Distance (TGun.transform.position, hitInfo.point) < stats.GetLaserRange())
            {
				laser.SetPosition(1, hitInfo.point);
                laser.GetComponent<Renderer>().material.color = (temp.name == "Asteroid") ? stats.GetLaserColor(false) : stats.GetLaserColor(true);    
            }
            else
            {
				laser.SetPosition(1, TGun.transform.position + -TGun.transform.up * stats.GetLaserRange());
                laser.GetComponent<Renderer>().material.color = stats.GetLaserColor(true); // idle laser
            }
        }
        else
        {
            laser.SetPosition(1, TGun.transform.position);
        }
        
        //laser.SetPosition(1, tempPos);
        //laser.GetComponent<Renderer>().material.color = tempColor; // idle laser
        
        

    }

	private GameObject FindObject()
	{
		detectObject = new Ray(TGun.transform.position, -TGun.transform.up * stats.GetLaserRange());
		hit = Physics.Raycast(detectObject, out hitInfo);

		if (stats.LaserState) 
		{
			if (hit && Vector3.Distance (TGun.transform.position, hitInfo.point) < stats.GetLaserRange()) {
				return hitInfo.transform.gameObject;
			}
		}
		return null;
	}

	public void MineAsteroid()
	{
		GameObject temp = FindObject ();


		if (temp != null) 
		{
			if (temp.name == "Asteroid") 
			{
				if (temp.GetComponent<Asteroid> ().GetOreAmountLeft () > 0.1f) 
				{
					if (stats.ShipCargo < stats.GetMaxCargoSpace()) 
					{
						stats.ShipCargo = temp.GetComponent<Asteroid> ().MineOre (stats.GetLaserSpeed());
					}
				} 
				else
					temp.GetComponent<Asteroid> ().DestroyAsteroid ();
			}
		}
	}

}
