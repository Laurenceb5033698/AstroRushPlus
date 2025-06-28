using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class ProjectileSpawner : MonoBehaviour
{
    //behaviour compenent that spawns projectiles in target direction at specified position
    public delegate void ProjectileSetupDelegate(GameObject _spawned);

    public void Spawn(GameObject _prefab, Vector3 _shootPosition, Quaternion _aimDirection, ProjectileSetupDelegate _SetupFunc)
    {
        List<GameObject>  _projectiles = new List<GameObject>();
        _projectiles = SpawnImpl(_prefab, _shootPosition, _aimDirection);

        //once spawned, initialises bullets with setup stats, and any other setup func, eg missile initilise method.
        foreach (GameObject _projectile in _projectiles)
        {
            _SetupFunc( _projectile);
        }
    }

    /// <summary>
    /// Standard one-bullet method. nothing fancy.
    /// </summary>
    /// <param name="_shootPosition"> World position of the objects when spawned.</param>
    /// <param name="_aimDirection"> Facing rotation of the object when spawned.</param>
    /// <returns> List of all bullets that were spawned.</returns>
    public virtual List<GameObject> SpawnImpl(GameObject _prefab, Vector3 _shootPosition, Quaternion _aimDirection)
    {
        //shoots 1 projectile, regardless of stats.
        //if you want a weapon to do something other than that, do it elsewhere. eg, numprojeciles increases dmg, goto setupBullet
        List<GameObject> bullets = new List<GameObject>();
        bullets.Add( Instantiate<GameObject>(_prefab, _shootPosition, _aimDirection));
        
        return bullets;
    }

    public IEnumerator SpawnAsync(float _betweenBurstTime, int _numBursts, GameObject _prefab, Vector3 _shootPosition, Quaternion _aimDirection, ProjectileSetupDelegate _SetupFunc)
    {
        for (int i = 0; i < _numBursts; i++)
        {
            Spawn(_prefab, _shootPosition, _aimDirection, _SetupFunc);

            yield return new WaitForSeconds(_betweenBurstTime);
        }
    }

    //UTILS
    protected float GetStat(StatType _type)
    {
        float val = GetComponent<Universal_Weapon_Base>().GetStat( _type );
        return val;
    }

}
