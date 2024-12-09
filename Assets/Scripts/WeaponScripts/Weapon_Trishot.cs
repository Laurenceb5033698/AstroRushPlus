using UnityEngine;
using System.Collections;

public class Weapon_Trishot : Weapon
{
    private float spread = 2;

    override public void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for weapon type
        GameObject mBullet;
        Vector3 adjust = (Vector3.Cross(aimDir, Vector3.up) * spread);
        Vector3 spreadera = (aimDir * 6f) + adjust;//spread is an arbitrary value which increases the angle of spread
        Vector3 spreaderb = (aimDir * 6f) - adjust;//spread is an arbitrary value which increases the angle of spread

        mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + spreadera, Quaternion.LookRotation(spreadera.normalized, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(finalBulletDamage, bulletSpeed, ship.tag);

        mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(finalBulletDamage, bulletSpeed, ship.tag);

        mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + spreaderb, Quaternion.LookRotation(spreaderb.normalized, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(finalBulletDamage, bulletSpeed, ship.tag);

    }

    override public void Shoot(Vector3 direction)
    {//shoot probably needs to change per weapon varient
        if (Time.time > reload)
        {
            reload = Time.time + shootSpeed;
            spawnProjectile(direction);
            //turret.transform.LookAt(direction);
            shootSound.Play();
        }

    }
}