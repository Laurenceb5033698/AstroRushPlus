using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	public GameObject ship;

	void Update () {
		transform.position = new Vector3 (ship.transform.position.x, 45f, ship.transform.position.z - 22.5f);
	}
}
