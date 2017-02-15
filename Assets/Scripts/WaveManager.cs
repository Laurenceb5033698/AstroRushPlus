using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

    private int wave = 0;
    private EnemyManager em;
    private const int noShipTypes = 3;
    private int[] spawnShips = new int[noShipTypes];
    [SerializeField] private int spawnIncAmount = 10;

	void Start ()	// Use this for initialization
    {
        em = GetComponent<EnemyManager>();
        spawnShips[0] = 0;
        spawnShips[1] = 0;
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
        for (int i = 0; i < noShipTypes; i++)
        {
            spawnShips[i] += spawnIncAmount;
            em.CreateOrder(i, spawnShips[i]);
        }

        em.SetActive(true);
    }

    public int GetWave()
    {
        return wave;    
    }
}
