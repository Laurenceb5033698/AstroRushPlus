//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//namespace Assets.Scripts
//{
    
//}
public class Arsenal : MonoBehaviour
{
    [SerializeField] private GameObject ship;

    //single weapon owned. is replaced via picking weapon type upgrade.
    [SerializeField] private Universal_Weapon_Base m_UWeapon;
    
    void Awake()
    {
        m_UWeapon = GetComponentInChildren<Universal_Weapon_Base>();
    }
    private void Start()
    {
        if (m_UWeapon)
            m_UWeapon.Setup(ship);

        UpdateDamageFromAttackStat();
    }

    public void SetShipObject(GameObject _obj)
    {
        ship = _obj;
    }

    /// <summary>
    /// Called when a new weapon is chosen via upgrade.
    /// replaces existing weapon for complete new one from prefab.
    /// </summary>
    public void ChangeGun( GameObject _newPrefab )
    {
        //float getvolume = m_UWeapon.gameObject.GetComponent<AudioSource>().volume;
        m_UWeapon.transform.parent = null;
        Destroy(m_UWeapon.gameObject);
        //create
        GameObject newWeapon = Instantiate<GameObject>(_newPrefab, this.transform);
        //set volume.
        //newWeapon.GetComponent<AudioSource>().volume = getvolume;

        m_UWeapon = newWeapon.GetComponent<Universal_Weapon_Base>();
        m_UWeapon.Setup(ship);
    }

    //Obselete: stats used directly in weapon
    public void UpdateDamageFromAttackStat()
    {   //call this after adding new weapons and after upgrading from upgrade screen
        
    }

    public void FireWeapon(Vector3 _aimDir)
    {
        if (m_UWeapon)
            m_UWeapon.Shoot(_aimDir);
    }

    public void VolumeChanged(float v)
    {
        m_UWeapon.VolumeChanged(v);
    }


    //###############//
    //Weapon Callbacks

    /// <summary>
    /// When a bullet first hits an object it calls this.
    /// Any subsequent hits in the bullets path will call onBulletHit.
    /// </summary>
    /// <param name="other">Other object Hit, either Enemy AI or Asteroid.</param>
    public void OnBulletFirstHitCAllback(GameObject _other)
    {
        //called once by bullet on the first thing it hits.
    }

    /// <summary>
    /// when a bullet hits, do this to its target
    /// </summary>
    public void OnBulletHitCallback(GameObject _other)
    {
        //called by bullet when it hits anything (exluding other bullets)
    }

    /// <summary>
    /// do this when a bullet self destroys (either on last hit or timeout)
    /// Eg spawns aoe around bullet.
    /// </summary>
    public void OnBulletExpireCallback()
    {
        //called by bullet belonging to this ship when it expires.
    }

    /// <summary>
    /// When an Enemy target is killed by this ship, apply these effects
    /// </summary>
    public void OnKillTarget()
    {
        //called by enemy target that was 'killed'.
    }

    /// <summary>
    /// Callback for when an non-enemy object is destroyed by this ship. eg an asteroid.
    /// </summary>
    public void OnObjectDestroyed()
    {
        //called by object that was 'destroyed'.
    }
}