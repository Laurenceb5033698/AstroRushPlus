using System.Collections;
using UnityEngine;

public class Script_VoidZone_Master : MonoBehaviour
{
    public GameObject Epicentre;
    public GameObject Radius;
    public AnimationCurve curveSpawn; // curve to grow the epicentre and radius at the start of its life, curve must start at a value of 1 to keep the correct scale
    public AnimationCurve curvePulse; // curve to pulse the scale of the epicentre during its lifetime
    public AnimationCurve curveGrow; // curve to grow the radius after growing on spaw, this growth is its growth over lifetime, curve must start at a value of 1 to keep the correct scale
    public AnimationCurve curveShrink; // curve to shrink the radius at the end of its life
    public AnimationCurve curveShrinkEpicentre; // curve to shrink the epicentre  at the end of its life
    public float intensity = 1f; // multiplier of the growth amount 
    public float durationSpawn = 1f; // how long the radius will grow for at the start of its lifetime, this is to grow the object from small to its correct scale
    public float durationPulse = 5f; // how long the epicentre should be pulsing for, this is just a viusal effect to make it look more interesting, but the collision will scale with it to ensure correct interactions with projectiles
    public float durationGrow = 5f; // how long the radius will grow for
    public float durationShrink = 1f; // how long it takes the radius to shrink at the end of its life
    private float lifetime; // the total lifetime of the voidzone before it starts its despawning sequence
    public float health = 3; // how many shots it takes to destroy the epdicentre, and therefore the whole voidzone 

    void Start()
    {
        lifetime = durationPulse + durationSpawn;
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
        StartCoroutine(Pulsing());
        StartCoroutine(TakeDamage()); // this is just to check that the correct triggers would occur on taking damage, this should not remain in the start step
    }

    IEnumerator Growing()
    {
        Vector3 scale = Radius.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationGrow)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveGrow.Evaluate(elapsedTime / durationGrow);
            float growthIntensity = strength * intensity;
            Radius.transform.localScale = scale * growthIntensity;

            yield return null;
        }

    }

    IEnumerator Pulsing()
    {
        Vector3 scale = Epicentre.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationPulse)
        {
            elapsedTime += Time.deltaTime;
            float strength = curvePulse.Evaluate(elapsedTime / durationPulse);
            float growthIntensity = strength * intensity;
            Epicentre.transform.localScale = scale * growthIntensity;
            yield return null;
        }
    }

    IEnumerator EndOfLife()
    {
        yield return new WaitForSeconds(lifetime);
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        Vector3 scale = Radius.transform.localScale;
        Vector3 scaleEpicentre = Epicentre.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < durationShrink)
        {
            elapsedTime += Time.deltaTime;
            float strength = curveShrink.Evaluate(elapsedTime / durationShrink);
            float strengthEpicentre = curveShrinkEpicentre.Evaluate(elapsedTime / durationShrink);
            float growthIntensity = strength * intensity;
            float growthIntensityEpicentre = strengthEpicentre * intensity;
            Radius.transform.localScale = scale * growthIntensity;
            Epicentre.transform.localScale = scaleEpicentre * growthIntensityEpicentre;
            yield return null;
        }
        yield return new WaitForSeconds(durationShrink);
        {
            Destroy(gameObject);
        }
    }

    IEnumerator TakeDamage()
    {
        health = (health - 1);
        if (health <= 0)
        {
            StopCoroutine(Growing());
            StopCoroutine(Pulsing());
            StartCoroutine(Despawn());
        }

        yield return null;
    }
    
}
