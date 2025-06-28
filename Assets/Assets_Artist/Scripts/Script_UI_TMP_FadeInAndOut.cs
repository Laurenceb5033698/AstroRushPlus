using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Threading;
public class Script_UI_TMP_FadeInAndOut : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public float FadeInDuation;
    public float FadeOutDuation;
    public float TextLengthDuration;
    public void Start()
    {
        StartCoroutine(FadeIn());
        //StartCoroutine(FadeTextIn());
        textDisplay.alpha = 0f;
    }

    //private IEnumerator FadeTextIn()
    //{
    //textDisplay.CrossFadeAlpha(0, 2f, true);
    //}

    private void Update()
    {
        TextLengthDuration -= Time.deltaTime;
        if (TextLengthDuration <= 0.0f)
        {
            TimerEnded();
        }
    }

    private IEnumerator FadeIn()
    {
        float currentTime = 0f;
        while (currentTime < FadeInDuation)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / FadeInDuation);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void TimerEnded()
    {
        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        float currentTime = 0f;

        while (currentTime < FadeOutDuation)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / FadeOutDuation);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
