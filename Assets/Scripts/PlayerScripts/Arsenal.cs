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

    private int NoWeapons = 3;
    [SerializeField] private List<Weapon> Weapons;//weapon prefabs
    private GameObject turret = null;
    private Weapon Gun = null;
    private int currentGun = 0;

    void Awake()
    {
        GetComponentsInChildren<Weapon>(Weapons);
        NoWeapons = Weapons.Count;
        foreach (Weapon item in Weapons)
        {
            item.SetShipObject(ship);
            item.enabled = false;
            //Debug.Log(item.gameObject.name);
        }
        Gun = Weapons[currentGun];
        Gun.enabled = true;
    }
    private void Start()
    {
        UpdateDamageFromAttackStat();
    }

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }

    public void RegisterUI()
    {
        UI_Game GameUI = (UI_Game)UIManager.GetGameUiObject();
        GameUI.RegisterWeaponIcons(Weapons);
    }

    public int ChangeGun( int a)
    {
        Gun.enabled = false;
        if (a > 0)
        {
            currentGun += 1;
            if (currentGun >= NoWeapons)
                currentGun = 0;
        }
        else if (a < 0)
        {
            currentGun -= 1;
            if (currentGun < 0)
                currentGun = NoWeapons-1;
        }
        Gun = Weapons[currentGun];
        Gun.enabled = true;
        Gun.OnSwappedTo();
        return currentGun;
    } 

    public void UpdateDamageFromAttackStat()
    {   //call this after adding new weapons and after upgrading from upgrade screen
        foreach (Weapon item in Weapons)
        {
            item.CalculateFinalDamage();
        }
    }

    public void FireWeapon(Vector3 Aimdir)
    {
        if (Gun)
        //{
            Gun.Shoot(Aimdir);
        //    Quaternion temp = new Quaternion();
        //    temp.SetLookRotation(Aimdir, transform.up);
        //    turret.transform.rotation = temp;
        //}
    }
    public void SwapTurret(GameObject prefab)
    {
        if (turret)
            Destroy(turret);
        
        turret = Instantiate<GameObject>(prefab, transform.position, transform.rotation, ship.transform);
    }

    public void volumeChanged(float v)
    {
        foreach (Weapon item in Weapons)
        {
            item.volumeChanged(v);
        }
    }
}