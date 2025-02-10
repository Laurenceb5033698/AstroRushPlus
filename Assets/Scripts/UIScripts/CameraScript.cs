using UnityEngine;
using System.Collections;

/// <summary>
/// Tracking Camera and Camera Shake.
/// </summary>
public class CameraScript : MonoBehaviour {

    //track player
	private GameObject followTarget;
    [SerializeField] private Vector3 positionOffset = new Vector3(0,70,-40);
    //focus offset adjusts camera lookat, so target object is not directly centred onscreen
    [SerializeField] private Vector3 focusOffset = new Vector3(0, 0, 0);


    //screen shake
    [SerializeField] private GameObject shakeObject;
    public AnimationCurve curve;
    [Range(0.1f,5)]     public float duration = 1f;
    [Range(0.01f,100)]  public float intensity = 1f;
    private float shakeProgress = 0f;

    //testing
    public bool testShake = false;



    //camera object follows player per update.
    //if screen shake is applied, camera is vibrated around main tracking object.

    //shaking is initiated when an event is called to add some shake amount

    //shake amount has duration and intensity

    //multiple events can be called to add intensity



    void Update ()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.transform.position + positionOffset;
            transform.LookAt(followTarget.transform.position+ focusOffset);
        }

        if (shakeObject != null && shakeProgress > Time.time)
        {
            StartCoroutine(Shaking());
        }
        //testing shake
        if (testShake)
        {
            testShake = false;
            shakeProgress = Time.time + duration;
        }
    }

    public void SetTarget(GameObject ps)
    {  
        followTarget = ps;
    }

    IEnumerator Shaking()
    {
        float percentComplete = 0f;
        while (shakeProgress > Time.time)
        {
            percentComplete = (duration-(shakeProgress - Time.time)) / duration;
            float strength = curve.Evaluate(percentComplete);
            float shakeIntensity = strength * intensity;
            shakeObject.transform.localPosition = Random.insideUnitSphere * shakeIntensity;
            yield return null;
        }
        shakeObject.transform.localPosition = Vector3.zero;
    }
}
