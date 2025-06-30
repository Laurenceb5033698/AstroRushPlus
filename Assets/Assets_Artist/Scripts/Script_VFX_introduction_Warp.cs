using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Script_VFX_introduction_Warp : MonoBehaviour
{
    public GameObject GOWarp;
    public VisualEffect VFXWarpDebris1;
    public MeshRenderer WarpCylinder;
    public GameObject WarpCylinderObj;
    public float WarpDuration;
    public float WarpCylinderSpool;
    public float WarpLength;
    public float WarpCylinderDelay;
    private float EndTime = 2f;
    public Boolean WarpAvailable = false;
    public void Start()
    {
        VFXWarpDebris1.Stop();
        WarpCylinder.material.SetFloat("_Active", 0);
        if (WarpAvailable == true)
        {
            StartCoroutine(WarpActive());
            StartCoroutine(WarpCylinderActive());
        }
    }

    public void Update()
    {
        if (WarpAvailable == true)
        {
            WarpLength -= Time.deltaTime;
            if (WarpLength <= 0.0f)
            {
                StartCoroutine(TimerEnded());
            }
        }
    }

    public IEnumerator WarpActive()
    {
        VFXWarpDebris1.Play();
        float currentTime = 0f;
        while (currentTime < WarpDuration)
        {
            float warpRate = Mathf.Lerp(0f, 1f, currentTime / WarpDuration);
            VFXWarpDebris1.SetFloat("WarpAmount", warpRate);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public IEnumerator WarpCylinderActive()
    {
        float currentTimeAlpha = 0f;
        while (currentTimeAlpha < WarpCylinderSpool)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTimeAlpha / WarpCylinderSpool);
            WarpCylinder.material.SetFloat("_Active", alpha);
            currentTimeAlpha += Time.deltaTime;
            yield return null;
        }
      }

    public IEnumerator TimerEnded()
    {
        VFXWarpDebris1.Stop();
        float currentTimeAlpha = 0f;
        while (currentTimeAlpha < 2f)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTimeAlpha / 1f);
            WarpCylinder.material.SetFloat("_Active", alpha);
            currentTimeAlpha += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(EndWarp());
        yield return null;
    }

    public IEnumerator EndWarp()
    {
        yield return new WaitForSeconds(EndTime);
        GOWarp.SetActive(false);
        WarpCylinderObj.SetActive(false);
    }

}
