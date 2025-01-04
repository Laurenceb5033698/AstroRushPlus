using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBurstProjectile : Projectile
{
    [SerializeField] GameObject PrefSecondaryBullet;
    [SerializeField] [Range(1,20)] int numberOfShards = 4;
    [SerializeField] int SecondDmg = 5;
    [SerializeField] float SecondSpd = 5;




    protected override void DestroySelf()
    {
        //calc angle from 360/numberOfShards
        int angle = 360 / numberOfShards;
        //spawn projectiles based on number Of Shards
        for( int index = 0; index < numberOfShards; index++)
        {
            //spawn bullet at each angle
            //Vector3 spreader = Vector3.Cross(transform.forward, Vector3.up) * angle*index;
            Quaternion lookAngle = Quaternion.Euler(0, angle*index, 0);
            GameObject secBullet = Instantiate(PrefSecondaryBullet, transform.position + lookAngle.eulerAngles.normalized*3, lookAngle);
            secBullet.GetComponent<Projectile>().SetupValues(SecondDmg, SecondSpd, ownertag);
        }

        

        Instantiate(psImpactPrefab, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }


}
