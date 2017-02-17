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
        return list[Random.Range(0, 4)];

    }
    public void playPickupSound()
    {
        pickupSound.Play();
    }
}
