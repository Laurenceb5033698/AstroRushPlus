using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour {

    private bool state;
    private ParticleSystem ps;
    
    //private float thrustAmount;

	// Use this for initialization
	void Start ()
    {
        ps = transform.GetComponent<ParticleSystem>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateThruster()
    {
        ParticleSystem.EmissionModule em;
        em = ps.emission;
        em.enabled = state;
    }

    public void SetState(bool s)
    {
        state = s;
        UpdateThruster();
    }
}
