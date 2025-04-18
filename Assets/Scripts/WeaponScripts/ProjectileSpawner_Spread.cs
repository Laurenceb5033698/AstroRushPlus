using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner_Spread : ProjectileSpawner
{

    /// <summary>
    /// Spread method Shoots one projectile in direction of aim.
    /// if ship has more projectiles, will shoot in a fan pattern.
    /// </summary>
    public override List<GameObject> SpawnImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(ShipStats.Get(StatType.gProjectileAmount));
        float spreadAngle = ShipStats.Get(StatType.gSpreadAngle);

        //angle between each bullet. S = A/N-1, where N>1
        float separation = 0.0f;

        if (numProjectiles < 1)
            numProjectiles = 1;

        if (numProjectiles == 1)
            spreadAngle = 0;
        else
            separation = spreadAngle / (numProjectiles - 1);



        //create rotation about up vector for start direction.
        Quaternion StartRotation = _aimDirection * Quaternion.AngleAxis(spreadAngle / -2, Vector3.up);

        List<GameObject> bullets = new List<GameObject>();
        for (int i = 0; i < numProjectiles; i++)
        {
            //create unit rotation for this index.
            Quaternion unitRotation = Quaternion.AngleAxis(separation * i, Vector3.up);
            //quaternions rotate by multiplying. Rotates startdirection by step amount.
            Quaternion bulletDirection = unitRotation * StartRotation;
            bullets.Add(Instantiate<GameObject>(m_spawnPrefab, _shootPosition, bulletDirection));

        }
        return bullets;
    }


}
