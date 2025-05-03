using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardDetailsHandler : MonoBehaviour
{
    //data set in inspector
    [SerializeField] CardClassTexturesScriptable cardDisplayDetails;

    //object hierarchy references
    [SerializeField] TextMeshProUGUI Txt_Name;
    [SerializeField] TextMeshProUGUI Txt_Description;
    [SerializeField] TextMeshProUGUI Txt_Stats;
    [SerializeField] TextMeshProUGUI Txt_classText;
    [SerializeField] RawImage FillIcon;
    [SerializeField] RawImage IconBorder;
    [SerializeField] RawImage bgClassIcon;

    //card class associates class icon and text with specific icons.
    //CardType CardClass;
    public void SetDisplay(CardDisplayDetails _moduleDetails)
    {
        Txt_Name.text = _moduleDetails.Txt_name;
        Txt_Description.text = _moduleDetails.Txt_description;
        Txt_Stats.text = _moduleDetails.Txt_stat;
        FillIcon.texture = _moduleDetails.Fill_Icon;

        CardClassTexturesScriptable.CardClass classDetails = cardDisplayDetails.GetClassDisplayDetails(_moduleDetails.Type);
        Txt_classText.text = classDetails.DisplayClass;
        bgClassIcon.texture = classDetails.BackgoundTexture;
    }
}
