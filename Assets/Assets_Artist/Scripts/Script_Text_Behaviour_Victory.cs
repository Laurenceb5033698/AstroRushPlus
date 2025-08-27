using System.Collections;
using TMPro;
using UnityEngine;

public class Script_Text_Behaviour_Victory : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float durationSpacing = 2f;
    public float spacingAmount = 60f;

    void Start()
    {
        text.characterSpacing = 0;
        StartCoroutine(Spacing());
    }

    IEnumerator Spacing()
    {
        float currentTime = 0f;
        while (currentTime < durationSpacing)
        {
            float currentSpacing = Mathf.Lerp(0f, spacingAmount, currentTime / durationSpacing);
            text.characterSpacing = currentSpacing;
            currentTime += Time.deltaTime;
            yield return null;
        }


        yield return null;
    }
}
