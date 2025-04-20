using TMPro;
using UnityEngine;

public class Script_UI_TextRenderer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    public void SetText(float Progress)
    {
        Text.SetText($"{(Progress * 100).ToString("N2")}%");
    }
}