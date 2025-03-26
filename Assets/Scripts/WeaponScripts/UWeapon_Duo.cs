using UnityEngine;

public class UWeapon_Duo : Universal_Weapon_Base
{
    //Shoot(); //use base method.


    /// <summary>
    /// Duo spawns projectiles that fly parallel in the shoot direction.
    /// However they are separated horizontally by some amount.
    /// </summary>
    override protected void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt( ShipStats.Get(StatType.gProjectileAmount));
        float spreadAngle = ShipStats.Get(StatType.gSpreadAngle) /10;

        //angle between each bullet. S = A/N-1, where N>1
        float separation = 0.0f;

        if (numProjectiles < 1)
            numProjectiles = 1;

        if (numProjectiles == 1)
            spreadAngle = 0;
        else
            separation = spreadAngle / (numProjectiles - 1);

        //TODO:
        //figure out sensible horizontal separation that is adjusted by spread...
        //for now: uses spread as horizontal distance.

        Vector3 AimDir = (_shootPosition - transform.position).normalized;
        Quaternion direction = Quaternion.LookRotation(_shootPosition - transform.position, Vector3.up);
        //perpendicular to aimDir and Up, where aimdir is always horizontal.
        Vector3 spreadDirection = Vector3.Cross(AimDir, Vector3.up);

        Vector3 horizontalSpreadStart = _shootPosition - (spreadDirection * spreadAngle/2);
        Vector3 separationUnit = spreadDirection * separation;

        GameObject bullet;
        for (int i =0; i < numProjectiles; i++)
        {
            Vector3 bulletSpawn = horizontalSpreadStart + (separationUnit * i);
            bullet = Instantiate<GameObject>(m_BulletPrefab, bulletSpawn, direction);
            bullet.GetComponent<Projectile>().SetupValues((int)ShipStats.Get(StatType.gAttack), ShipStats.Get(StatType.bSpeed), m_Ship.tag);
        }
    }
}
