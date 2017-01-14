using UnityEngine;
using System.Collections;

public class CheckerScript : MonoBehaviour {

	public Collider col;

	public bool collided = false;

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
