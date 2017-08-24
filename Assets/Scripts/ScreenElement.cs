using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ScreenElement : MonoBehaviour {

    private Canvas cv = null;

    virtual public void Awake()
    {
        cv = GetComponent<Canvas>();
    }

    virtual public void OnEnable()
    {
        cv.enabled = true;
    }
    virtual public void OnDisable()
    {
        cv.enabled = false;
    }
}
