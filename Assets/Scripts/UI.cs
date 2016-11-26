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
	void Start ()
    {
        SSIpanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateShipStats(int u, float f, string c, int d)
	{
		units.text = "Units: " + u;
		fuel.text = "Fuel: " + f.ToString ("F2") + "%";
		cargo.text = "Cargo: " + c;
		damage.text = "Damage: " + d + "%";
	}

    public void UpdateStationPanelToggle(bool s)
    {
        SSIpanel.SetActive(s);
    }
}
