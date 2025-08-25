using UnityEngine;
using System.Collections.Generic;

//required data to recreate a new buff
public class BuffData
{
    //values for applied buff
    BuffType BuffType;
    StatType appliedStatType;
    float value;
    float duration;

}

public class UModule_Buff : UpgradeModule
{
    
    public UModule_Buff(List<Stat> _statList, List<BuffData> _BuffStats) :base(_statList)
    {

    }

    public void AttachToEvent()
    {
        //on kill
    }

    public void RemoveFromEvent()
    {
        throw new System.NotImplementedException();
    }
    //buff conditional
    //applies buff when condition is met, checked every update.


    //buff onEvent
    //applies buff when event happens

}
