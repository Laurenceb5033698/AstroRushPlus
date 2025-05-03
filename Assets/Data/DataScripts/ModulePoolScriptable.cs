using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Holds permenant List of all modules for this specific pool.
/// List is readonly at runtime.
/// </summary>
[CreateAssetMenu(fileName = "UModule", menuName = "Upgrade Modules/Module Pool")]
public class ModulePoolScriptable : ScriptableObject
{
    [SerializeField] public List<UModuleScriptable> Pool;

}
