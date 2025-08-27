using System.Collections;
using UnityEngine;

public class Script_VoidZone_Epicentre : MonoBehaviour
{
    public AnimationCurve curveGrow; // curve to grow the radius at the start of its life, curve must start at a value of 1 to keep the correct scale
    public AnimationCurve curvePulse; // curve to shrink the radius at the end of its life, curve must end at a value of 0 to keep the correct scale
    public float intensity = 1f; // multiplier of the growth amount 
    public float durationSpawn = 1f; // how long the radius will grow for at the start of its lifetime, this is to grow the object from small to its correct scale
    public float durationPulse = 5f; // how long the epicentre should be pulsing for, this is just a viusal effect to make it look more interesting, but the collision will scale with it to ensure correct interactions with projectiles
    private float lifetime; // the total lifetime of the voidzone before it starts its despawning sequence
    public float health = 3; // how many shots it takes to destroy the epdicentre, and therefore the whole voidzone 

    void Start()
    {
        lifetime = durationPulse + durationSpawn;
        StartCoroutine(Growing());
        StartCoroutine(StartOfLife());
    }


    IEnumerator Growing()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationSpawn)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveGrow.Evaluate(elapsedTime / durationSpawn);
            float growthIntensity = strength * intensity;
            transform.localScale = scale * growthIntensity;

            yield return null;
        }
    }

    IEnumerator StartOfLife()
    {
        yield return new WaitForSeconds(durationSpawn);
        StartCoroutine(Pulsing());
    }

    IEnumerator Pulsing()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationPulse)
        {
            elapsedTime += Time.deltaTime;
            float strength = curvePulse.Evaluate(elapsedTime / durationPulse);
            float growthIntensity = strength * intensity;
            transform.localScale = scale * growthIntensity;
            yield return null;
        }
    }



    void Update()
    {
        
    }
}
