using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour {

    [SerializeField] private GameObject[] list = new GameObject[4];
    [SerializeField] private AudioSource pickupSound;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playPickupSound()
    {
        pickupSound.Play();
    }

    public bool SpawnPickup(Vector3 pos) // return successful spawn
    {
        if (Random.Range(0, 10) > 3)
        {
            GameObject temp = (GameObject)Instantiate(list[Random.Range(0, list.Length)], pos, Quaternion.identity);   // create gameobject
            temp.GetComponent<PickupItem>().Init(transform.gameObject);
            return true;
        }
        return false;
    }
}
