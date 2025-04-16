using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ProjectileSpawner_Minigun : ProjectileSpawner
{
    float m_innaccuracy = 0;

    public void Spawn(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        SpawnImpl(_shootPosition, _aimDirection);
    }
    public override List<GameObject> SpawnImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(ShipStats.Get(StatType.gProjectileAmount));
        float spreadAngle = ShipStats.Get(StatType.gSpreadAngle) / 10;

        //unknown if minigun can have multiple projectiles

        //angle between each bullet. S = A/N-1, where N>1
        float separation = 0.0f;

        if (numProjectiles < 1)
            numProjectiles = 1;

        if (numProjectiles == 1)
            spreadAngle = 0;
        else
            separation = spreadAngle / (numProjectiles - 1);

        //create rotation about up vector for start direction.
        Quaternion direction = Quaternion.LookRotation(_shootPosition - transform.position, Vector3.up);
        Quaternion StartRotation = direction * Quaternion.AngleAxis(spreadAngle / -2, Vector3.up);

        //minigun ramps up, then begins to get innacurate as it overheats.
        List<GameObject> bullets = new List<GameObject>();
        for (int i = 0; i < numProjectiles; i++)
        {
            //if firing multiple projectiles, each projectile gets its own inaccuracy.
            Quaternion QuatInaccuracy = Quaternion.AngleAxis(Random.Range(-m_innaccuracy, m_innaccuracy), Vector3.up);

            //if spread shot, unit rotation splits projectiles into streams
            Quaternion unitRotation = Quaternion.AngleAxis(separation * i, Vector3.up);
            //quaternions rotate by multiplying. Rotates startdirection by step amount.
            Quaternion bulletDirection = unitRotation * StartRotation * QuatInaccuracy;

            bullets.Add( Instantiate<GameObject>(m_spawnPrefab, _shootPosition, bulletDirection));
        }
        return bullets;
    }

    public void SetValues(float _burnoutCurrent)
    {
        m_innaccuracy = _burnoutCurrent;
    }
}
