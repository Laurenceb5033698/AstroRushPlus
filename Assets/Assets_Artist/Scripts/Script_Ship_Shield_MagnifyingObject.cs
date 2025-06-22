using UnityEngine;

public class Script_Ship_Shield_MagnifyingObject : MonoBehaviour
{
    Renderer _renderer;
    Camera _cam;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _cam = Camera.main;
        
    }

    void Update()
    {
        Vector3 screenPoint = _cam.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;
        _renderer.material.SetVector("_ObjScreenPosition", screenPoint);
    }
}
