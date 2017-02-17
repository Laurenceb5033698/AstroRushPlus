using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {

    [SerializeField] private GameObject pointerM; // model
    [SerializeField] private GameObject from;
    [SerializeField] private Vector3 to;

    private const float distance = 8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePointer();
    }

    private void UpdatePointer()
    {
        Vector3 posA = from.transform.position;
        Vector3 posB = to;

        Vector3 pointDir = (posB - posA).normalized;
        pointerM.transform.position = posA + pointDir * distance;
        pointerM.transform.rotation = Quaternion.LookRotation(-pointDir);
    }

    public void SetNewTarget(Vector3 go)
    {
        to = go;
    }

    public void SetFollowTarget(GameObject go)
    {
        from = go;
    }


}
