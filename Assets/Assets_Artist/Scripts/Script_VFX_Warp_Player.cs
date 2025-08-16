using System.Collections;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.VFX;

public class Script_VFX_Warp_Player : MonoBehaviour
{
    public VisualEffect WarpSpeedVFX; // The warp debris VFX
    public MeshRenderer WarpCylinder; // The warp debris cylinder mesh
    public float WarpCylinderDelay = 2f; // Delay for the cylinder to appear, as it has no distance to travel unlike the VFX
    public float warpRate = 0.25f; // How fast the warp visual increases
    private bool warpActive;
    void Start()
    {
        WarpSpeedVFX.Stop();
        WarpSpeedVFX.SetFloat("WarpAmount", 0);

        WarpCylinder.material.SetFloat("_Active", 0);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            warpActive = true;
            StartCoroutine(ActivateParticles());
            StartCoroutine(ActivateCylinder());
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            warpActive = false;
            StartCoroutine(ActivateParticles());
            StartCoroutine(ActivateCylinder());

        }
    }

    IEnumerator ActivateParticles()
    {
        if (warpActive)
        {
            WarpSpeedVFX.Play();

            float amount = WarpSpeedVFX.GetFloat("WarpAmount");
            while (amount < 1 & warpActive)
            {
                amount += warpRate;
                WarpSpeedVFX.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.1f);
            }
            if (amount >= 1 + warpRate)
            {
                amount = 1;
                WarpSpeedVFX.SetFloat("WarpAmount", amount);
            }
        }
        else
        {
            float amount = WarpSpeedVFX.GetFloat("WarpAmount");
            while (amount > 0 & !warpActive)
            {
                amount -= warpRate;
                WarpSpeedVFX.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.1f);

                if (amount <= 0 + warpRate)
                {
                    amount = 0;
                    WarpSpeedVFX.SetFloat("WarpAmount", amount);
                    WarpSpeedVFX.Stop();
                }
            }
        }
    }

    IEnumerator ActivateCylinder()
    {
        yield return new WaitForSeconds(WarpCylinderDelay);
        if (warpActive)
        {
            float amount = WarpCylinder.material.GetFloat("_Active");
            while (amount < 1 & warpActive)
            {
                amount += warpRate;
                WarpCylinder.material.SetFloat("_Active", amount);
                yield return new WaitForSeconds(0.1f);
            }
            if (amount >= 1 + warpRate)
            {
                amount = 1;
                WarpCylinder.material.SetFloat("_Active", amount);
            }
        }
        else
        {
            float amount = WarpCylinder.material.GetFloat("_Active");
            while (amount > 0 & !warpActive)
            {
                amount -= warpRate;
                WarpCylinder.material.SetFloat("_Active", amount);
                yield return new WaitForSeconds(0.1f);

                if (amount <= 0 + warpRate)
                {
                    amount = 0;
                    WarpCylinder.material.SetFloat("_Active", amount);
                }
            }
        }
    }

}
