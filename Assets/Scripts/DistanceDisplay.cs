using UnityEngine;
using System.Collections;

public class DistanceDisplay : MonoBehaviour {

    [SerializeField] private GameObject pointer;
    [SerializeField] private TextMesh distanceDisplay;
    [SerializeField] private GameObject from;
    [SerializeField] private GameObject target;
    [SerializeField] private Camera cam;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
            target = transform.gameObject;

        distanceDisplay.text = Vector3.Distance(from.transform.position, target.transform.position).ToString("F2");
        distanceDisplay.transform.position = new Vector3(pointer.transform.position.x + 1.21f, pointer.transform.position.y + 1.46f, pointer.transform.position.z + 1.79f);
        distanceDisplay.transform.eulerAngles = new Vector3(45f, cam.transform.eulerAngles.y, 0f); // make text face camera
    }

    public void SetNewTarget(GameObject go)
    {
        target = go;
    }
}
