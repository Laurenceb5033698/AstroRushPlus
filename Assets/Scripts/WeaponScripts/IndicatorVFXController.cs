using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class IndicatorVFXController : MonoBehaviour
{

    //controller is sat on the indicator holder, parent to all vfx related to weapon vfx
    //when shoot happens, controller.shoot tells child vfx to play, or sets isfriing for child vfx


    //store children vfx
    //List<VisualEffect> effects;


    //needs support for setting certain specific states.
    //Charging
    //reloading
    //ramping %
    //burnout % /cooldown %
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //called at start of firing.
    public void ShootVFX(bool _firing)
    {
        SetFiring(_firing);
    }

    //some might want to be played in full.
    private void PlayVFX(bool _playing)
    {
        VisualEffect[] vfxlist = GetComponentsInChildren<VisualEffect>();
        foreach (VisualEffect vfx in vfxlist)
        {
            if(_playing)
                vfx.Play();
            else
                vfx.Stop();
        }
    }

    //set isfiring
    private void SetFiring(bool _firing)
    {
        VisualEffect[] vfxlist = GetComponentsInChildren<VisualEffect>();
        foreach (VisualEffect vfx in vfxlist)
        {
            if (vfx.HasBool("IsFiring?"))
                vfx.SetBool("IsFiring?", _firing);
        }
    }

}
