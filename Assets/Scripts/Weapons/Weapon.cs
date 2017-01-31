using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour{

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject ship;
    //private Inputs controls = null;
    [SerializeField] private float shootSpeed;//fiddle with this one

    enum WeaponFlavour { Pew, Tri };

    [SerializeField] private WeaponFlavour WeaponType = WeaponFlavour.Pew;
    private float reload = 0;

    [SerializeField] private float bulletDamage = 5f;
    [SerializeField] private float bulletSpeed = 20f;

	
	void Start () // Use this for initialization
    {
	}
	void Update () // Update is called once per frame
    {
	}

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }
    private void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for weapon type
        GameObject mBullet;
        switch (WeaponType)
        {
            case WeaponFlavour.Pew:
                mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
                mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);
                break;
            case WeaponFlavour.Tri:
                //three instances
                mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
                mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);

                //mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
                //mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);

                //mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
                //mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);
                break;
            default://pew
                mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
                mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);
                break;

        }
    }
    public void Shoot(Vector3 direction)
    {
        if(Time.time > reload)
        {
            reload = Time.time + shootSpeed;
            spawnProjectile(direction);
        }
         
         
    }

}
