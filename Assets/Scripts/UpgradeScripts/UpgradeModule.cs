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
    [SerializeField] List<Stat> m_upgradeList;


    public UpgradeModule(List<Stat> _list)
    {
        m_upgradeList = _list;
    }

    /// <summary>
    /// Validate ship stats before processing upgrade.
    /// </summary>
    public bool PreProcess(Stats _shipStats)
    {
        //verify ship stats list has allstat types assigned.
        if(_shipStats.block.statList.Count() != (int)StatType.NUM)
        {
            Debug.Log("Error UpgradeModule Preprocess: Ship stats list does not have all stat types.");
            return false;
        }
        //an upgrade can have no stats to apply and only have behaviour change.
        //in this case we still need to proceed with processImpl.
        return true;
    }

    /// <summary>
    /// Called Once on apply. or once on recalc
    /// Core function for applying module effects to ship.
    /// needs to know what it's applying its effects to, eg if given a playership or ai ship.
    /// </summary>
    public void ProcessModule(ref Stats _shipStats)
    {
        if (!PreProcess(_shipStats))
            return;
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
        DoOnce();
        AttachCallbacks(_shipStats.gameObject);
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
        RemoteApplyStatChanges(_shipBlock.statList, m_upgradeList);
    }

    public void RemoteApplyStatChanges(List<Stat> _shipStatsList, List<Stat> _upgradeList)
    {
        foreach (Stat UStat in m_upgradeList)
        {
            if (UStat.type == StatType.NUM)
            {
                Debug.Log("Error UpgradeModule: Upgrade Stat type incorrect.");
                continue;
            }
            //get enum of type, cast to int for index.
            int typeToUpgrade = (int)UStat.type;
            //upgrade process adds all modifiers from Ustat to target stat in shiplist.
            UStat.PassModifiers(_shipStatsList[typeToUpgrade]);
        }
    }

    /// <summary>
    /// for any callbacks that need to be attached to ship event caller.
    /// </summary>
    protected virtual void AttachCallbacks(GameObject _shipObject)
    {
        
    }

    protected virtual void DoOnce()
    {

    }
}
