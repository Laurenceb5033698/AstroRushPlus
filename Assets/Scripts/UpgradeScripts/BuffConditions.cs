using System.Collections.Generic;
using TMPro;
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
    protected bool m_isEventBased;
    [SerializeField]
    protected BuffCondition m_condition;
    public BuffConditions(BuffCondition _cond)
    {
        m_condition = _cond;
        m_isEventBased = IsEvent(_cond);
    }
    private bool IsEvent(BuffCondition _cond)
    {
        switch (_cond)
        {
            case BuffCondition.OutOfCombat:
                return false;
            case BuffCondition.DuringCombat:
                return false;
            case BuffCondition.LowHealth:
                return false;
            case BuffCondition.MidHealth:
                return false;
            case BuffCondition.HighHealth:
                return false;
            case BuffCondition.FullHealth:
                return false;
            case BuffCondition.Undamaged:
                return false;
            case BuffCondition.LowShield:
                return false;
            case BuffCondition.MidShield:
                return false;
            case BuffCondition.HighShield:
                return false;
            case BuffCondition.FullShield:
                return false;
            case BuffCondition.OnPickup:
                return true;
            case BuffCondition.OnKill:
                return true;
            case BuffCondition.OnDamage:
                return true;
            case BuffCondition.OnShoot:
                return true;
            case BuffCondition.OnCollide:
                return true;
            case BuffCondition.Channelling:
                return false;
            case BuffCondition.Stationary:
                return false;
            case BuffCondition.Other:
                return false;
            case BuffCondition.None:
            default:
                return false;
        }
    }
    virtual public void AttachToEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction) { }
    virtual public void RemoveFromEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction) { }
    virtual public bool Condition() { return false; }
}

/// <summary>
/// Attach buff to event
/// </summary>
public class EventBuffCondition : BuffConditions
{
    public EventBuffCondition(BuffCondition _cond) : base(_cond) 
    { }
    override public void AttachToEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction)
    {
        _shipEventSource.AttachEventGeneric(m_condition, _ActionFunction);
    }
    override public void RemoveFromEvent(EventSource _shipEventSource, EventSource.Interaction _ActionFunction) 
    {
        _shipEventSource.DetachEventGeneric(m_condition, _ActionFunction);
    }

}


/// <summary>
/// check stat, apply on while condition is true.
/// </summary>
public class ContinuousBuffCondition : BuffConditions
{
    public ContinuousBuffCondition(BuffCondition _cond) :base(_cond)
    {

    }
    public bool CheckCondition()
    {
        return false;
    }

}


//eventBuffs
public class OnKillEventBuffCondition : EventBuffCondition
{
    public OnKillEventBuffCondition(BuffCondition _cond) : base(_cond) 
    { }
    
}


