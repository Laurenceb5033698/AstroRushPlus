using UnityEngine;

public class Script_Ship_Shield_LookAtCamera : MonoBehaviour
{
    Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
    }

    
    void Update()
    {
        transform.forward = _cam.transform.position - transform.position;
    }
}
