using UnityEngine;

public class UWeapon_Shotgun : Universal_Weapon_Base
{
    protected override void Startup()
    {
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
        {
            //no spawner found, add ours.
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Scatter>();
        }
        else
        {   //spawner found, but what type?
            //do not want base type. i want derived type
            if (m_ProjectileSpawner is ProjectileSpawner_Scatter)
            {
                //happy
            }
            else
            {
                //not happy
                Destroy(m_ProjectileSpawner);
                m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner_Scatter>();
            }
        }
    }

    public override void Update()
    {
        ReloadAmmo();
        base.Update();
    }

    protected override void postShoot()
    {
        if(m_DidShoot)
        {
            m_Ammo--;
        }
        base.postShoot();
    }

    protected override bool ShootConditions()
    {
        if (isReloading())
        {   //if we're out of ammo, cannot shoot
            return false;
        }
        //if we have ammo, check attack interval
        return base.ShootConditions();
    }

    protected override void SpawnProjectilesImpl(Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //see projectile spawner
    }
}
