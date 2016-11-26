using UnityEngine;
using System.Collections;

public class CheckerScript : MonoBehaviour {

	public Collider col;

	public bool collided = false;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider col)
	{
		collided = true;
	}

	public void ResetCollider()
	{
		collided = false;
	}

	public bool GetColliderState()
	{
		return collided;
	}


}
