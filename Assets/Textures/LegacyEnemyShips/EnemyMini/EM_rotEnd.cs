using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_rotEnd : MonoBehaviour {

    Quaternion rot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(transform.parent);
        transform.gameObject.transform.Rotate(transform.forward, 200 * Time.deltaTime);
	}
}
