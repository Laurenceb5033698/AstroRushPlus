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

}
