using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Cannon : Weapon {

    [SerializeField] protected GameObject mAimingVisuals;//object that holds the laser
    protected Quaternion mLaserAimDir;//the actual direction used for aiming

    [SerializeField] protected float mAimingSpeed = 2f;

    private void OnEnable()
    {
        mLaserAimDir = transform.rotation;
        mAimingVisuals.SetActive(true);
        mAimingVisuals.transform.rotation = mLaserAimDir;
    }

    private void OnDisable()
    {
        mAimingVisuals.SetActive(false);
    }

    private void Update()
    {
        //while enabled: every tick, set laser AimingVisuals = mLaserAimDir;
        mAimingVisuals.transform.rotation = mLaserAimDir;
    }

    override public void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for weapon type
        GameObject mBullet;
        mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(finalBulletDamage, bulletSpeed, ship.tag);

    }

    override public void Shoot(Vector3 direction)
    {
        //while trying to shoot
        //turn laser sight towards player aim direction.
        Quaternion PlayerShootDir = Quaternion.LookRotation(direction, Vector3.up);
        mLaserAimDir = Quaternion.RotateTowards(mLaserAimDir, PlayerShootDir, mAimingSpeed);

        //test new angle
        float aimDiff = Quaternion.Angle(mLaserAimDir, PlayerShootDir);
        
        if (Time.time > reload)//if reloaded
            if (aimDiff < 1f)//test angle
            {//if it's close enough, shoot bullet
                reload = Time.time + shootSpeed;
                spawnProjectile(direction);

                shootSound.Play();
                //play shooting effect
            }

        //when reloading
        //  play charge effect (particle effect)


    }
}
