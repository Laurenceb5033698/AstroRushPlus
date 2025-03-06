using UnityEngine;

//base module for upgrades
//each module represents a unique upgrade that can be picked ingame
//each upgrade represents a change in stats (added or removed)
//  and also a change in behaviour
//some modules have functions that need to be hooked up to callbacks
public class UpgradeModule
{



    //Stat modifiers;
    

    /// <summary>
    /// Core function for applying module effects to ship.
    /// needs to know what it's applying its effects to, eg if given a playership or ai ship.
    /// </summary>
    public void ProcessModule()
    {
        //preProcess();
        ProcessImpl();
        //postProcess();
    }

    /// <summary>
    /// Implementation of process. this gets overriden.
    /// by default it applies simple stat changes.
    /// </summary>
    protected void ProcessImpl()
    {
        ApplyStats();
        AttachCallbacks();
    }

    /// <summary>
    /// if any stats changes are set, this applies them to given ship.
    /// </summary>
    public void ApplyStats()
    {

    }

    /// <summary>
    /// for any callbacks that need to be attached to ship event caller.
    /// </summary>
    public void AttachCallbacks()
    {

    }
}
