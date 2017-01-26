using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

    private int wave = 0;
    private EnemyManager em;

	void Start ()	// Use this for initialization
    {
        em = GetComponent<EnemyManager>();
        UpdateWave();
    }
	
	void Update ()	// Update is called once per frame
    {
        if (em.GetSpawnState())
        {
            UpdateWave();
        }
	}

    private void UpdateWave()
    {
        em.Reset();
        em.SetActive(false);
        wave++;

        switch (wave)
        {
            case 1: {
                    em.CreateOrder(0, 2);
                    em.CreateOrder(1, 2);
                } break;
            case 2:
                {
                    em.CreateOrder(0, 5);
                    em.CreateOrder(1, 5);
                }
                break;
            case 3:
                {
                    em.CreateOrder(0, 10);
                    em.CreateOrder(1, 10);
                }
                break;
            default: Debug.Log("Wave out of bound!"); wave--; break;
        }
        em.SetActive(true);
    }

    public int GetWave()
    {
        return wave;    
    }
}
