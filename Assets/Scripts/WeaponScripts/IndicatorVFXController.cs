using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class IndicatorVFXController : MonoBehaviour
{
    //controller is sat on the indicator holder, parent to all vfx related to weapon vfx
    //when shoot happens, controller.shoot tells child vfx to play, or sets isfriing for child vfx

    //store children vfx
    [SerializeField] List<VisualEffect> vfxFiring;
    [SerializeField] List<VisualEffect> vfxCharge;
    [SerializeField] List<VisualEffect> vfxRamping;
    [SerializeField] List<VisualEffect> vfxReloading;
    [SerializeField] List<VisualEffect> vfxBurnout;

    //needs support for setting certain specific states.
    //Charging
    //reloading
    //ramping %
    //burnout % /cooldown %
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get each visual component in children
        VisualEffect[] vfxlist = GetComponentsInChildren<VisualEffect>();

        //assign each to a list if they have required properties.
        foreach (VisualEffect vfx in vfxlist)
        {
            if (vfx.HasBool("IsFiring?"))
                vfxFiring.Add(vfx);
            if (vfx.HasFloat("Charge_Speed"))
                vfxCharge.Add(vfx);
            if (vfx.HasFloat("Burnout"))
                vfxBurnout.Add(vfx);
        }
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
    public void PlayChargeVFX(bool _play)
    {
        foreach (VisualEffect vfx in vfxCharge)
        {
            if (_play)
            {
                if (vfx.HasBool("SetAlive"))
                {
                    vfx.SetBool("SetAlive", true);
                }
                vfx.Play();
            }
            else
            {
                vfx.Stop();
                if (vfx.HasBool("SetAlive"))
                {
                    vfx.SetBool("SetAlive", false);
                }
            }
            
        }
    }

    public void ChargeSpeed(float _charge)
    {
        SetChargeSpeed(_charge);
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

    private void SetChargeSpeed(float _chrg)
    {
        foreach ( VisualEffect vfx in vfxCharge)
        {
            vfx.SetFloat("Charge_Speed", _chrg);
        }
    }

    //set isfiring
    private void SetFiring(bool _firing)
    {
        foreach (VisualEffect vfx in vfxFiring)
        {
            vfx.SetBool("IsFiring?", _firing);
        }
    }

}
