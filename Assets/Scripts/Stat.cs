using UnityEngine;
using UnityEngine.Purchasing;

[System.Serializable]
public class Stat
{
    //values to re-create max at anytime.
    [SerializeField] private float BaseStatMax;//default value that stat starts on
    [SerializeField] private float flat;         // flat bonus added to base value
    [SerializeField] private float scale;        //per-ship modifier. x<1.0 means total stat is reduced & growth is reduced
    [SerializeField] private float mod;          //% bonus mods that can be added/removed. 

    public float Max { get; private set; }

    public Stat()
    {
        //this does not work with inspector-set values.
        RecalculateMax();
    }

    public float Get()
    {
        return Max;
    }

    private float RecalculateMax()
    {
        //general formula for calculating max. scale+mod cannot reduce lower than 5%
        Max = (BaseStatMax+flat) * Mathf.Max(0.05f, (1+scale) * (1+mod));
        return Max;
    }


    public void Recalculate()
    {
        RecalculateMax();
    }


    //interfaces for modifiers.
    public void SetFlatMod(float _valToSet)
    {
        flat += _valToSet;
        Recalculate();
    }
    public void SetBonusMod(float _valToadd)
    {
        mod += _valToadd;  //cannot go lower than 5)%
        Recalculate();
    }

    public void SetShipMod(float _valToSet)
    {
        scale += _valToSet;
        Recalculate();

    }
}
