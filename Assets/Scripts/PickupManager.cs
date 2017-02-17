using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] list = new GameObject[4];
    [SerializeField]
    private AudioSource pickupSound;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetRandomPickup()
    {
        int randomN = Random.Range(0,100);
        int ID = -1;

        if (randomN >= 0 && randomN <= 35) ID = 0;
        if (randomN > 35 && randomN <= 50) ID = 1;
        if (randomN > 50 && randomN <= 85) ID = 2;
        if (randomN > 85 && randomN <= 100) ID = 3;

        return list[ID];

    }
    public void playPickupSound()
    {
        pickupSound.Play();
    }
}
