using UnityEngine;

[CreateAssetMenu(fileName = "UWeaponTypeScriptable", menuName = "Upgrade Modules/UWeaponTypeScriptable")]
public class UWeaponTypeScriptable : ScriptableObject
{
    //a weapon module takes a prefab of the weapon to be instantiated.
    public GameObject m_WeaponPrefab;

    //called by Upgrademanager to create behaviour module using data
    public UModule_WeaponType Parse()
    {
        return new UModule_WeaponType(m_WeaponPrefab);
    }
}
