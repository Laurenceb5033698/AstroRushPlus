﻿using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using System;

public class Health : MonoBehaviour {

    [SerializeField] protected float health = 100;

    //void OnCollisionEnter(Collision c)
    //{
    //    if (c.relativeVelocity.magnitude > 5f) TakeDamage(c.relativeVelocity.magnitude);
    //}

    public virtual void TakeDamage(float amount)
    {
        health -= Mathf.Abs(amount);

        if (health <= 0)
        {
            health = 0;    

            //switch (transform.gameObject.tag)
            //{
            //    case "Asteroid": transform.gameObject.GetComponent<ID>().Reset(); break;
            //}
        }
    }
    public bool IsAlive()
    {
        return (health > 0);
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(int h)
    {
        health = h;
    }

   
}
