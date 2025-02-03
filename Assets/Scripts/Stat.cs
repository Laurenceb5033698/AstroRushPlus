using UnityEngine;

[System.Serializable]
public class Stat
{
    //values to re-create max at anytime.
    [SerializeField] private float BaseStatMax = 10.0f;//default value that stat starts on
    [SerializeField] private float flat = 0.0f;         // flat bonus added to base value
    [SerializeField] private float scale = 1.0f;        //per-ship modifier. x<1.0 means total stat is reduced & growth is reduced
    [SerializeField] private float mod = 1.0f;          //% bonus mods that can be added/removed. 

    //Value is actual value in use
    public float Value { get; set; }
    public float Max { get; private set; }

    public Stat(float _baseStatMax = 10, float _flat = 0.0f, float _shipMod = 1.0f, float _bonusMod = 1.0f)
    {
        BaseStatMax = _baseStatMax;
        flat = _flat;
        scale = Mathf.Abs(_shipMod);
        mod = _bonusMod;

        RecalculateFinalStat();
        Value = Max;
    }

    private void RecalculateFinalStat()
    {
        //general formula for calculating max. scale+mod cannot reduce lower than 5%
        Max = (BaseStatMax+flat) * Mathf.Max(0.05f,scale * mod);
    }


    public void Recalculate()
    {
        //recalculate max value, but maintain stat proportion
        float oldpercentage = Value / Max;

        RecalculateFinalStat();
        Value = (Max * oldpercentage);

    }

    //interfaces for modifiers.
    public void SetFlatMod(float _valToSet)
    {
        flat += _valToSet;
        Recalculate();
    }
    public void SetBonusMod(float _valToadd)
    {
        mod = 1.0f + _valToadd;
        mod = Mathf.Max(0.05f, mod);  //cannot go lower than 5%
        Recalculate();
    }

    public void SetShipMod(float _valToSet)
    {
        scale = Mathf.Abs(_valToSet);
        Recalculate();

    }
}
