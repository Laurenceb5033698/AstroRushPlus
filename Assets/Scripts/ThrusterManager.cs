using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;

public class ThrusterManager : MonoBehaviour {

    [SerializeField] private List<VisualEffect> thrusters = new List<VisualEffect>();
    [SerializeField] private List<VisualEffect> alternate = new List<VisualEffect>();
    private Inputs controls;
    private PlayerController playership;

    private bool stateThruster; // master state for normal thruster effects
    private bool stateAlternate;// for any ship specific or ability specific effects.

	void Start ()
    {
        controls = GameManager.instance.GlobalInputs;
        playership = GetComponentInParent<PlayerController>();
        stateThruster = false;
        stateAlternate = false;
    }
	
	void Update ()
    {
        GetInputs();
        //if player movement input detected play all normal thrusters
        UpdateVisualEffects(thrusters, stateThruster);
        //plays all alternate vfx when ship specific conditions are met (condition determined in player controller)
        UpdateVisualEffects(alternate, stateAlternate);
	}

    private void GetInputs()
    {
        //get inputs to set state for movement
        stateThruster = controls.LeftAnalogueInUse;

        //Alternate effects set by ship controller
        stateAlternate = playership.AlternateShipVFX();
    }

    private void UpdateVisualEffects( List<VisualEffect> _effectslist, bool _state)
    {
        //controls play/stop state of all vfx in the given list.
        foreach (VisualEffect vfx in _effectslist)
        {
            SetState(vfx, _state);
        }
    }   
    
    private void SetState(VisualEffect _vfx, bool _state)
    {
        if (_state)
        {
            _vfx.Play();
        }
        else
        {
            _vfx.Stop();
        }
    }
}
