using UnityEngine;

public enum BuffType
{
    FlatStat,
    MultStat,
    Effect,
    NONE
}

/// <summary>
/// A singular buff 
/// </summary>
public abstract class Buff
{
    protected BuffType m_type;
    protected float m_timer;
    protected float m_value;

    public Buff(BuffType _type, float _timer, float _value)
    {
        m_type = _type;
        m_timer = _timer;
        m_value = _value;
    }

    //  some buffs might want to stack their effect.
    //however stacking can apply in several different ways.
    //int m_stacks;
    //  property
    //int Stacks {  get { return m_stacks; } set { m_stacks += value; } }

    //  condition for applying is handled by upgrade that adds the buff.
    public abstract void OnApply();

    //  removed when timer expires, or cleansed or overriden.
    public abstract void OnRemove();

    public void UpdateTimer(float _deltaTime)
    {
        m_timer -= _deltaTime;
    }

    public void SetTimer(float _value)
    {
        m_timer = _value;
    }

    public bool expired()
    {
        return m_timer <= 0;
    }
}
