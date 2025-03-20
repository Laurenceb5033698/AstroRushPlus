using UnityEngine;

[System.Serializable]
public class UModule_WeaponType : UpgradeModule
{
    //new weapon prefab
    [SerializeField] GameObject m_WeaponPrefab;

    //ctor from scriptable object data
    public UModule_WeaponType(GameObject _prefab)
    {
        m_WeaponPrefab = _prefab;
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

    protected override void ProcessImpl(ref Stats _shipStats)
    {
        ReplaceWeaponType(_shipStats.gameObject);
        //apply new stats after weapon object swapped.
        base.ProcessImpl(ref _shipStats);
    }
}
