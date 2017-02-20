using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WaveManager : MonoBehaviour {

    List<int> spawnShips = new List<int>();
    private int wave = 0;
    private int noShipTypes;
    private int spawnIncAmount = 2;

    private EnemyManager em;
    [SerializeField] private AudioSource nextWaveSound;

    void Start ()	// Use this for initialization
    {
        em = GetComponent<EnemyManager>();
        noShipTypes = em.GetNoShipTypes();
        for (int i = 0; i < noShipTypes; i++) { spawnShips.Add(0); }
        UpdateWave();
    }
	
	void Update ()	// Update is called once per frame
    {
        if (em.GetSpawnState())
        {
            playNextWaveSound();
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

    private void playNextWaveSound()
    {
        nextWaveSound.Play();
    }
}
