using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {

    [SerializeField] private GameObject ship;

    private Ordinance Gun;

    public delegate void MissileUpgrader(GameObject _missile);
    public MissileUpgrader UpgraderAction;

    private void Awake()
    {
        Gun = GetComponentInChildren<Ordinance>();
        if (Gun == null)
            Debug.Log("ERROR: Equipment: Ordinance not found in children.");
    }

    void Start() {
        if (Gun)
        {
            Gun.Setup(ship);
            Gun.SetupDelegate += MissileInitialise;
        }
    }

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
    }

    public void UseOrdinance(Vector3 Aimdir)
    {
        if (Gun) 
        {
            Gun.Shoot(Aimdir);            
        }
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
