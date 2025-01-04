
using System.Collections;
using UnityEngine;

public class Script_Camera_Shake : MonoBehaviour {
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;
    public float intensity = 1f;

    void Update() {
        if (start) {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking() {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            float shakeIntensity = strength * intensity;
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;
            yield return null;
        }

        transform.position = startPosition;
    }
}
