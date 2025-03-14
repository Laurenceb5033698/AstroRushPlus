using UnityEngine;

[System.Serializable]
public class Stat
{
    //values to re-create max at anytime.
    [SerializeField] private float BaseStatMax;//default value that stat starts on
    [SerializeField] private float flat;         // flat bonus added to base value
    [SerializeField] private float scale;        //per-ship modifier. x<1.0 means total stat is reduced & growth is reduced
    [SerializeField] private float mod;          //% bonus mods that can be added/removed. 

    //Value is actual value in use
    public float Value { get; set; }
    public float Max { get; private set; }

    public Stat()
    {
        RecalculateMax();
        Value = Max;
    }
    public Stat(float _baseStatMax = 0.0f, float _flat = 0.0f, float _shipMod = 0.0f, float _bonusMod = 0.0f)
    {
        BaseStatMax = _baseStatMax;
        flat = _flat;
        scale = _shipMod;
        mod = _bonusMod;

        RecalculateMax();
        Value = Max;
    }

    private void RecalculateMax()
    {
        //general formula for calculating max. scale+mod cannot reduce lower than 5%
        Max = (BaseStatMax+flat) * Mathf.Max(0.05f, (1+scale) * (1+mod));
    }


    public void Recalculate()
    {
        //does not maintain proportion by default.
        //if max value is 0, and is never changed, cannot create proportion due to div by 0.
        //some stats do not use float, and 0 is a reasonable value.

        RecalculateMax();

        //if value is to be made to increase proportionally to max, then must be done outside stat.
    }

    public void InitialiseValue()
    {
        Value = Max;
    }

    //interfaces for modifiers.
    public void SetFlatMod(float _valToSet)
    {
        flat += _valToSet;
        Recalculate();
    }
    public void SetBonusMod(float _valToadd)
    {
        mod = _valToadd;  //cannot go lower than 5)%
        Recalculate();
    }

    public void SetShipMod(float _valToSet)
    {
        scale = _valToSet;
        Recalculate();

    }
}
