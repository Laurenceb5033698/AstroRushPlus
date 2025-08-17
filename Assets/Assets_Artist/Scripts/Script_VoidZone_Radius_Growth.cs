using System.Collections;
using UnityEngine;

public class Script_VoidZone_Radius_Growth : MonoBehaviour
{
    public AnimationCurve curveGrow; // curve to grow the radius at the start of its life, curve must start at a value of 1 to keep the correct scale
    public AnimationCurve curveShrink; // curve to shrink the radius at the end of its life, curve must end at a value of 0 to keep the correct scale
    public float intensity = 1f; // multiplier of the growth amount 
    public float duration = 5f; // how long the radius will grow for
    public float durationShrink = 2f; // how long it takes the radius to shrink at the end of its life
    public float lifetime = 5f; // the total lifetime of the voidzone before it starts its despawning sequence

    void Start()
    {
        StartCoroutine(Growing());
        StartCoroutine(EndOfLife());
    }


    IEnumerator Growing()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveGrow.Evaluate(elapsedTime / duration);
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



    void Update()
    {
        
    }
}
