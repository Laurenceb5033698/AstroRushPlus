using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour{

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject ship;
    //private Inputs controls = null;
    [SerializeField] private float shootSpeed;//fiddle with this one
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
    {
		GameObject mBullet = (GameObject)Instantiate(bullet, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(bulletDamage, bulletSpeed, ship.tag);
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
