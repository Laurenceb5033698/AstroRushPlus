using UnityEngine;

[System.Serializable]
public class Stat
{

    [SerializeField] private int BaseStatMax = 100;     //default value that stat starts on
    [SerializeField] private float ShipMod = 1.0f;       //per-ship modifier. x<1.0 means total stat is reduced & growth is reduced
    [SerializeField] private float BonusMod = 1.0f;      //bonus mods that can be added/removed. 

    //Value is actual value in use
    public int Value { get; set; }
    public int Max { get; private set; }

    public Stat(int baseStatMax = 100, float shipMod = 1.0f, float bonusMod = 1.0f)
    {
        BaseStatMax = baseStatMax;
        ShipMod = Mathf.Abs(shipMod);
        BonusMod = bonusMod;

        RecalculateFinalStat();
        Value = Max;
    }




    private void RecalculateFinalStat()
    {
        Max = Mathf.FloorToInt((((float)BaseStatMax) * ShipMod * BonusMod));
    }

    //TODO: Refactor for upgerade slot implementation.
    public void Recalculate()
    {
        //recalculate max value, but maintain stat proportion
        float oldpercentage = (float)Value / Max;

        RecalculateFinalStat();
        Value = Mathf.FloorToInt((float)Max * oldpercentage);

    }


    public void SetBonusMod(float valToadd)
    {
        BonusMod = 1.0f + valToadd;
        BonusMod = Mathf.Max(0.05f, BonusMod);  //cannot go lower than 5%
        Recalculate();
    }

    public void SetShipMod(float valToSet)
    {
        ShipMod = Mathf.Abs(valToSet);
        Recalculate();

    }
}
