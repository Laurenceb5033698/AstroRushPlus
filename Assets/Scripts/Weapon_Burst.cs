using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Burst : Weapon {

    public int maxAmmo = 10;
    private int ammo = 0;//current ammo
    public float reloadRate = 1f;//1second
    private float reloadingClip =  0;//internal variable

    public int numProjectilesPerBurst = 1;
    public float spread = 2f;

    public override void Awake()
    {
        base.Awake();
        reloadingClip = reloadRate;
    }
    private void Update()
    {
        if (ammo <= 0)
        {//reloading
            reloadingClip -= Time.deltaTime;
            if (reloadingClip <= 0f)
            {//actaully reload after delay
                ammo = maxAmmo;
                reloadingClip = reloadRate;
            }
        }
    }

    override public void spawnProjectile(Vector3 aimDir)
    {
        GameObject mBullet;
        Vector3 adjustTemp = Vector3.Cross(aimDir, Vector3.up) * spread;
        Vector3 spreadera = (aimDir * 6f) + (adjustTemp * Random.Range(-1f, 1f));

        for (int shot=0 ;shot < numProjectilesPerBurst; shot++)
        {
            mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + spreadera, Quaternion.LookRotation(spreadera.normalized, Vector3.up));
            mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);
            spreadera = (aimDir * 6f) + (adjustTemp * Random.Range(-1f, 1f));
        }
    }

    override public void Shoot(Vector3 direction)
    {
        if (ammo > 0)
        {//we have ammo, so shoot
            if (Time.time > reload)
            {
                ammo--;
                reload = Time.time + shootSpeed;
                spawnProjectile(direction);
                //turret.transform.LookAt(direction);
                shootSound.Play();
            }
        }
    }
}
