using UnityEngine;

public class UWeapon_Standard : Universal_Weapon_Base
{
    //reads values straight from ship's stats
    //is set from owner controller

    

    private void Awake()
    {
        
    }


    void Start()
    {
        
    }
    
    /// <summary>
    /// Standard Shoots one projectile in direction of aim.
    /// loadout upgrades still affect it though, so still reads from stats.
    /// </summary>
    protected override void SpawnProjectilesImpl()
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(ShipStats.gProjectileAmount.Value);
        float spreadAngle = ShipStats.gSpreadAngle.Value;

        if (numProjectiles < 1)
            numProjectiles = 1;
        if (spreadAngle < 1)
            spreadAngle = 1;

        //angle between each bullet.
        float separation = spreadAngle / numProjectiles;

        Vector3 spawnPosition = m_AimingIndicator.transform.position;
        Quaternion spawnDirection = m_AimingIndicator.transform.rotation;

        //create rotation about up vector for start direction.
        Quaternion StartRotation = spawnDirection * Quaternion.AngleAxis(-spreadAngle/2, Vector3.up);
        

        GameObject bullet;
        for (int i = 0; i < numProjectiles; i++)
        {
            //create unit rotation for this index.
            Quaternion unitRotation = Quaternion.AngleAxis(separation*i, Vector3.up);
            //quaternions rotate by multiplying. Rotates startdirection by step amount.
            Quaternion bulletDirection = StartRotation * unitRotation;
            bullet = Instantiate<GameObject>(m_BulletPrefab, spawnPosition, spawnDirection);
            //bullet.setupBullet
            bullet.GetComponent<Projectile>().SetupValues(5,20.0f, m_Ship.tag);
        }
    }



}
