using UnityEngine;
using System.Collections;

public class AnimateThrusters : MonoBehaviour {

	private bool ThrustersEnabled = true;
	public Inputs controls;

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

	public ParticleSystem FRT; // front right top thruster
	public ParticleSystem RRT; // rear right top
	public ParticleSystem RRB;
	public ParticleSystem FRB;

	public ParticleSystem FLB;
	public ParticleSystem RLB;
	public ParticleSystem RLT;
	public ParticleSystem FLT;


	void Start () 	// Use this for initialization
    {
        InitaliseThrusters();
	}
	void Update () 	// Update is called once per frame
    {
		SetThrusterSpeed ();

		if (ThrustersEnabled)
			AnimateThrust ();
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

		// --------- Top Thrusters -------------------------
		SetEmission(FRT,false);
		SetEmission(RRT,false);
		SetEmission(RLT,false);
		SetEmission(FLT,false);

		// -------- Bottom Thrusters -----------------------
		SetEmission(RRB,false);
		SetEmission(FRB,false);
		SetEmission(FLB,false);
		SetEmission(RLB,false);

    }
	private void SetEmission(ParticleSystem p, bool v)
	{
		ParticleSystem.EmissionModule temp;
		temp = p.emission;
		temp.enabled = v;
	}
	private void SetSpeed(ParticleSystem p,float f)
	{
		p.startSpeed = f;
	}
	public void SetThrusterState(bool v)
	{
		ThrustersEnabled = v;
	}
	private void SetThrusterSpeed()
	{
		float thrustAmount = 50 / 100 * controls.ThrustLevel;

		SetSpeed(fThrust1,thrustAmount);
		SetSpeed(fThrust2,thrustAmount);
		SetSpeed(fThrust3,thrustAmount);
		SetSpeed(fThrust4,thrustAmount);
	}
    
	private void AnimateThrust()
    {
		// ----------------- Forward Thrusters -------------------------------
		bool fThrusters = (controls.ThrustLevel > 0f || controls.forward);
		SetEmission (fThrust1, fThrusters);
		SetEmission (fThrust2, fThrusters);
		SetEmission (fThrust3, fThrusters);
		SetEmission (fThrust4, fThrusters);

		// ----------------- Backward Thrusters -------------------------------
		SetEmission (rThrustLeft, controls.backward);
		SetEmission (rThrustRight, controls.backward);

		// ----------------- Left & Right Thrusters -----------------------------------

		if (controls.yawRight || controls.right) 	SetEmission (flThrust, true);
		else 										SetEmission (flThrust, false);

		if (controls.yawLeft || controls.right) 	SetEmission (rlThrust, true);
		else 										SetEmission (rlThrust, false);

		if (controls.yawLeft || controls.left) 		SetEmission (frThrust, true);
		else 										SetEmission (frThrust, false);

		if (controls.yawRight || controls.left) 	SetEmission (rrThrust, true);
		else 										SetEmission (rrThrust, false);

		// -------------------------- Top Thrusters -----------------------------------------------
		if (controls.PitchUp || controls.rollLeft || controls.down) 	SetEmission (RLT, true);
		else 															SetEmission (RLT, false);

		if (controls.PitchUp || controls.rollRight || controls.down) 	SetEmission (RRT, true);
		else 															SetEmission (RRT, false);

		if (controls.PitchDown || controls.rollRight || controls.down) 	SetEmission (FRT, true);
		else 															SetEmission (FRT, false);

		if (controls.PitchDown || controls.rollLeft || controls.down) 	SetEmission (FLT, true);
		else 															SetEmission (FLT, false);

		// -------------------------- Bottom Thrusters ------------------------------------------
		if (controls.PitchDown || controls.rollLeft || controls.up) 	SetEmission (RRB, true);
		else 															SetEmission (RRB, false);

		if (controls.PitchUp || controls.rollLeft || controls.up) 		SetEmission (FRB, true);
		else 															SetEmission (FRB, false);

		if (controls.PitchUp || controls.rollRight || controls.up) 		SetEmission (FLB, true);
		else 															SetEmission (FLB, false);

		if (controls.PitchDown || controls.rollRight || controls.up) 	SetEmission (RLB, true);
		else 															SetEmission (RLB, false);
		// ----------------------------------------------------------------------------------------
    }




}
