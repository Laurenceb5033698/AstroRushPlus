using UnityEngine;

/// <summary>
/// Base class for all modules
/// has basic stats object 
/// </summary>
[CreateAssetMenu(fileName = "UModule", menuName = "Scriptable Objects/UModule")]
public class UModuleScriptable : ScriptableObject
{
    //stores statblock that only contains modifiers set in inspector
    //StatBlock m_upgradeStatBlock = new StatBlock();
}
