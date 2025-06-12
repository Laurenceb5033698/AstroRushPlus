using UnityEngine;

public class ShipRotationHandler : MonoBehaviour
{
    //rotates ship visuals to facing direction.
    //rotates ship model for tilt effect.
    [Range(0f, 20f)]
    [SerializeField] private float MaxTiltAmount = 1.0f;

    public void Tilt(Vector3 _targetDir, float _mod)
    {
        //needto know which way
        Vector3 crosspord = Vector3.Cross(transform.forward, _targetDir);
        float rolldir = crosspord.y > 0 ? 1 : -1;
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z = 0 + 90 * rolldir * _mod;

        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(euler), MaxTiltAmount);

    }
}
