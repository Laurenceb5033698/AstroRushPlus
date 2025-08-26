using System.Collections.Generic;
using UnityEngine;
public enum BuffCondition {
    OutOfCombat,
    DuringCombat,
    Undamaged,
    LowHealth,
    MidHealth,
    HighHealth,
    FullHealth,
    LowShield,
    MidShield,
    HighShield,
    FullShield,
    OnPickup,
    OnKill,
    OnDamage,
    OnShoot,
    OnCollide,
    Channelling,
    Stationary,
    Other,
    None
}

//extensive list of all conditions that could apply a buff to player or AI ships.

//handles application of buff, and checking of condition
//  this interface is used to extend a buff upgrade module, so that a buff condition can be checked using upgrade module access.
public abstract class BuffConditions
{
    protected BuffData m_buffData;

    public BuffConditions(BuffData _data)
    {
        m_buffData = _data;
    }
    public void Attach(EventSource _evtSrc)
    {
        AttachToEvent(_evtSrc, Callback);
    }
    public void Detach(EventSource _evtSrc)
    {
        RemoveFromEvent(_evtSrc, Callback);
    }

    virtual public void AttachToEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction) { }
    virtual public void RemoveFromEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction) { }
    virtual public bool Condition() { return false; }

    virtual public void Callback(GameObject _HostObject, GameObject _targetObject)
    {
        switch (m_buffData.BuffType) 
        {
            case BuffType.FlatStat:
            case BuffType.MultStat:
                AddStatBuff(_HostObject, _targetObject);
                break;
            case BuffType.Effect:
                break;
                case BuffType.NONE:
            default:
                break;
        }
    }

    virtual protected void AddStatBuff(GameObject _HostObject, GameObject _targetObject)
    {
        Stats hostStats = _HostObject.GetComponent<Stats>();
        if (hostStats)
        {
            hostStats.m_Buffs.AddStatBuff(m_buffData.appliedStatType, m_buffData.BuffType, m_buffData.duration, m_buffData.value);
        }
    }

    public BuffCondition GetConditionType() { return m_buffData.condition; }
}

/// <summary>
/// Attach buff to event
/// </summary>
public class EventBuffCondition : BuffConditions
{
    public EventBuffCondition(BuffData _data) : base(_data) 
    { }
    override public void AttachToEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction)
    {
        _shipEventSource.AttachEventGeneric(m_buffData.condition, _ActionFunction);
    }
    override public void RemoveFromEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction) 
    {
        _shipEventSource.DetachEventGeneric(m_buffData.condition, _ActionFunction);
    }

}


/// <summary>
/// check stat, apply on while condition is true.
/// </summary>
public class ContinuousBuffCondition : BuffConditions
{
    public ContinuousBuffCondition(BuffData _data) :base(_data)
    {    }
    public bool CheckCondition()
    {
        return false;
    }
}
