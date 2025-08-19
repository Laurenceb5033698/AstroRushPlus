using UnityEngine;

//type of buff that applies specified stattype
public class StatBuff : Buff
{
    StatType m_statType;
    //reference to ship's stats
    Stats m_shipStats;

    public StatBuff(StatType _statType, Stats _shipStats, BuffType _type, float _timer, float _value) 
        : base (_type, _timer, _value)
    {
        m_statType = _statType;
        m_shipStats = _shipStats;
    }

    public override void OnApply()
    {
        UpdateStat(m_value);
    }

    public override void OnRemove()
    {
        //undo stat change (stats are additive)
        UpdateStat(-m_value);
    }

    private void UpdateStat(float _val)
    {
        //for given stat type, add value to ship stats.
        Stat affectedStat = m_shipStats.block.Get(m_statType);
        switch(m_type)
        {
            case BuffType.FlatStat:
                affectedStat.SetFlatMod(_val);
                break;
            case BuffType.MultStat:
                affectedStat.SetBonusMod(_val);
                break;
            default:
                Debug.Log("Error StatBuff: Invalid bufftype for stat buff.");
                break;
        }
    }

}
