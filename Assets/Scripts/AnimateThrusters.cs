using UnityEngine;
using System.Collections;

public class AnimateThrusters : MonoBehaviour
{

	private bool ThrustersEnabled = true;

    [SerializeField]
	private Inputs controls;
    // particle effect for thrusters
    [SerializeField]
    private ParticleSystem fThrust1;        // forward thrust
    [SerializeField]
    private ParticleSystem fThrust2;        // forward thrust
    [SerializeField]
    private ParticleSystem fThrust3;        // forward thrust
    [SerializeField]
    private ParticleSystem fThrust4;        // forward thrust
    [SerializeField]
    private ParticleSystem rThrustLeft;     // backward thrust
    [SerializeField]
    private ParticleSystem rThrustRight;    // backward thrust
    [SerializeField]
    private ParticleSystem flThrust;        // front left thrust
    [SerializeField]
    private ParticleSystem frThrust;        // front right thrust
    [SerializeField]
    private ParticleSystem rlThrust;        // rear left thrust
    [SerializeField]
    private ParticleSystem rrThrust;	    // rear right thrust

	void Start () 	// Use this for initialization
    {
        InitaliseThrusters();
	}
	void Update () 	// Update is called once per frame
    {
	}

    public void UpdateThrusters()
    {
        if (ThrustersEnabled) AnimateThrust();
        else InitaliseThrusters();
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
	private void AnimateThrust()
	{
		// ----------------- Forward Thrusters -------------------------------
		bool fThrusters = (controls.zAxis > 0.1f);
		SetEmission (fThrust1, fThrusters);
		SetEmission (fThrust2, fThrusters);
		SetEmission (fThrust3, fThrusters);
		SetEmission (fThrust4, fThrusters);

        fThrust1.startSpeed = Mathf.Abs(controls.zAxis) * 10;
        fThrust2.startSpeed = Mathf.Abs(controls.zAxis) * 10;

        // ----------------- Backward Thrusters -------------------------------
        SetEmission (rThrustLeft, controls.zAxis < -0.1f);
		SetEmission (rThrustRight, controls.zAxis < -0.1f);

		// ----------------- Left & Right Thrusters -----------------------------------
        SetEmission(flThrust, (controls.yawAxis > 0 || controls.xAxis > 0));
        SetEmission(rlThrust, (controls.yawAxis < 0 || controls.xAxis > 0));
        SetEmission(frThrust, (controls.yawAxis < 0 || controls.xAxis < 0));
        SetEmission(rrThrust, (controls.yawAxis > 0 || controls.xAxis < 0));
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

}
