using UnityEngine;

[CreateAssetMenu(fileName = "UWeaponTypeScriptable", menuName = "Upgrade Modules/UWeaponTypeScriptable")]
public class UWeaponTypeScriptable : UModuleScriptable
{
    //a weapon module takes a prefab of the weapon to be instantiated.
    public GameObject m_WeaponPrefab;
    public ModulePoolScriptable m_SpecificPool;

    //called by Upgrademanager to create behaviour module using data
    override public UpgradeModule Parse()
    {
        return new UModule_WeaponType(m_UpgradeList, m_WeaponPrefab, m_SpecificPool);
    }
}
