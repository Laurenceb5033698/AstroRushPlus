using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// component for holding upgrade modules for player or ai ship.
/// is responsible for processing modules when new ones are added.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    //list of upgrade modules
    List<UpgradeModule> m_modules;

    void Start()
    {
        UpgradeModule module = new UpgradeModule();
        AddNewUpgrade(module);
    }

    
    void Update()
    {
        
    }


    //add new upgrade
    /// <summary>
    /// takes param module from player selection, or from system that adds specific modules.
    /// </summary>
    /// <param name="module">upgrade to add.</param>
    private void AddNewUpgrade(UpgradeModule module)
    {
        //validation? does module already exist?
        m_modules.Add(module);
    }
    //recalculate all upgrades (for spawning with modules already existing)
}
