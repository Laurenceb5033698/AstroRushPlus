using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {

    public GameObject pointerM; // model
    public GameObject from;
    public GameObject to;
    public float distance = 8f;



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
        Vector3 posB = to.transform.position;

        Vector3 pointDir = (posB - posA).normalized;
        pointerM.transform.position = posA + pointDir * distance;
        pointerM.transform.rotation = Quaternion.LookRotation(-pointDir);
    }


}
