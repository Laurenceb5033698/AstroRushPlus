using UnityEngine;
using System.Collections;

public class Weapon_Standard : Weapon
{
    override public void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for weapon type
        GameObject mBullet;
        mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);

    }

    override public void Shoot(Vector3 direction)
    {
        if (Time.time > reload)
        {
            reload = Time.time + shootSpeed;
            spawnProjectile(direction);

            shootSound.Play();
        }

    }
}
