using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

public class ThrusterManager : MonoBehaviour {

    [SerializeField] private List<VisualEffect> thrusters = new List<VisualEffect>();
    [SerializeField] private List<VisualEffect> alternate = new List<VisualEffect>();
    [SerializeField] private List<TrailRenderer> maintrails = new List<TrailRenderer>();
    [SerializeField] private List<TrailRenderer> altTrails = new List<TrailRenderer>();

    private PlayerController playership;

    private bool stateThruster; // master state for normal thruster effects
    private bool stateAlternate;// for any ship specific or ability specific effects.

	void Start ()
    {
        playership = GetComponentInParent<PlayerController>();
        stateThruster = false;
        stateAlternate = false;
    }
	
	void Update ()
    {
        GetInputs();
        //if player movement input detected play all normal thrusters
        MainEffects();
        //plays all alternate vfx when ship specific conditions are met (condition determined in player controller)
        AlternateEffects();
	}

    private void GetInputs()
    {
        //get inputs to set state for movement
        stateThruster = playership.MainShipVFX();

        //Alternate effects set by ship controller
        stateAlternate = playership.AlternateShipVFX();
    }
    private void MainEffects()
    {
        //emit trails & vfx on normal movement
        UpdateVisualEffects(thrusters, stateThruster);
        UpdateTrailRenderers(maintrails, stateThruster);
    }
    private void AlternateEffects()
    {
        //emit trails & vfx on Alternate movement
        UpdateVisualEffects(alternate, stateAlternate);
        UpdateTrailRenderers(altTrails, stateAlternate);
    }

    //Trail handling
    private void UpdateTrailRenderers(List<TrailRenderer> _trails, bool _state)
    {
        //controls if trail is emitting or not
        foreach (TrailRenderer _trail in _trails)
        {
            TrailSetState(_trail, _state);
        }
    }
    private void TrailSetState(TrailRenderer _trail, bool _state)
    {
        _trail.emitting = _state;
    }

    //vfx handling
    private void UpdateVisualEffects( List<VisualEffect> _effectslist, bool _state)
    {
        //controls play/stop state of all vfx in the given list.
        foreach (VisualEffect vfx in _effectslist)
        {
            VFXSetState(vfx, _state);
        }
    }   
    private void VFXSetState(VisualEffect _vfx, bool _state)
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
