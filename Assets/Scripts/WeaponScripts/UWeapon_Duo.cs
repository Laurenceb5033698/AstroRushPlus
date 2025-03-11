using UnityEngine;

public class UWeapon_Duo : Universal_Weapon_Base
{

    void Start()
    {
        
    }

    // for visual updates.
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    //Shoot(); //use base method.


    /// <summary>
    /// Duo spawns projectiles that fly parallel in the shoot direction.
    /// However they are separated horizontally by some amount.
    /// </summary>
    override protected void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt( ShipStats.gProjectileAmount.Value);
        float spreadAngle = ShipStats.gSpreadAngle.Value;

        if (numProjectiles < 1)
            numProjectiles = 1;
        if (spreadAngle < 1)
            spreadAngle = 1;

        //TODO:
        //figure out sensible horizontal separation that is adjusted by spread...
        //for now: uses spread as horizontal distance.

        float separation = spreadAngle / numProjectiles;


        //perpendicular to aimDir and Up, where aimdir is always horizontal.
        Vector3 spreadDirection = Vector3.Cross(_aimDirection.eulerAngles, Vector3.up);

        Vector3 horizontalSpreadStart = _shootPosition - (spreadDirection * spreadAngle/2);
        Vector3 separationUnit = spreadDirection * separation;

        GameObject bullet;
        for (int i =0; i < numProjectiles; i++)
        {
            Vector3 bulletSpawn = horizontalSpreadStart + (separationUnit * i);
            bullet = Instantiate<GameObject>(m_BulletPrefab, bulletSpawn, _aimDirection);
            bullet.GetComponent<Projectile>().SetupValues(5, 20.0f, m_Ship.tag);
        }
    }
}
