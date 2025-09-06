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
    //camera smooth follow speed.
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float smoothMaxSpeed = 10.0f;
    [SerializeField] private float MaxDistance = 10.0f;

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

    private void LateUpdate()
    {
        FollowTarget();   

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

    /// <summary>
    /// Follow target with offset with smoothing.
    /// </summary>
    private void FollowTarget()
    {
        //moves transform towards position with max offset.

        //if camera falls too far behind target, uses maximum distace to keep up.

        if (followTarget != null)
        {
            Vector3 targetpos = followTarget.transform.position + positionOffset;
            //distance from targetpos
            //float currentDistance = (targetpos - transform.position).magnitude;
            //float percentDistFromWanted = Mathf.Clamp01(currentDistance / MaxDistance);

            Vector3 smoothFollow = Vector3.Lerp(transform.position, targetpos, smoothTime);

            Vector3 focusPoint = followTarget.transform.position + focusOffset;
            Quaternion LookRotation = Quaternion.LookRotation(focusPoint - transform.position, Vector3.up);

            

            transform.position = smoothFollow;
            //transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, smoothTime);

            //does not look at target, rather looks at a point in that sort of direction but is actually relative to camera.
            Vector3 focusPosition = transform.position - positionOffset + focusOffset;
            transform.LookAt(focusPosition);
        }
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


    //Util
    public void SetTarget(GameObject ps)
    {
        followTarget = ps;
    }
}
