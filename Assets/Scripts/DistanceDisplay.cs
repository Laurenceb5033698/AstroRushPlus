using UnityEngine;
using System.Collections;

public class DistanceDisplay : MonoBehaviour {

    public GameObject pointer;
    public TextMesh distanceDisplay;
    public GameObject from;
    public GameObject target;
    public Camera cam;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        distanceDisplay.text = Vector3.Distance(from.transform.position, target.transform.position).ToString("F2");
        distanceDisplay.transform.position = new Vector3(pointer.transform.position.x + 1.21f, pointer.transform.position.y + 1.46f, pointer.transform.position.z + 1.79f);

        distanceDisplay.transform.eulerAngles = new Vector3(45f, cam.transform.eulerAngles.y, 0f); // make text face camera
    }
}
