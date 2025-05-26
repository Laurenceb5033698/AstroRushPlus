using UnityEngine;

[CreateAssetMenu(fileName = "UMissileTypeScriptable", menuName = "Upgrade Modules/UMissileTypeScriptable")]
public class UMissileTypeScriptable : UModuleScriptable
{
    //a Missile type upgrade will attach a new behaviour component to the missile.
    [SerializeReference] public BaseMissileComponent m_MissileComponent;

    //called by Upgrademanager to create behaviour module using data
    override public UpgradeModule Parse()
    {
        return new UModule_MissileType(m_UpgradeList, m_MissileComponent);
    }
}
