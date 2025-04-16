using UnityEngine;

public class UWeapon_Trishot : Universal_Weapon_Base
{
    //reads values straight from ship's stats
    //is set from owner controller

    protected override void Startup()
    {
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
        {
            //no spawner found, add ours.
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Spread>();
        }
        else
        {   //spawner found, but what type?
            //do not want base type. i want derived type
            if (m_ProjectileSpawner is ProjectileSpawner_Spread)
            {
                //happy
            }
            else
            {
                //not happy
                Destroy(m_ProjectileSpawner);
                m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Spread>();
            }
        }
    }

    /// <summary>
    /// Trishot Shoots a spread of projectiles about direction of aim.
    /// </summary>
    protected override void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection)
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
        Quaternion direction = Quaternion.LookRotation(_shootPosition - transform.position, Vector3.up);
        Quaternion StartRotation = direction * Quaternion.AngleAxis(spreadAngle / -2, Vector3.up);

        GameObject bullet;
        for (int i = 0; i < numProjectiles; i++)
        {
            //create unit rotation for this index.
            Quaternion unitRotation = Quaternion.AngleAxis(separation * i, Vector3.up);
            //quaternions rotate by multiplying. Rotates startdirection by step amount.
            Quaternion bulletDirection = unitRotation * StartRotation;
            bullet = Instantiate<GameObject>(m_BulletPrefab, _shootPosition, bulletDirection);
            
            SetupBullet(bullet);
        }
    }



}
