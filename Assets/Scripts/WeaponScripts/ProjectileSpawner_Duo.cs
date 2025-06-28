using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner_Duo : ProjectileSpawner
{
    /// <summary>
    /// Duo spawns projectiles that fly parallel in the shoot direction.
    /// However they are separated horizontally by some amount.
    /// </summary>
    public override List<GameObject> SpawnImpl(GameObject _prefab, Vector3 _shootPosition, Quaternion _aimDirection)        
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(GetStat(StatType.gProjectileAmount));
        float spreadAngle = GetStat(StatType.gSpreadAngle);

        float separation = 0.0f;

        if (numProjectiles < 1)
            numProjectiles = 1;

        //if (numProjectiles == 1)
        //    spreadAngle = 0;
        //else
            separation = spreadAngle;

        //duo spawns two bullets per shot

        Vector3 AimDir = (_shootPosition - transform.position);
        //perpendicular to aimDir and Up, where aimdir is always horizontal.
        Vector3 spreadDirection = Vector3.Cross(AimDir, Vector3.up).normalized;

        //slightly larger gap in the middle
        Vector3 CenterSpawn = _shootPosition;
        Vector3 CenterOffset = spreadDirection * separation;//horizontal from spawncenter * separation

        //want a chevron-style spawn for bullets
        Vector3 M = transform.position + (AimDir + spreadDirection) * 0.9f;
        Vector3 N = transform.position + (AimDir - spreadDirection) * 0.9f;

        //new spreadirection in slightly backward sweeping chevron
        //projectiles either side spawn close together than in center gap.
        Vector3 separationUnitA = (M - _shootPosition) * separation;
        Vector3 separationUnitB = (N - _shootPosition) * separation;

        //spawn two bullets per loop, one either side of center. increasing numProjectiles adds more pairs of bullets
        List<GameObject> bullets = new List<GameObject>();
        for (int i = 0; i < numProjectiles; i++)
        {
            Vector3 bulletSpawn = CenterSpawn+CenterOffset + (separationUnitA * i);
            bullets.Add(Instantiate<GameObject>(_prefab, bulletSpawn, _aimDirection));

            bulletSpawn = CenterSpawn-CenterOffset + (separationUnitB * i);
            bullets.Add(Instantiate<GameObject>(_prefab, bulletSpawn, _aimDirection));

        }

        return bullets;
    }
}
