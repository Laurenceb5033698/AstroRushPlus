using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {

    [SerializeField] private GameObject ship;

    private int NoOrdinances = 3;
    [SerializeField] private List<Ordinance> ordini;//weapon prefabs

    private Ordinance Gun = null;
    private int currentGun = 0;

    public delegate void MissileUpgrader(GameObject _missile);
    public MissileUpgrader UpgraderAction;

    //all Ordini use from Ammo
    [SerializeField] private int AmmoCommon = 20;

    void Start() {
        GetComponentsInChildren<Ordinance>(ordini);
        NoOrdinances = ordini.Count;
        foreach (Ordinance item in ordini)
        {
            item.SetShipObject(ship);
            item.SetupDelegate += MissileInitialise;
            //Debug.Log(item.gameObject.name);
        }
        Gun = ordini[currentGun];
        Gun.enabled = true;
    }

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }

    public void RegisterUI()
    {//need UI elements for this (i guess we have 2 already, misslie + bfom)
        //deffo need UI system to house them though

        //UI_Game GameUI = (UI_Game)UIManager.GetGameUiObject();
        //GameUI.RegisterWeaponIcons(Weapons);
    }

    public int ChangeGun(int a)
    {
        Gun.enabled = false;
        if (a > 0)
        {
            currentGun += 1;
            if (currentGun >= NoOrdinances)
                currentGun = 0;
        }
        else if (a < 0)
        {
            currentGun -= 1;
            if (currentGun < 0)
                currentGun = NoOrdinances - 1;
        }
        Gun = ordini[currentGun];
        Gun.enabled = true;
        return currentGun;
    }

    public void UseOrdinance(Vector3 Aimdir)
    {
        if (Gun) {
            if (AmmoCommon > 0)
            {
                --AmmoCommon;
                Gun.Shoot(Aimdir);
                //    Quaternion temp = new Quaternion();
                //    temp.SetLookRotation(Aimdir, transform.up);
                //    turret.transform.rotation = temp;
            }
        }
    }

    public void AddAmmo(int val)
    {
        AmmoCommon = (AmmoCommon + val > 20) ? 20 : AmmoCommon + val;
    }
    public bool HasAmmo()
    {
        return AmmoCommon > 0;
    }
    public int GetAmmoCount()
    {
        return AmmoCommon;
    }


    //###############
    //Event Callbacks

    /// <summary>
    /// Called by missile weapon when a projectile is spawned.
    /// used to add components from upgrades.
    /// </summary>
    public void MissileInitialise(GameObject _missileObject)
    {
        //pass missile to event delegates so thay can do their upgrade things
        if (UpgraderAction != null)
            UpgraderAction.Invoke(_missileObject);
    }

    /// <summary>
    /// When a missile upgrade is picked, it will register itself here 
    /// </summary>
    /// <param name="_upgradeFunc"></param>
    public void RegisterToMissileUpgrader(MissileUpgrader _upgradeFunc)
    {
        UpgraderAction += _upgradeFunc;
    }

    public void DeRegisterFromMissileUpgrader(MissileUpgrader _upgradeFunc)
    {
        UpgraderAction -= _upgradeFunc;
    }
}
