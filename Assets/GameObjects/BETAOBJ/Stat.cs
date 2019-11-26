using UnityEngine;

[System.Serializable]
public class Stat
{

    [SerializeField] private float BaseStatMax = 100;     //default value that stat starts on
    [SerializeField] private float ShipMod = 1.0f;       //per-ship modifier. x<1.0 means total stat is reduced & growth is reduced
    [SerializeField] private float BonusMod = 1.0f;      //bonus mods that can be added/removed. 

    //Value is actual value in use
    public float Value { get; set; }
    public float Max { get; private set; }

    public Stat(float baseStatMax = 100, float shipMod = 1.0f, float bonusMod = 1.0f)
    {
        BaseStatMax = baseStatMax;
        ShipMod = Mathf.Abs(shipMod);
        BonusMod = bonusMod;

        RecalculateFinalStat();
        Value = Max;
    }




    private void RecalculateFinalStat()
    { 
        Max = (BaseStatMax * ShipMod * BonusMod);
    }

    public void Recalculate()
    {
        //recalculate max value, but maintain stat proportion
        float oldpercentage = Value / Max;

        RecalculateFinalStat();
        Value = (Max * oldpercentage);

    }


    public void AddToBonusMod(float valToadd)
    {
        BonusMod += valToadd;
        BonusMod = Mathf.Max(0.05f, BonusMod);  //cannot go lower than 5%
        Recalculate();
    }

    public void SetShipMod(float valToSet)
    {
        ShipMod = Mathf.Abs(valToSet);
        Recalculate();

    }
}
