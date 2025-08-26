using UnityEngine;
using System.Collections.Generic;

//required data to recreate a new buff
[System.Serializable]
public class BuffData
{
    //values for applied buff
    [SerializeField] public BuffType BuffType;
    [SerializeField] public StatType appliedStatType;
    [SerializeField] public BuffCondition condition;
    [SerializeField] public float value;
    [SerializeField] public float duration;
}

public class UModule_Buff : UpgradeModule
{
    List<BuffConditions> m_ConditionList;
    public UModule_Buff(List<Stat> _statList, List<BuffData> _buffdata) :base(_statList)
    {
        m_ConditionList = new List<BuffConditions>();
        foreach (BuffData data in _buffdata)
        {
            if (data.condition == BuffCondition.None)
            {
                Debug.Log("Error UModule_Buff: buffdata contians invlaid condition.");
                continue;
            }

            BuffConditions bCondition;
            if (IsEvent(data.condition))
            {
                bCondition = new EventBuffCondition(data);
            }
            else
            {
                bCondition = new ContinuousBuffCondition(data);
            }
            m_ConditionList.Add(bCondition);

        }
    }

    protected override void AttachCallbacks(GameObject _shipObject)
    {
        EventSource evtSrc = _shipObject.GetComponent<EventSource>();
        if (!evtSrc)
        {
            Debug.Log("Error UModule_Buff: No EventSource Found on ship object..");
            return;
        }

        foreach (BuffConditions cond in m_ConditionList)
        {
            if (cond is EventBuffCondition)
            {
                cond.Attach(evtSrc);
            }
        }

        /*
        //foreach (BuffData data in m_BuffData)
        //{
        //    if(data.condition == BuffCondition.None)
        //    {
        //        Debug.Log("Error UModule_Buff: buffdata contians invlaid condition.");
        //        continue;
        //    }
        //    EventSource evtSrc = _shipObject.GetComponent<EventSource>();
        //    if (!evtSrc)
        //    {
        //        Debug.Log("Error UModule_Buff: No EventSource Found on ship object..");
        //        continue;
        //    }
        //
        //    BuffConditions bCondition;
        //    if (IsEvent(data.condition))
        //    {
        //        bCondition = new EventBuffCondition(data);
        //        bCondition.Attach(evtSrc);
        //    }
        //    else
        //    {
        //        bCondition = new ContinuousBuffCondition(data);
        //    }
        //    m_ConditionList.Add(bCondition);
        //}
        */
    }

    public void RemoveFromEvent(GameObject _shipObject)
    {
        EventSource evtSrc = _shipObject.GetComponent<EventSource>();
        if (!evtSrc)
        {
            Debug.Log("Error UModule_Buff: No EventSource Found on ship object..");
            return;
        }
        foreach (BuffConditions cond in m_ConditionList)
        {
            if (cond is EventBuffCondition)
            {
                cond.Detach(evtSrc);
            }
        }
    }

    //Note: Requires updating when new events are implemented
    private bool IsEvent(BuffCondition _cond)
    {
        switch (_cond)
        {
            case BuffCondition.OutOfCombat: return false;
            case BuffCondition.DuringCombat:return false;
            case BuffCondition.LowHealth:   return false;
            case BuffCondition.MidHealth:   return false;
            case BuffCondition.HighHealth:  return false;
            case BuffCondition.FullHealth:  return false;
            case BuffCondition.Undamaged:   return false;
            case BuffCondition.LowShield:   return false;
            case BuffCondition.MidShield:   return false;
            case BuffCondition.HighShield:  return false;
            case BuffCondition.FullShield:  return false;
            case BuffCondition.OnPickup:    return true;
            case BuffCondition.OnKill:      return true;
            case BuffCondition.OnDamage:    return true;
            case BuffCondition.OnShoot:     return true;
            case BuffCondition.OnCollide:   return true;
            case BuffCondition.Channelling: return false;
            case BuffCondition.Stationary:  return false;
            case BuffCondition.Other:       return false;
            case BuffCondition.None:
            default:
                return false;
        }
    }
}
