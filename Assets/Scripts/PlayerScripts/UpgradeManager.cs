using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// component for holding upgrade modules for player or ai ship.
/// is responsible for processing modules when new ones are added.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    //one upgrade manager per ship. Stats of ship related to manager.
    [SerializeField] public Stats shipStats;

    //list of upgrade modules
    [SerializeField] List<UpgradeModule> m_modules;

    //use this to test applying a module to a ship.
    [SerializeField] UModuleScriptable m_TestModule;
    [SerializeField] UModuleScriptable m_BaseStatsModule;

    void Start()
    {
        //start function is only properly called for enemyai, as the upgrade manager is on their gameobject.
        //for the player, upgrade manager is created when game starts.
        ApplyBaseStats();
    }

    public void ApplyBaseStats()
    {
        if (shipStats != null && m_BaseStatsModule != null)
        {
            AddNewModule(m_BaseStatsModule);
        }
        if (shipStats)
        {
            shipStats.PostLoaded();
        }
    }
    
    void Update()
    {
        
    }

    public void TestAddModule()
    {
        AddNewUpgrade(m_TestModule.Parse());
    }

    /// <summary>
    /// Called from Upgrade Pool Manager.
    /// adds a selected module.
    /// </summary>
    public void AddNewModule(UModuleScriptable _UModuleScriptable)
    {
        AddNewUpgrade(_UModuleScriptable.Parse());
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
        module.ProcessModule(ref shipStats);
    }
    //recalculate all upgrades (for spawning with modules already existing)


    public void RestartingGame()
    {
        m_modules.Clear();
    }
}
