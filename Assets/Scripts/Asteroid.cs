using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float ore = 20;

	public void MineOre(float mineSpeed)
	{
		ore -= mineSpeed;

        if (ore < 0.1f)
        {
            Destroy(transform.gameObject);
        }
	}

    void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.magnitude > 4f)
        {
            Destroy(transform.gameObject);
        }
    }
}
