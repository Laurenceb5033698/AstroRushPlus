using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProjectileSpawner : MonoBehaviour
{
    //behaviour compenent that spawns projectiles in target direction at specified position
    
    protected GameObject m_spawnPrefab;

    public void Setup( GameObject _bulletPrefab)
    {
        m_spawnPrefab = _bulletPrefab;
    }

    public void Spawn(Vector3 _shootPosition, Quaternion _aimDirection, out List<GameObject> _projectiles)
    {
        _projectiles = SpawnImpl(_shootPosition, _aimDirection);
    }

    /// <summary>
    /// Standard one-bullet method. nothing fancy.
    /// </summary>
    /// <param name="_shootPosition"> World position of the objects when spawned.</param>
    /// <param name="_aimDirection"> Facing rotation of the object when spawned.</param>
    /// <returns> List of all bullets that were spawned.</returns>
    public virtual List<GameObject> SpawnImpl( Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //shoots 1 projectile, regardless of stats.
        //if you want a weapon to do something other than that, do it elsewhere. eg, numprojeciles increases dmg, goto setupBullet
        List<GameObject> bullets = new List<GameObject>();
        bullets.Add( Instantiate<GameObject>(m_spawnPrefab, _shootPosition, _aimDirection));
        
        return bullets;
    }

    //UTILS
    protected float GetStat(StatType _type)
    {
        float val = GetComponent<Universal_Weapon_Base>().GetStat( _type );
        return val;
    }

}
