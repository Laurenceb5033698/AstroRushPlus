using UnityEngine;
using System.Collections;

public class StationManager : MonoBehaviour 
{
	// Station Variables -------------------------------------------
	private const int fuelPrice = 10; 	// fuel price per %
	private const int cargoPrice = 20; // cargo price per %
	private const int RepairCost = 5; 	// cost of repair per %

	public void GetDockingRequest()
	{
		Debug.Log ("Docking is not a feature yet");
	}

	public int GetFuelPrice()
	{
		return fuelPrice;
	}
	public int GetRepairCost()
	{
		return RepairCost;
	}
	public int GetCargoPrice()
	{
		return cargoPrice;
	}
}
