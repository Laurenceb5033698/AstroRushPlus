using System.Collections;
using UnityEngine;

public class Script_VoidZone_Radius_Growth : MonoBehaviour
{
    public AnimationCurve curveSpawn; // curve to grow the radius on spawn, this must go from 0 to 1 to keep the correct scale with curveGrow
    public AnimationCurve curveGrow; // curve to grow the radius after growing on spaw, this growth is its growth over lifetime, curve must start at a value of 1 to keep the correct scale
    public AnimationCurve curveShrink; // curve to shrink the radius at the end of its life, curve must end at a value of 0 to keep the correct scale
    public float intensity = 1f; // multiplier of the growth amount 
    public float durationGrow = 5f; // how long the radius will grow for
    public float durationSpawn = 1f; // how long it takes the radius to grow to its starting scale on spawn
    public float durationShrink = 1f; // how long it takes the radius to shrink at the end of its life
    private float lifetime; // the total lifetime of the voidzone before it starts its despawning sequence, the duration should be the length of time of the spawn and growth sequences combind

    void Start()
    {
        lifetime = durationSpawn + durationGrow;
        StartCoroutine(Spawn());
        StartCoroutine(EndOfLife());
    }


    IEnumerator Spawn()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationSpawn)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveSpawn.Evaluate(elapsedTime / durationSpawn);
            float growthIntensity = strength * intensity;
            transform.localScale = scale * growthIntensity;
            yield return null;
        }

        yield return new WaitForSeconds(durationSpawn);
        StartCoroutine(Growing());

    }

    IEnumerator Growing()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationGrow)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveGrow.Evaluate(elapsedTime / durationGrow);
            float growthIntensity = strength * intensity;
            transform.localScale = scale * growthIntensity;

            yield return null;
        }
    }

    IEnumerator EndOfLife()
    {
        yield return new WaitForSeconds(lifetime);
        StartCoroutine(DespawnRadius());
    }

    IEnumerator DespawnRadius()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationShrink)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveShrink.Evaluate(elapsedTime / durationShrink);
            float growthIntensity = strength * intensity;
            transform.localScale = scale * growthIntensity;
            yield return null;
        }
    }
}
