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
class Arsenal : MonoBehaviour
{
    [SerializeField] private GameObject ship;

    private int NoWeapons = 3;
    [SerializeField] private List<Weapon> Weapons;//weapon prefabs
    private GameObject turret = null;
    private Weapon Gun = null;
    private int currentGun = 0;

    private void Start()
    {
        GetComponentsInChildren<Weapon>(Weapons);
        NoWeapons = Weapons.Count;
        foreach (Weapon item in Weapons)
        {
            item.SetShipObject(ship);
            //Debug.Log(item.gameObject.name);
        }
        Gun = Weapons[currentGun];
        Gun.enabled = true;
    }

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }

    public void ChangeGun( int a)
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
}