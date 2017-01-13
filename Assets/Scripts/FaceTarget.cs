using UnityEngine;
using System.Collections;

public class FaceTarget : MonoBehaviour {

    [SerializeField]
    private GameObject target;

    private Quaternion lookD;

	// Use this for initialization
	void Start () {
        lookD = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () 
    {
        lookD = Quaternion.LookRotation(target.transform.position);
        transform.localEulerAngles = new Vector3(0, 0, lookD.z);

        
	}
}
