using UnityEngine;
using System.Collections;

public class AnimateThrusters : MonoBehaviour {

	private bool ThrustersEnabled = true;
	public Inputs controls;
	//public GameObject ship;

    // particle effect for thrusters
    public ParticleSystem fThrust1; 	// forward thrust
	public ParticleSystem fThrust2; 	// forward thrust
	public ParticleSystem fThrust3; 	// forward thrust
	public ParticleSystem fThrust4; 	// forward thrust

    public ParticleSystem rThrustLeft; 	// backward thrust
	public ParticleSystem rThrustRight; 	// backward thrust


    public ParticleSystem flThrust; // front left thrust
    public ParticleSystem frThrust; // front right thrust
    public ParticleSystem rlThrust;	// rear left thrust
    public ParticleSystem rrThrust;	// rear right thrust

	//private Vector3 prevPos;


	void Start () 	// Use this for initialization
    {
        InitaliseThrusters();
		//prevPos = ship.transform.position;

	}
	void Update () 	// Update is called once per frame
    {
		if (ThrustersEnabled)
			AnimateThrust2 ();
		else
			InitaliseThrusters ();
	}

    private void InitaliseThrusters()
    {
		// ---------- Rear Thrusters -----------------------
		SetEmission(fThrust1,false);
		SetEmission(fThrust2,false);
		SetEmission(fThrust3,false);
		SetEmission(fThrust4,false);

		// ---------- Forward Thrusters ---------------------
		SetEmission(rThrustLeft,false);
		SetEmission(rThrustRight,false);

		// --------- Side Thrusters -------------------------
		SetEmission(flThrust,false);
		SetEmission(frThrust,false);
		SetEmission(rlThrust,false);
		SetEmission(rrThrust,false);
    }
	private void SetEmission(ParticleSystem p, bool v)
	{
		ParticleSystem.EmissionModule temp;
		temp = p.emission;
		temp.enabled = v;
	}
	public void SetThrusterState(bool v)
	{
		ThrustersEnabled = v;
	}
    
	private void AnimateThrust()
    {
		// ----------------- Forward Thrusters -------------------------------
		bool fThrusters = (controls.zAxis > 0);
		SetEmission (fThrust1, fThrusters);
		SetEmission (fThrust2, fThrusters);
		SetEmission (fThrust3, fThrusters);
		SetEmission (fThrust4, fThrusters);

		// ----------------- Backward Thrusters -------------------------------
		SetEmission (rThrustLeft, controls.zAxis < 0);
		SetEmission (rThrustRight, controls.zAxis < 0);

		// ----------------- Left & Right Thrusters -----------------------------------

		if (controls.yawAxis > 0 || controls.xAxis > 0)
			SetEmission (flThrust, true);
		else 										
			SetEmission (flThrust, false);

		if (controls.yawAxis < 0 || controls.xAxis > 0)
			SetEmission (rlThrust, true);
		else 
			SetEmission (rlThrust, false);

		if (controls.yawAxis < 0 || controls.xAxis < 0)
			SetEmission (frThrust, true);
		else 
			SetEmission (frThrust, false);

		if (controls.yawAxis > 0 || controls.xAxis < 0) 	
			SetEmission (rrThrust, true);
		else 										
			SetEmission (rrThrust, false);
    }

	private void AnimateThrust2()
	{
		/*
		// ship variables
		Vector3 cPos = ship.transform.position;
		//Vector3 vel = (cPos - prevPos) / Time.deltaTime;
		Vector3 mDir = cPos - prevPos;
		Vector3 dir = ship.transform.right;
		float angle = ship.transform.localEulerAngles.y;

		prevPos = cPos;

		// thruster variables
		//Debug.Log("Thruster FL: "+flThrust.transform.forward);
		//Debug.Log("Thruster FL: "+Vector3.Angle(ship.transform.right,flThrust.transform.forward));
		Debug.Log("Thruster FL: "+Vector3.Angle(ship.transform.right,mDir));
		*/

		// ----------------- Forward Thrusters -------------------------------
		bool fThrusters = (controls.zAxis > 0);
		SetEmission (fThrust1, fThrusters);
		SetEmission (fThrust2, fThrusters);
		SetEmission (fThrust3, fThrusters);
		SetEmission (fThrust4, fThrusters);

		// ----------------- Backward Thrusters -------------------------------
		SetEmission (rThrustLeft, controls.zAxis < 0);
		SetEmission (rThrustRight, controls.zAxis < 0);

		// ----------------- Left & Right Thrusters -----------------------------------

		if (controls.yawAxis > 0 || controls.xAxis > 0)
			SetEmission (flThrust, true);
		else 										
			SetEmission (flThrust, false);

		if (controls.yawAxis < 0 || controls.xAxis > 0)
			SetEmission (rlThrust, true);
		else 
			SetEmission (rlThrust, false);

		if (controls.yawAxis < 0 || controls.xAxis < 0)
			SetEmission (frThrust, true);
		else 
			SetEmission (frThrust, false);

		if (controls.yawAxis > 0 || controls.xAxis < 0) 	
			SetEmission (rrThrust, true);
		else 										
			SetEmission (rrThrust, false);
	}

}
