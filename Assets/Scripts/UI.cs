using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GameObject SSIpanel;
	public Text units;
	public Text fuel;
	public Text cargo;
	public Text damage;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateShipStats(int u, float f, float c, int d)
	{
		units.text = "Units: " + u;
		fuel.text = "Fuel: " + f.ToString ("F2") + "%";
		cargo.text = "Cargo: " + c.ToString ("F2") + "%";
		damage.text = "Damage: " + d + "%";
	}
}
