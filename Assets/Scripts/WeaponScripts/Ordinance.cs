using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//twin of weapon class
public class Ordinance : MonoBehaviour {

    //shoots a projectile
    [SerializeField] protected GameObject m_ProjectilePrefab;//bullet prefab
    [SerializeField] protected GameObject ship; //reference to ship
    public ProjectileSpawner m_ProjectileSpawner;

    public ProjectileSpawner.ProjectileSetupDelegate SetupDelegate;

    [SerializeField] protected int Damage = 5;
    [SerializeField] protected float Speed = 20f;
    protected float m_attackInterval = 0.0f;

    [SerializeField] protected float m_BurstDelay = 0.2f;

    //[SerializeField] protected AudioSource shootSound;


    //get ship stats

    //startup
    private void Awake()
    {
        Startup();
        SetupDelegate += SetupBullet;
    }
    /// <summary>
    /// Adds required components in start. Override to add specific components
    /// </summary>
    protected virtual void Startup()
    {   //adds required spawner component
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner>();

    }
    //projectile spawner

    //shoot lockout/attackspeed to stop spam

    //allow mulitple types of spawn prefabs/sapwn types

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }
    public void spawnProjectile(Vector3 aimDir, out List<GameObject> _list)
    {//spawn pattern for missile ordianance

        //VERY TEMPORARY
        Stats shipstats = ship.GetComponent<Stats>();

        GameObject mBullet;

        mBullet = (GameObject)Instantiate(m_ProjectilePrefab, ship.transform.position + aimDir * 8f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(ship.tag, shipstats);

        _list = new List<GameObject>();
        _list.Add(mBullet);
        
    }

    public void Shoot(Vector3 direction)
    {
        //use burst
        //List<GameObject> list;
        Stats shipStats = ship.GetComponent<Stats>();
        Vector3 shootPosition = ship.transform.position + direction * 8f;
        Vector3 toIndicator = (shootPosition - ship.transform.position).normalized;
        Quaternion aimDirection = Quaternion.LookRotation(toIndicator, Vector3.up);

        if (m_attackInterval > Time.time)
        {
            return;
        }


        int numBursts = Mathf.FloorToInt(shipStats.Get(StatType.mBurst));
        if (numBursts == 0)
        {
            m_ProjectileSpawner.Spawn(m_ProjectilePrefab, shootPosition, aimDirection, SetupDelegate);
            m_attackInterval = Time.time + 2.0f;
        }
        else
        {
            float timeBetween = m_BurstDelay / numBursts;

            StartCoroutine( m_ProjectileSpawner.SpawnAsync(timeBetween, numBursts+1, m_ProjectilePrefab, shootPosition, aimDirection, SetupDelegate));
            m_attackInterval = Time.time + 2.0f + CalcBurstInterval();
        }

        //shootSound.Play();
    }



    public void SetupBullet(GameObject _missile)
    {
        Stats shipStats = ship.GetComponent<Stats>();

        _missile.GetComponent<Projectile>().SetupValues(ship.tag, shipStats);
    }

    protected float CalcBurstInterval()
    {
        Stats shipStats = ship.GetComponent<Stats>();
        float interval = 0;
        int amount = Mathf.FloorToInt(shipStats.Get(StatType.mBurst));

        if (amount > 0)
        {
            float attacksPerSec = amount / 2.0f;
            interval = amount * attacksPerSec * m_BurstDelay;

        }

        return interval;
    }
}
