using UnityEngine;

public class MainMenuCameraRotate : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 1;

    void Update()
    {
        float angle = rotateSpeed * Time.deltaTime;
        this.transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }
}
