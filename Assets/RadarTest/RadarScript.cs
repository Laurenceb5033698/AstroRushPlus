using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarScript : MonoBehaviour {


	public GameObject ship;
	public GameObject radar;
	public GameObject yPlane;
	public Material lineMat;

	public LineRenderer line;
	public List<GameObject> objects = new List<GameObject> (); 
	public List<GameObject> RadarObjects = new List<GameObject> (); 

	private float toShipMinDist = 2f;
	private float toShipMaxDist = 10f;
	private float rMinDist = 0.5f;
	private float rMaxDist = 3f;

	// Use this for initialization
	void Start () 
	{
		
		for (int i = 0; i < objects.Capacity; i++) 
		{
			RadarObjects [i].gameObject.AddComponent<LineRenderer> ();
			LineRenderer temp = RadarObjects[i].gameObject.GetComponent<LineRenderer>();
			temp.SetWidth (0.1f,0.1f);
			temp.material = new Material (Shader.Find("Particles/Additive"));
			temp.SetColors (Color.green,Color.green);

		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdatePositions ();
		drawLines ();
	}

	private void UpdatePositions()
	{
		Vector3 pos = ship.transform.position;
		float tempDist = 0f;
		Vector3 tempDir = Vector3.zero;

		for (int i = 0; i < objects.Capacity; i++) 
		{
			tempDist = Vector3.Distance (pos,objects[i].transform.position);
			tempDir = (objects [i].transform.position - pos)/(objects [i].transform.position - pos).magnitude;

			if (tempDist < toShipMaxDist && tempDist > toShipMinDist) 
			{
				RadarObjects [i].transform.position = radar.transform.position + tempDir * (((rMaxDist - rMinDist) / 100) * (tempDist / toShipMaxDist * 100) + rMinDist);
			} 
			else if (tempDist <= toShipMinDist) 
			{
				RadarObjects [i].transform.position = radar.transform.position + tempDir * (rMinDist);
			}
			else if (tempDist >= toShipMaxDist) 
			{
				RadarObjects [i].transform.position = radar.transform.position + tempDir * (rMaxDist);
			}
		}
	}

	private void drawLines ()
	{
		Vector3 tempPos;

		for (int i = 0; i < RadarObjects.Capacity; i++) 
		{
			Vector3[] points = new Vector3[3];

			LineRenderer temp = RadarObjects [i].gameObject.GetComponent<LineRenderer> ();
			temp.SetVertexCount (3);
			tempPos = RadarObjects [i].transform.position;


			points [0] = tempPos;
			points [1] = new Vector3 (tempPos.x, yPlane.transform.position.y, tempPos.z);
			points [2] = radar.transform.position;

			temp.SetPositions (points);

		}
	}


}
