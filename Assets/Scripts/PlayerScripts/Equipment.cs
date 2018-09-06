using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {

    [SerializeField] private GameObject ship;

    private int NoOrdinances = 3;
    [SerializeField] private List<Ordinance> ordini;//weapon prefabs
    
    private Ordinance Gun = null;
    private int currentGun = 0;

    //all Ordini use from Ammo
    [SerializeField] private int AmmoCommon = 20;

    void Start () {
        GetComponentsInChildren<Ordinance>(ordini);
        NoOrdinances = ordini.Count;
        foreach (Ordinance item in ordini)
        {
            item.SetShipObject(ship);
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
}
