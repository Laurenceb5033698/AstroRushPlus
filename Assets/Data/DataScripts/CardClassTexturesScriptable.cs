using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows assigning class textures and class text for upgrade cards display
/// </summary>
[CreateAssetMenu(fileName ="Card Class Details", menuName = "Upgrade Modules/Details")]
public class CardClassTexturesScriptable : ScriptableObject
{
    //set values in inspector
    [System.Serializable] public struct CardClass 
    {
        public CardType Type;
        public string DisplayClass;
        public Texture2D BackgoundTexture;
    }
    //readonly list pls.
    [SerializeField] private List<CardClass> CardClassDisplay;

    public CardClass GetClassDisplayDetails(CardType _type)
    {
        return CardClassDisplay.Find(i => i.Type == _type);
    }
}
