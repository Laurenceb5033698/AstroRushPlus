using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner_Scatter : ProjectileSpawner
{
    /// <summary>
    /// Scatter will randomly send projectiles within the spread angle.
    /// </summary>
    public override List<GameObject> SpawnImpl(GameObject _prefab, Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //number of projectiles
        int numProjectiles = Mathf.CeilToInt(IStats().ProjectileAmount  );
        float spreadAngle = IStats().SpreadAngle;

        if (numProjectiles < 1)
            numProjectiles = 1;

        if (numProjectiles == 1)
            spreadAngle = 0;

        Quaternion StartRotation = _aimDirection * Quaternion.AngleAxis(spreadAngle / -2, Vector3.up);

        List<GameObject> bullets = new List<GameObject>();
        for ( int i = 0; i< numProjectiles; i++)
        {
            Quaternion QuatInaccuracy = Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), Vector3.up);

            Quaternion bulletDirection =  StartRotation * QuatInaccuracy;

            bullets.Add(Instantiate<GameObject>(_prefab, _shootPosition, bulletDirection));
        }
        return bullets;
    }
}
