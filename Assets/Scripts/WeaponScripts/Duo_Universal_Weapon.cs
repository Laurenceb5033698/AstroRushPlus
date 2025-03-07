using UnityEngine;

public class Duo_Universal_Weapon : Universal_Weapon_Base
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
    override protected void SpawnProjectilesImpl()
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt( ShipStats.gProjectileAmount.Value);
        float spreadAngle = ShipStats.gSpreadAngle.Value;

        if (numProjectiles < 1)
            numProjectiles = 1;
        if (spreadAngle < 1)
            spreadAngle = 1;

        float separation = spreadAngle / numProjectiles;

        //Spawning requires aiming Indicator object.
        Vector3 spawnPosition = m_AimingIndicator.transform.position;
        Quaternion spawnDirection = m_AimingIndicator.transform.rotation;

        //perpendicular to aimDir and Up, where aimdir is always horizontal.
        Vector3 spreadDirection = Vector3.Cross(spawnDirection.eulerAngles, Vector3.up);

        Vector3 horizontalSpreadStart = spawnPosition - (spreadDirection * spreadAngle/2);
        Vector3 separationUnit = spreadDirection * separation;

        GameObject bullet;
        for (int i =0; i < numProjectiles; i++)
        {
            Vector3 bulletSpawn = horizontalSpreadStart + (separationUnit * i);
            bullet = Instantiate<GameObject>(m_BulletPrefab, bulletSpawn, spawnDirection);
            //bullet.setupBullet
        }
    }
}
