using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

    public ParticleSystem thruster;
    public bool isActive = true;



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleThruster(bool s)
    {
        isActive = s;
        updateThruster();
    }

    private void updateThruster()
    {
        ParticleSystem.EmissionModule temp;
        temp = thruster.emission;
        temp.enabled = isActive;
    }

}
