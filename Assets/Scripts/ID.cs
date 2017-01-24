using UnityEngine;
using System.Collections;

public class ID : MonoBehaviour
{
    private int id;
    private GameObject sm;

    public int GetID()
    {
        return id;
    }

    public void Initalise(int i, GameObject s)
    {
        id = i;
        sm = s;
    }

    public void Reset()
    {
        //switch(transform.gameObject.tag)
        //{
        //    //case "Asteroid": sm.GetComponent<AsteroidManager>().Reset(id); break;
            
        //}
    }
}
