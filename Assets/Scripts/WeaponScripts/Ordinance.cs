using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//twin of weapon class
public class Ordinance : Universal_Weapon_Base {

    //shoots a projectile
    [SerializeField] protected GameObject m_ProjectilePrefab;//bullet prefab
    [SerializeField] protected GameObject ship; //reference to ship
    //public ProjectileSpawner m_ProjectileSpawner;

    //public ProjectileSpawner.ProjectileSetupDelegate SetupDelegate;


    //[SerializeField] protected AudioSource shootSound;


    //get ship stats

    //startup
    private void Awake()
    {
        Startup();
        InterfaceStats = new IMissileStats(ShipStats);
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

    public void Shoot(Vector3 _aimDir)
    {
        preShoot(_aimDir);
        ShootImpl();
        postShoot();
        
        //shootSound.Play();
    }

    virtual protected void ShootImpl()
    {
        if (ShootConditions())
        {
            SpawnProjectiles();
            m_DidShoot = true;
        }
    }
    virtual protected void postShoot()
    {

    }

    override protected void SpawnProjectiles()
    {
        //use burst
        //List<GameObject> list;
        Stats shipStats = m_Ship.GetComponent<Stats>();
        Vector3 shootPosition = m_AimingIndicator.transform.position;
        Vector3 toIndicator = (shootPosition - m_AimingIndicatorHolder.transform.position).normalized;
        Quaternion aimDirection = Quaternion.LookRotation(toIndicator, Vector3.up);

        if (m_AttackInterval > Time.time)
        {
            return;
        }


        int numBursts = Mathf.FloorToInt(InterfaceStats.BurstAmount);
        if (numBursts == 0)
        {
            m_ProjectileSpawner.Spawn(m_ProjectilePrefab, shootPosition, aimDirection, SetupDelegate);
            m_AttackInterval = Time.time + 2.0f;
        }
        else
        {
            float timeBetween = m_BurstDelay / numBursts;

            StartCoroutine(m_ProjectileSpawner.SpawnAsync(timeBetween, numBursts + 1, m_ProjectilePrefab, shootPosition, aimDirection, SetupDelegate));
            m_AttackInterval = Time.time + 2.0f + CalcBurstInterval();
        }

    }

    override protected void SetupBullet(GameObject _missile)
    {
        Stats shipStats = m_Ship.GetComponent<Stats>();

        _missile.GetComponent<Projectile>().SetupValues(m_Ship.tag, shipStats);
    }

    protected float CalcBurstInterval()
    {
        Stats shipStats = m_Ship.GetComponent<Stats>();
        float interval = 0;
        int amount = Mathf.FloorToInt(InterfaceStats.BurstAmount);

        if (amount > 0)
        {
            float attacksPerSec = amount / 2.0f;
            interval = amount * attacksPerSec * m_BurstDelay;

        }

        return interval;
    }

    protected override bool ShootConditions()
    {

        return true;
    }
}
