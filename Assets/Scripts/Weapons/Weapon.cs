using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour{

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject ship;
    //private Inputs controls = null;
    [SerializeField] private float shootSpeed;//fiddle with this one
    private float reload = 0;//used temp

    [SerializeField] private float bulletDamage = 5f;
    [SerializeField] private float bulletSpeed = 20f;

	// Use this for initialization
	void Start () {
        //ship = GetComponentInParent<GameObject>();
        
        //if (ship.GetComponent<ShipController>() != null)
        //    controls = GetComponentInParent<Inputs>();    //?
	}
	
	// Update is called once per frame
	void Update () {
        if (reload >0)
            reload -= Time.deltaTime;

	    //if controls.shoot
        //      shoot()
	}
    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }
    private void spawnProjectile(Vector3 aimDir)
    {//makes a spawn pattern
        //GameObject mBullet = null;
        /*
         shoot in spawn pattern around direction
         give each projectile speed and damage?
         */

		GameObject mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));

        mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);
        //set bullet damage and speed

    }
    public void Shoot(Vector3 direction)
    {//governs how fast this weapon shoots
        if(reload <= 0)
        {
            reload = shootSpeed;
            spawnProjectile(direction);
        }
         
         
    }

}
