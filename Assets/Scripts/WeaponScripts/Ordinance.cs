﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//twin of weapon class
public class Ordinance : MonoBehaviour {

    //shoots a projectile
    [SerializeField] protected GameObject ProjectilePrefab;//bullet prefab
    [SerializeField] protected GameObject ship; //reference to ship

    [SerializeField] protected int Damage = 5;
    [SerializeField] protected float Speed = 20f;
    //[SerializeField] protected AudioSource shootSound;
    

    public void Awake() // Use this for initialization
    {
        //shootSound = GetComponent<AudioSource>();


    }
    public void volumeChanged(float val)
    {
       // shootSound.volume = val;
    }
    private void OnEnable()
    {  }

    private void OnDisable()
    { }


    void Update() // Update is called once per frame
    { }

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }
    public void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for missile ordianance
        GameObject mBullet;

        mBullet = (GameObject)Instantiate(ProjectilePrefab, ship.transform.position + aimDir * 8f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(Damage, Speed, ship.tag);

    }

    public void Shoot(Vector3 direction)
    {
        spawnProjectile(direction);

        //shootSound.Play();
    }
}
