using System.Transactions;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class ShipRotationHandler : MonoBehaviour
{
    //rotates ship visuals to facing direction.
    //rotates ship model for tilt effect.
    [Range(0f, 100f)]
    [SerializeField] private float MaxTiltAmount = 0.09f;
    [Range(0f, 1f)]
    [SerializeField] private float turnSharpness = 0.95f;
    float targetRotation;
    bool moving = false;

    private void Update()
    {
        //while not tilting, return to this
        if(!moving)
            targetRotation = 0f;

        //lerp current rotation towards target amount
        //Vector3 intermediate = Vector3.Slerp(transform.localRotation.eulerAngles, targetRotation, MaxTiltAmount);

        float currentAngle = transform.localRotation.eulerAngles.z;
        currentAngle = Mathf.LerpAngle(currentAngle, targetRotation, MaxTiltAmount * Time.deltaTime);
        Vector3 intermediate = new Vector3(0, 0, currentAngle);
        //apply new rotation
        transform.localEulerAngles = intermediate;
        

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetRotation, Vector3.up), MaxTiltAmount);
        moving = false;
    }

    public void Tilt(Vector3 _targetDir, float _mod)
    {
        //only tilt if turn is shar penough
        Vector3 facingdir = transform.forward;
        float dotprod = Vector3.Dot(facingdir, _targetDir);

        //target rot is same as forward, but with z value between -90:90.
        targetRotation = 0f;

        //magnitude = 1 at opposite, 0 at same dir as 
        float turnmagnitude = 1 - ( (dotprod + 1) / 2);
        if(dotprod < turnSharpness)
        {
            //needto know which way
            Vector3 crosspord = Vector3.Cross(transform.forward, _targetDir);
            float rolldir = crosspord.y > 0 ? -1 : 1;
            
            targetRotation = 89 * rolldir * turnmagnitude *_mod;
        }
        

        moving = true;
    }
}
