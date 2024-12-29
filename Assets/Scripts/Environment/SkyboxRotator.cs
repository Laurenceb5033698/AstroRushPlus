using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SkyboxRotator : MonoBehaviour
{
    
    [SerializeField]Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        Vector3 camWorldPos = mainCamera.transform.position/32;
        
        Quaternion rotationFromPosition = Quaternion.identity;
        rotationFromPosition.eulerAngles = new Vector3(camWorldPos.z, 0, -camWorldPos.x);
        Quaternion outRotation = Quaternion.SlerpUnclamped(transform.rotation, rotationFromPosition, 0.5f);
        this.transform.rotation = outRotation;
    }
}
