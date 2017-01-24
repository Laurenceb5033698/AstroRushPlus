using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {

    [SerializeField] private GameObject pointerM; // model
    [SerializeField] private GameObject from;
    [SerializeField] private GameObject to;

    private const float distance = 8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (to == null)
            to = transform.gameObject;

        UpdatePointer();
    }

    private void UpdatePointer()
    {
        Vector3 posA = from.transform.position;
        Vector3 posB = to.transform.position;

        Vector3 pointDir = (posB - posA).normalized;
        pointerM.transform.position = posA + pointDir * distance;
        pointerM.transform.rotation = Quaternion.LookRotation(-pointDir);
    }

    public void SetNewTarget(GameObject go)
    {
        to = go;
    }


}
