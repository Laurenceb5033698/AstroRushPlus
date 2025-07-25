﻿using UnityEngine;
using System.Collections;


public class Asteroid : MonoBehaviour
{
    private GameObject am; // asteroid manager
    private Health hp;
    [SerializeField] private GameObject psDestructPrefab;
    public float CollisionImpulse = 18;
    void OnEnable()
    {
        hp.SetHealth(200);
    }

    void Awake()
    {
        hp = gameObject.AddComponent<Health>();
       // hp = new Health();
    }

    public void SetAsteroidManager(GameObject a)
    {
        am = a;
    }

    void OnCollisionEnter(Collision c)
    {
        GetComponent<Rigidbody>().AddForce(((transform.position - c.gameObject.transform.position).normalized) * CollisionImpulse, ForceMode.Impulse);
        if (c.relativeVelocity.magnitude > 5f)
        {
            //int impactDamage = Mathf.FloorToInt(c.relativeVelocity.magnitude);
            int impactDamage = 4;
            TakeDamage(c.transform.position, impactDamage);
            
        }
    }
    public void TakeDamage(Vector3 _otherpos, float val)
    {
        hp.TakeDamage(val);
        if (!hp.IsAlive())
        {   //asteroid dies
            //spawn debris and particles
            am.GetComponent<AsteroidManager>().SpawnChunks(transform.position);

            gameObject.SetActive(false);
            Instantiate(psDestructPrefab, transform.position, transform.rotation);

            Reset();
        }
    }

    public void Reset()
    {   //tell asteroidManager that i wish to be reset
        am.GetComponent<AsteroidManager>().Reset(gameObject);
        gameObject.SetActive(true);
        hp.SetHealth(200);
    }

}
