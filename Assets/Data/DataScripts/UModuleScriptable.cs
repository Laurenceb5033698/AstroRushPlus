using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

/// <summary>
/// Base class for all modules
/// has basic stats object 
/// </summary>
[CreateAssetMenu(fileName = "UModule", menuName = "Upgrade Modules/UModule")]
public class UModuleScriptable : ScriptableObject
{
    //stores details require for displaying upgrade as a card in ui.
    [SerializeField] public CardDisplayDetails DisplayDetails;
    //stores statblock that only contains modifiers set in inspector
    [SerializeField] protected List<Stat> m_UpgradeList;

    virtual public UpgradeModule Parse()
    {
        return new UpgradeModule(m_UpgradeList);
    }
}
