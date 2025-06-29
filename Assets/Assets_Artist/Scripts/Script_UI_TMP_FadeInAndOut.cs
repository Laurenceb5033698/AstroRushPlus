using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Threading;
using UnityEngine.Analytics;
public class Script_UI_TMP_FadeInAndOut : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public GameObject TriggerNextText;
    public float FadeInDuation;
    public float FadeOutDuation;
    public float TextLengthDuration;
    public Boolean TriggerSelf;
    private Boolean TextLive = false;

    
    public void Start()
    {
        textDisplay.alpha = 0f;

        if (TriggerSelf == true)
        {
            StartTextAnimation();
        }
    }

    public void StartTextAnimation()
    {
            StartCoroutine(FadeIn());
            textDisplay.alpha = 0f;
            TextLive = true;
    }


    private void Update()
    {
        if (TextLive == true)
        {
            TextLengthDuration -= Time.deltaTime;
            if (TextLengthDuration <= 0.0f)
            {
                TimerEnded();
            }
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

   private void TriggerNext()
  {
    
  }
}
