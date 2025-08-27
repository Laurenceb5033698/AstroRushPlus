using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Script_UI_Anim_Victory_Entrance : MonoBehaviour
{
    public Image background; // The image with the background colour
    public Image skipProgress; // The image of the skip progress bar
    public RawImage skipInput; // The raw image that contains the input for skipping
    public TextMeshProUGUI textSkip; // The TMP that contains the skip text
    public GameObject Canvas; // The game object containing the whole canvas
    public float bgFadeIn = 0.5f; // How long it takes for the background colour to fade in
    public float skipFadeIn = 0.5f; // How long it takes for the skip function to fade in
    public float bgAlpha = 0.5f; // The opacity of the background, the value scales between 0 - 1, 0 being transparent, and 1 being opaque
    private float skipAlpha = 1f; // The opactiy of the skip function, the value scales between 0 - 1, 0 being transparent, and 1 being opaque
    public float skipSpeed = 0.05f; // How fast the skip speed is, the skip speed will be added together from zero every 0.01 seconds, and the skip will trigger at a value of 1
    private float skipFadeOut = 2f; // How long it takes for the skip function to fade out if it isn't being prompted
    private bool skipping = false; // Bool to check whether or not the player is currently skipping the cutscene
    private bool skipComplete = false; //Bool to chekc whether or not the player has chosen to skip the cutscene

    void Start()
    {
        // Sets alpha of the background, and the skip components to 0
        Color bgColor = background.color;
        bgColor.a = 0;
        Color skipColor = skipProgress.color;
        skipColor.a = 0;
        background.color = skipColor;
        skipProgress.color = skipColor;
        skipInput.color = skipColor;
        textSkip.alpha = 0;

        // Starts off the animation 
        StartCoroutine(BackgroundAnimateIn());

    }

    IEnumerator BackgroundAnimateIn()
    {
        Color localColour = background.color;
        float elapsedTime = 0;

        while (elapsedTime < bgFadeIn)
        {
            localColour.a = Mathf.Lerp(0f, bgAlpha, elapsedTime / bgFadeIn);
            background.color = localColour;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            skipping = true;
            if (textSkip.alpha < 0.1f)
            {
                StartCoroutine(RevealSkipping());
                StartCoroutine(HideSkipping());
                StartCoroutine(StartSkipping());
            }
            else
            {
                // Creates a colour with a full alpha for the skip function to reference later
                Color skipColorFull = skipProgress.color;
                skipColorFull.a = 1f;

                textSkip.alpha = 1f;
                skipInput.color = skipColorFull;
                skipProgress.color = skipColorFull;
            }
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            skipping = false;
            StartCoroutine(StartSkipping());
        }

        if (skipProgress.fillAmount == 1)
        {
            skipComplete = true;
        }

        if (skipComplete == true)
            {
                Canvas.SetActive(false);
            }
    }

    IEnumerator RevealSkipping()
    {

        Color localColour = skipProgress.color;
        float elapsedTime = 0;

        while (elapsedTime < skipFadeIn)
        {
            localColour.a = Mathf.Lerp(0f, skipAlpha, elapsedTime / skipFadeIn);
            skipProgress.color = localColour;
            skipInput.color = localColour;
            textSkip.color = localColour;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator HideSkipping()
    {
        yield return new WaitForSeconds(skipFadeOut);
        Color localColour = skipProgress.color;
        float elapsedTime = 0;

        while (elapsedTime < skipFadeIn)
        {
            localColour.a = Mathf.Lerp(skipAlpha, 0f, elapsedTime / skipFadeIn);
            skipProgress.color = localColour;
            skipInput.color = localColour;
            textSkip.color = localColour;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator StartSkipping()
    
    {
        if (skipping)
        {
            float amount = skipProgress.fillAmount;
            while (amount < 1 & skipping)
            {
                amount += skipSpeed;
                skipProgress.fillAmount = amount;
                yield return new WaitForSeconds(0.01f);
            }
            if (amount >= 1 + skipSpeed)
            {
                amount = 1;
                skipProgress.fillAmount = amount;
            }
        }
        else
        {
            float amount = skipProgress.fillAmount;
            while (amount > 0 & !skipping)
            {
                amount -= skipSpeed;
                skipProgress.fillAmount = amount;
                yield return new WaitForSeconds(0.01f);

                if (amount <= 0 + skipSpeed)
                {
                    amount = 0;
                    skipProgress.fillAmount = amount;
                }
            }
        }
    }

}