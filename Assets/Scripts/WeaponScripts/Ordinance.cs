using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//twin of weapon class
public class Ordinance : Universal_Weapon_Base {

    //shoots a projectile
    //[SerializeField] protected GameObject m_ProjectilePrefab;//bullet prefab
    //[SerializeField] protected GameObject m_Ship; //reference to ship
    

    //startup
    private void Awake()
    {
        Startup();
        SetupDelegate += SetupBullet;
    }
    /// <summary>
    /// Called on awake, this setups up self-specific references.
    /// Adds required components in start. Override for derrived classes to add specific components
    /// </summary>
    protected override void Startup()
    {   //adds required spawner component
        m_ProjectileSpawner = GetComponent<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
            m_ProjectileSpawner = this.gameObject.AddComponent<ProjectileSpawner>();

    }
    //projectile spawner

    //shoot lockout/attackspeed to stop spam

    //allow mulitple types of spawn prefabs/spawn types

    /// <summary>
    /// Different from Startup, this is called from outside to setup Ship specific references.
    /// </summary>
    /// <param name="obj"></param>
    public override void Setup(GameObject obj)
    {
        m_Ship = obj;
        ShipStats = m_Ship.GetComponent<Stats>();
        InterfaceStats = new IMissileStats(ShipStats);

    }

    public void Shoot(Vector3 _aimDir)
    {
        preShoot(_aimDir);
        ShootImpl();
        postShoot();
        
        //shootSound.Play();
    }

    //override protected void preShoot(Vector3 _aimDir)
    //{
    //    m_Firing = true;

    //}
    override protected void ShootImpl()
    {
        if (ShootConditions())
        {
            SpawnProjectiles();
            m_DidShoot = true;
        }
    }

    override protected void postShoot() 
    {
        if (m_DidShoot)
        {
            //spend ammo
            ShipStats.OrdinanceAmmo = -1;

            float attackInterval = 1 / (ShipStats.Get(StatType.gAttackspeed) < 0.1f ? 0.1f : ShipStats.Get(StatType.gAttackspeed));
            m_AttackInterval = Time.time + attackInterval + CalcBurstInterval();
        }

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
            m_ProjectileSpawner.Spawn(m_BulletPrefab, shootPosition, aimDirection, SetupDelegate);
            m_AttackInterval = Time.time + 2.0f;
        }
        else
        {
            float timeBetween = m_BurstDelay / numBursts;

            StartCoroutine(m_ProjectileSpawner.SpawnAsync(timeBetween, numBursts + 1, m_BulletPrefab, shootPosition, aimDirection, SetupDelegate));
            m_AttackInterval = Time.time + 2.0f + CalcBurstInterval();
        }

    }

    override protected void SetupBullet(GameObject _missile)
    {
        Stats shipStats = m_Ship.GetComponent<Stats>();

        _missile.GetComponent<Projectile>().SetupValues(m_Ship.tag, shipStats);
    }

    protected override float CalcBurstInterval()
    {
        float interval = 0;
        int amount = Mathf.FloorToInt(InterfaceStats.BurstAmount);

        if (amount > 0)
        {
            float attacksPerSec = amount / 1.0f;
            interval = amount * attacksPerSec * m_BurstDelay;

        }

        return interval;
    }

    protected override bool ShootConditions()
    { 
        if (Time.time > m_AttackInterval && ShipStats.HasAmmo())
        {
            return true;
        }
        return false;
    }
}
