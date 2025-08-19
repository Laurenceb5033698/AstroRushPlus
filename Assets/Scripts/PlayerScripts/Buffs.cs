using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds list of all buffs affecting the ship with these stats.
/// Requires a Stat component on the same object to work.
/// </summary>
public class Buffs : MonoBehaviour
{
    [SerializeField]
    private List<Buff> m_BuffList = new List<Buff>();


    //NOTE: this currently does not distinguish buffs as unique effects.
    //applying the same buff twice or more will simply add more instances of that effect.
    //in future, this must be able to find out if the buff has already been applied.
    //  if found, it can then add a stack, or extend duration, or both.
    //  stacked buffs must beable to manage stat changes properly.
    //  temporary buffs must have a non-destructive effect on stats.

    public void AddStatBuff(StatType _stat, BuffType _type, float _timer, float _value)
    {
        Stats shipStats = GetComponent<Stats>();
        AddInternal(new StatBuff(_stat, shipStats, _type, _timer, _value));
    }

    //this buff created outside of buffs manager (eg passive shield regen after not being hit)
    public void AddExistingBuff(Buff _buffToAdd)
    {
        AddInternal(_buffToAdd);
    }

    private void AddInternal(Buff _buffToAdd)
    {
        m_BuffList.Add(_buffToAdd);
        _buffToAdd.OnApply();
    }

    private void Update()
    {
        //only process buff timers while in main gamestate.
        //prevents things like buffs timing out during loading/warping/picking upgrade etc.
        //  However, damaging buffs could also cause player to die after loading and entering new stage...
        if (ServicesManager.Instance.GameStateService.GameState == GameState.MAINGAME) 
        {
            UpdateBuffs();
        }
    }

    public void UpdateBuffs() 
    {
        float dt = Time.deltaTime;

        //iterate backwards through list. allows removal if buff expired.
        for (int i = m_BuffList.Count - 1; i >= 0; i--)
        {
            Buff buff = m_BuffList[i];
            buff.UpdateTimer(dt);
            if (buff.expired())
            {
                buff.OnRemove();
                m_BuffList.RemoveAt(i);
            }
        }
    }

    public void RemoveExistingBuff(Buff _buffToRemove)
    {
        _buffToRemove.OnRemove();
        if (!m_BuffList.Remove(_buffToRemove))
        {
            Debug.Log("Error Buffs: could not remove existing buff; " + this.gameObject.name);
        }
    }
    public void RemoveBuff()
    {
        //somehow find buff in question, and either set timer to expire, or remove immediately.
    }

}
