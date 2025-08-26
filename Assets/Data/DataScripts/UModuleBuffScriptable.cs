using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UModuleBuffScriptable", menuName = "Upgrade Modules/UModule Buff")]
public class UModuleBuffScriptable : UModuleScriptable
{
    //list of stats that are applied when buff is applied
    [SerializeField] protected List<BuffData> m_BuffStats;

    //called by Upgrademanager to create behaviour module using data
    override public UpgradeModule Parse()
    {
        return new UModule_Buff(m_UpgradeList, m_BuffStats);
    }
}
