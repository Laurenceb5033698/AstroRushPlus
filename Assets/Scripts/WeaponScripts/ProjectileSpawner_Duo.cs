using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner_Duo : ProjectileSpawner
{
    /// <summary>
    /// Duo spawns projectiles that fly parallel in the shoot direction.
    /// However they are separated horizontally by some amount.
    /// </summary>
    public override List<GameObject> SpawnImpl(Vector3 _shootPosition, Quaternion _aimDirection)        
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(GetStat(StatType.gProjectileAmount));
        float spreadAngle = GetStat(StatType.gSpreadAngle) / 10;

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

        Vector3 horizontalSpreadStart = _shootPosition - (spreadDirection * spreadAngle / 2);
        Vector3 separationUnit = spreadDirection * separation;

        List<GameObject> bullets = new List<GameObject>();
        for (int i = 0; i < numProjectiles; i++)
        {
            Vector3 bulletSpawn = horizontalSpreadStart + (separationUnit * i);
            bullets.Add(Instantiate<GameObject>(m_spawnPrefab, bulletSpawn, direction));
        }

        return bullets;
    }
}
