using UnityEngine;
using UnityEngine.UI;

//Card Type enum links background class graphics and border class text
public enum CardType
{
    None,
    Weapon,
    Missile,
    Ship,
    Generic,
    Dual,
    Tri,
    Railgun,
    Shotgun,
    Arc,
    Minigun,
    NUM
};

//struct that defines all detials require to fully change an upgrad Card display.
[System.Serializable]
public struct CardDisplayDetails
{
    public string Txt_name;
    public string Txt_description;
    public string Txt_stat;
    public CardType Type;
    public Texture2D Fill_Icon;
};