using UnityEngine;

[CreateAssetMenu(fileName = "UMS_", menuName = "Upgrade Modules/USpecific/Salvo")]
public class UModuleScriptable_Salvo : UModuleScriptable
{
    public override UpgradeModule Parse()
    {
        return new UModule_Salvo(m_UpgradeList);
    }
}
