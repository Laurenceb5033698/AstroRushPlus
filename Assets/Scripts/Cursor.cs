using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

    public GameObject cursor;
    public float cursorSize = 1;

    void Start()
    {
        cursor.transform.localScale = new Vector3(cursorSize, cursorSize, 1);
        
    }

    void Update()
    {
        cursor.transform.position = Input.mousePosition;
    }

}
