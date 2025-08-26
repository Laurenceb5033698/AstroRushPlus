using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Script_UI_Anim_EndOfRun_Entrance : MonoBehaviour
{
    public Image background; // The image with the background colour
    public GameObject dividingLineLeft; // The game object that contains the bar on the left of the screen and its other parts
    public GameObject dividingLineRight; // The game object that contains the bar on the right of the screen and its other parts
    public GameObject header; // The game object that contains the header and all its other parts
    public GameObject boxStats; // The game object that contains the stats box and all of its other parts
    public GameObject boxCurrency; // The game object that contains the stats box and all of its other parts
    public GameObject boxLoadout; // The game object that contains the stats box and all of its other parts
    public GameObject buttonLaunch; // The game object that contains the launch button
    public GameObject buttonHangar; // The game object that contains the launch button
    public float bgFadeIn = 0.5f; // How long it takes for the background colour to fade in
    public float bgAlpha = 0.5f; // The opacity of the background, the value scales between 0 - 1, 0 being transparent, and 1 being opaque
    public float dlMovement = 0.5f; // How long it takes the dividing lines to move into place
    public float boxFadeIn = 0.25f; // How long it takes for the boxes to fade in
    public bool upgradeChosen = false; // If the upgrade has been selected, triggers the exit animations

    void Start()
    {
        // Sets the background alpha to 0, as it needs to fade in from 0 next
        Color bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;

        // Sets the position of the dividing lines, header, and buttons to offscreen
        dividingLineLeft.transform.localPosition = new Vector3(-1920, 2160, 0); // This sets it to off screen left
        dividingLineRight.transform.localPosition = new Vector3(1920, -2160, 0); // This sets it to off screen right
        header.transform.localPosition = new Vector3(0, 1320, 0); // This sets it to off screen north
        buttonLaunch.transform.localPosition = new Vector3(-1024, 0, 0); // This sets it to off screen behind its mask to the left
        buttonHangar.transform.localPosition = new Vector3(-1024, 0, 0); // This sets it to off screen behind its mask to the left

        // Sets the transform of the boxes and turns them off
        boxStats.transform.localScale = new Vector3(0, 0, 0);
        boxStats.SetActive(false);
        boxCurrency.transform.localScale = new Vector3(0, 0, 0);
        boxCurrency.SetActive(false);
        boxLoadout.transform.localScale = new Vector3(0, 0, 0);
        boxLoadout.SetActive(false);

        // Starts off the animation 
        StartCoroutine(BackgroundAnimateIn());
        StartCoroutine(DividingLineLeftAnimateIn());
        StartCoroutine(DividingLineRightAnimateIn());
        StartCoroutine(HeaderAnimateIn());
        StartCoroutine(DelayBoxAnimateIn());
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

    IEnumerator DividingLineLeftAnimateIn() // Move the left dividing line onto screen with a lerp
    {
        float newPosition = dividingLineLeft.transform.localPosition.y;
        float startPosition = dividingLineLeft.transform.localPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 0f, elapsedTime / dlMovement);
            dividingLineLeft.transform.localPosition = new Vector3(-1920, newPosition, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DividingLineRightAnimateIn() // Move the right dividing line onto screen with a lerp
    {
        float newPosition = dividingLineRight.transform.localPosition.y;
        float startPosition = dividingLineRight.transform.localPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 0f, elapsedTime / dlMovement);
            dividingLineRight.transform.localPosition = new Vector3(1920, newPosition, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator HeaderAnimateIn() // Move the header onto screen with a lerp
    {
        float newPosition = header.transform.localPosition.y;
        float startPosition = header.transform.localPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 1080, elapsedTime / dlMovement);
            header.transform.localPosition = new Vector3(0, newPosition, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator DelayBoxAnimateIn() // Wait to turn on the upgrade cards, then turn them on in sequence
    {
        yield return new WaitForSeconds(.25f);
        StartCoroutine(BoxStatsAnimateIn());
        StartCoroutine(BoxCurrencyAnimateIn());
        StartCoroutine(BoxLoadoutAnimateIn());
        StartCoroutine(ButtonLaunchAnimateIn());
        StartCoroutine(ButtonHangarAnimateIn());
    }

    IEnumerator BoxStatsAnimateIn()
    {
        float scaleX = boxStats.transform.localScale.x;
        float elapsedTime = 0;
        boxStats.SetActive(true);

        while (elapsedTime < boxFadeIn)
        {
            scaleX = Mathf.Lerp(0, 1, elapsedTime / boxFadeIn);
            boxStats.transform.localScale = new Vector3(scaleX, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator BoxCurrencyAnimateIn()
    {
        float scaleX = boxCurrency.transform.localScale.x;
        float elapsedTime = 0;
        boxCurrency.SetActive(true);

        while (elapsedTime < boxFadeIn)
        {
            scaleX = Mathf.Lerp(0, 1, elapsedTime / boxFadeIn);
            boxCurrency.transform.localScale = new Vector3(scaleX, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator BoxLoadoutAnimateIn()
    {
        float scaleY = boxLoadout.transform.localScale.y;
        float elapsedTime = 0;
        boxLoadout.SetActive(true);

        while (elapsedTime < boxFadeIn)
        {
            scaleY = Mathf.Lerp(0, 1, elapsedTime / boxFadeIn);
            boxLoadout.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ButtonLaunchAnimateIn()
    {
        float newPosition = buttonLaunch.transform.localPosition.x;
        float startPosition = buttonLaunch.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < boxFadeIn)
        {
            newPosition = Mathf.Lerp(startPosition, 0f, elapsedTime / boxFadeIn);
            buttonLaunch.transform.localPosition = new Vector3(newPosition, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ButtonHangarAnimateIn()
    {
        float newPosition = buttonHangar.transform.localPosition.x;
        float startPosition = buttonHangar.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < boxFadeIn)
        {
            newPosition = Mathf.Lerp(startPosition, 0f, elapsedTime / boxFadeIn);
            buttonHangar.transform.localPosition = new Vector3(newPosition, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void Update() // Temporary checker for if the card has been chosen, this should be replaced onto the selection event and trigger the coroutine that way
    {
        if (upgradeChosen == true)
        {
            StartCoroutine(AnimationExitStart());
        }
    }

    IEnumerator AnimationExitStart() //Starts off all the exit animations at the same time
    {
        StartCoroutine(BackgroundAnimateOff());
        StartCoroutine(DividingLineLeftAnimateOff());
        StartCoroutine(DividingLineRightAnimateOff());
        StartCoroutine(HeaderAnimateOff());
        StartCoroutine(BoxStatsAnimateOff());
        StartCoroutine(BoxCurrencyAnimateOff());
        StartCoroutine(BoxLoadoutAnimateOff());
        StartCoroutine(ButtonLaunchAnimateOff());
        StartCoroutine(ButtonHangarAnimateOff());
        yield return null;
    }

    IEnumerator BackgroundAnimateOff() // Fades out the background colour
    {
        Color localColour = background.color;
        float elapsedTime = 0;

        while (elapsedTime < bgFadeIn)
        {
            localColour.a = Mathf.Lerp(bgAlpha, 0f, elapsedTime / bgFadeIn);
            background.color = localColour;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DividingLineLeftAnimateOff() // Moves the left dividing line off screen
    {
        float newPosition = dividingLineLeft.transform.localPosition.y;
        float startPosition = dividingLineLeft.transform.localPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, -2160f, elapsedTime / dlMovement);
            dividingLineLeft.transform.localPosition = new Vector3(-1920, newPosition, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    IEnumerator DividingLineRightAnimateOff() // Moves the right dividing line off screen
    {
        float newPosition = dividingLineRight.transform.localPosition.y;
        float startPosition = dividingLineRight.transform.localPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 2160f, elapsedTime / dlMovement);
            dividingLineRight.transform.localPosition = new Vector3(1920, newPosition, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator HeaderAnimateOff() // Moves the header off screen
    {
        float newPosition = header.transform.localPosition.y;
        float startPosition = header.transform.localPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 1320, elapsedTime / dlMovement);
            header.transform.localPosition = new Vector3(0, newPosition, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator BoxStatsAnimateOff()
    {
        float scaleX = boxStats.transform.localScale.x;
        float elapsedTime = 0;
        boxStats.SetActive(true);

        while (elapsedTime < boxFadeIn)
        {
            scaleX = Mathf.Lerp(1, 0, elapsedTime / boxFadeIn);
            boxStats.transform.localScale = new Vector3(scaleX, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        boxStats.SetActive(false);
    }
    IEnumerator BoxCurrencyAnimateOff()
    {
        float scaleX = boxCurrency.transform.localScale.x;
        float elapsedTime = 0;
        boxCurrency.SetActive(true);

        while (elapsedTime < boxFadeIn)
        {
            scaleX = Mathf.Lerp(1, 0, elapsedTime / boxFadeIn);
            boxCurrency.transform.localScale = new Vector3(scaleX, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        boxCurrency.SetActive(false);

    }

    IEnumerator BoxLoadoutAnimateOff()
    {
        float scaleY = boxLoadout.transform.localScale.y;
        float elapsedTime = 0;
        boxLoadout.SetActive(true);

        while (elapsedTime < boxFadeIn)
        {
            scaleY = Mathf.Lerp(1, 0, elapsedTime / boxFadeIn);
            boxLoadout.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        boxLoadout.SetActive(false);
    }

    IEnumerator ButtonLaunchAnimateOff()
    {
        float newPosition = buttonLaunch.transform.localPosition.x;
        float startPosition = buttonLaunch.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < boxFadeIn)
        {
            newPosition = Mathf.Lerp(startPosition, 1024f, elapsedTime / boxFadeIn);
            buttonLaunch.transform.localPosition = new Vector3(newPosition, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ButtonHangarAnimateOff()
    {
        float newPosition = buttonHangar.transform.localPosition.x;
        float startPosition = buttonHangar.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < boxFadeIn)
        {
            newPosition = Mathf.Lerp(startPosition, 1024f, elapsedTime / boxFadeIn);
            buttonHangar.transform.localPosition = new Vector3(newPosition, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}


