using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//base module for upgrades
//each module represents a unique upgrade that can be picked ingame
//each upgrade represents a change in stats (added or removed)
//  and also a change in behaviour
//some modules have functions that need to be hooked up to callbacks
[System.Serializable]
public class UpgradeModule 
{



    //Stat modifiers;
    //set via scriptable object in editor.
    StatBlock m_upgradeBlock;

    /// <summary>
    /// Called Once on apply. or once on recalc
    /// Core function for applying module effects to ship.
    /// needs to know what it's applying its effects to, eg if given a playership or ai ship.
    /// </summary>
    public void ProcessModule(ref Stats _shipStats)
    {
        //preProcess();
        ProcessImpl(ref _shipStats);
        //postProcess();
    }

    /// <summary>
    /// Implementation of process. this gets overriden.
    /// by default it applies simple stat changes.
    /// </summary>
    virtual protected void ProcessImpl(ref Stats _shipStats)
    {
        ApplyStats(ref _shipStats.block);
        AttachCallbacks();
    }

    /// <summary>
    /// Called once per update while attached to a manager.
    /// Some modules do nothing here.
    /// </summary>
    public void UpdateModule()
    {
        //preUpdate();
        UpdateImpl();
        //postUpdate();
    }

    /// <summary>
    /// override func for updating the module.
    /// </summary>
    virtual protected void UpdateImpl()
    {

    }

    /// <summary>
    /// if any stats changes are set, this applies them to given ship.
    /// </summary>
    public void ApplyStats(ref StatBlock _shipBlock)
    {
        //for each stat, add upgrade to ship
        RemoteApplyStatChanges(_shipBlock.statList, m_upgradeBlock.statList);
    }

    public void RemoteApplyStatChanges(List<Stat> _shipStatsList, List<Stat> _upgradeList)
    {

        for (int i=0; i<_shipStatsList.Count;i++)
        {
            _upgradeList[i].PassModifiers(_shipStatsList[i]);
        }
        
    }

    /// <summary>
    /// for any callbacks that need to be attached to ship event caller.
    /// </summary>
    public void AttachCallbacks()
    {

    }
}
