using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UModule_WeaponType : UpgradeModule
{
    //new weapon prefab
    [SerializeField] GameObject m_WeaponPrefab;
    [SerializeField] ModulePoolScriptable m_SpecificModulePoolScriptable;

    //ctor from scriptable object data
    public UModule_WeaponType(List<Stat> _list, GameObject _prefab, ModulePoolScriptable _poolScriptable) 
        : base(_list)
    {
        m_WeaponPrefab = _prefab;
        m_SpecificModulePoolScriptable = _poolScriptable;
    }


    //Specific behaviour that replaces ship weapon with given weapon script
    protected void ReplaceWeaponType(GameObject _shipObject)
    {
        //weapon stored as prefab.
        //arsenal handles replacement impl

        //getarsenal
        Arsenal shipArsenal = _shipObject.GetComponentInChildren<Arsenal>();
        shipArsenal.ChangeGun(m_WeaponPrefab);
    }

    //when picked, a weapon type upgrade sets the upgrade pool manager to add a new set of upgrades to the random pool.
    protected void SetSpecificWeaponPool()
    {
        UpgradePoolManager.instance.SetSpecificWeaponPool(m_SpecificModulePoolScriptable);
    }

    protected override void ProcessImpl(ref Stats _shipStats)
    {
        ReplaceWeaponType(_shipStats.gameObject);
        SetSpecificWeaponPool();
        //apply new stats after weapon object swapped.
        base.ProcessImpl(ref _shipStats);
    }
}
