using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float ore = 20;

	public float MineOre(float mineSpeed)
	{
		float temp = 0;
		if (ore - mineSpeed >= 0) 
		{
			temp = mineSpeed;
			ore -= mineSpeed;
		}
		return temp;
	}

	public float GetOreAmountLeft()
	{
		return ore;
	}

	public void DestroyAsteroid()
	{
		Destroy (transform.gameObject);
	}

}
