using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GameObject target;
    private readonly Vector3 offset = new Vector3(0,70,-40);

	void Update ()
    {
        if (target != null)
        {
            transform.position = target.transform.position + offset;
            transform.LookAt(target.transform.position);
        }
    }

    public void SetTarget(GameObject ps)
    {  
        target = (ps != null) ? ps : null;
    }
}
