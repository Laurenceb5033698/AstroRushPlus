
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Script_UI_Ship_Systems_1 : MonoBehaviour
{
    public Image background; //The image with the background colour
    public RawImage backgroundGrid; //The raw image containing the background grid
    public RawImage ringOuter; // The raw image containing the outer ring
    public RawImage ringContainer; // The raw image containing the ring container
    public Image ringCharge; //The image containing the charge effect of the rings
    public RawImage ringInner; // The raw image containing the inner ring
    public GameObject dividingLineBottom; // The game object that contains the bar at the bottom of the screen and its other parts
    public GameObject dividingLineTop; // The game object that contains the bar at the top of the screen and its other parts
    public GameObject textBorder; // The game object containing the text border
    public GameObject TriggerText; // The game object that contains the first piece of text we want to trigger
    public float bgFadeIn = .75f; // How long it takes for the background colour to fade in
    public float bgAlpha = 0.75f; // The opacity of the background, the value scales between 0 - 1, 0 being transparent, and 1 being opaque
    public float bgGridFadeIn = .5f; // How long it takes for the background grid colour to fade in
    public float bgGridAlpha = 0.25f; // The opacity of the background grid, the value scales between 0 - 1, 0 being transparent, and 1 being opaque    
    public float dlMovement = 1f; // How long it takes the dividing lines to move into place
    public float ringFadeIn = 0.25f; // How long it takes for the rings to fade in
    public float ringChargeFillTime = 1f; // How long it takes the ring charge progress bar to fill from 0 to 1
    public float ringeChargeOutTime = 0.5f; // How long it takes the ring charge progress bar to fill from 1 to 0
    public float textBorderFadeIn = .5f; // How long it takes the text border to scale to its full size
    public float delayTextTime = 1f; // How long the delay is before triggering the text event to start
    public float delayTextFinished = 10f; // How long the text takes to finish, this depends on how long the text events are set for and how many, do the maths yourself

    void Start()
    {
        // Sets the background alpha to 0, as it needs to fade in from 0 next
        Color bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;

        // Sets the background grid alpha to 0, as it needs to fade in from 0
        Color bgGridColor = backgroundGrid.color;
        bgGridColor.a = 0;
        backgroundGrid.color = bgGridColor;

        // Sets the background grid scale to 0 on the y axis
        backgroundGrid.transform.localScale = new Vector3(1, 0, 1);

        // Sets the all the rings alphas to 0, not the ring charge as that has a progress bar animation
        Color ringOuterColor = ringOuter.color;
        ringOuterColor.a = 0;
        ringOuter.color = ringOuterColor;
        Color ringContainerColor = ringContainer.color;
        ringContainerColor.a = 0;
        ringContainer.color = ringContainerColor;
        Color ringInnerColor = ringInner.color;
        ringInnerColor.a = 0;
        ringInner.color = ringInnerColor;

        // Sets all the ring scales to their starting scale value
        ringOuter.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // The outer ring starting scale
        ringContainer.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f); // The ring container starting scale
        ringInner.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); // The inner ring starting scale

        // Sets the ring charge progress bar value to 0
        ringCharge.fillAmount = 0;

        // Sets the diving lines position to offscreen
        dividingLineBottom.transform.localPosition = new Vector3(3840, -1080, 0); // This sets it to off screen left
        dividingLineTop.transform.localPosition = new Vector3(-3840, 1080, 0); // This sets it to off screen right

        // Sets the text border transform to 0 in the y axis
        textBorder.transform.localScale = new Vector3(1f, 0f, 1f);

        StartCoroutine(BackgroundAnimateIn());
        StartCoroutine(DividingLineTopAnimateIn());
        StartCoroutine(DividingLineBottomAnimateIn());
        StartCoroutine(TextBorderAnimateIn());
        StartCoroutine(DelayBackgroundGrid());
        StartCoroutine(DelayRings());
        StartCoroutine(DelayText());
    }

    IEnumerator BackgroundAnimateIn() // Fades the backgrounds alpha in with a lerp
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

    IEnumerator DividingLineTopAnimateIn() // Moves the top dividing line across the screen from left to right with a lerp
    {
        float newPosition = dividingLineTop.transform.localPosition.x;
        float startPosition = dividingLineTop.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 0f, elapsedTime / dlMovement);
            dividingLineTop.transform.localPosition = new Vector3(newPosition, 1080, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DividingLineBottomAnimateIn() // Moves the bottom dividing line across the screen from left to right with a lerp
    {
        float newPosition = dividingLineBottom.transform.localPosition.x;
        float startPosition = dividingLineBottom.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 0f, elapsedTime / dlMovement);
            dividingLineBottom.transform.localPosition = new Vector3(newPosition, -1080, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DelayBackgroundGrid() // Delays the start of the background grid for a given time
    {
        yield return new WaitForSeconds(.5f); // the 0.5f is a value to delay the grid, this value is chosen to sync the grid finishing at the same time as the background, if the background time changes, change this value
        StartCoroutine(BackgroundGridAnimateIn());
    }

    IEnumerator BackgroundGridAnimateIn() // Fades the background grid alpha in
    {
        Color localColour = backgroundGrid.color;
        float scaleY = backgroundGrid.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < bgGridFadeIn)
        {
            localColour.a = Mathf.Lerp(0f, bgGridAlpha, elapsedTime / bgGridFadeIn);
            backgroundGrid.color = localColour;
            scaleY = Mathf.Lerp(0, 1, elapsedTime / bgGridFadeIn);
            backgroundGrid.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DelayRings()
    {
        yield return new WaitForSeconds(1f); // The wait for seconds value is a delay on the rings starting their animation
        StartCoroutine(RingOuterAnimateIn());
        StartCoroutine(RingContainerAnimateIn());
        StartCoroutine(RingInnerAnimateIn());
        StartCoroutine(RingChargeProgressFill());
    }

    IEnumerator RingOuterAnimateIn()
    {
        Color localColour = ringOuter.color;
        Vector3 ringScale = ringOuter.transform.localScale;
        float scaleXYZ = ringOuter.transform.localScale.x;
        float scaleStart = ringOuter.transform.localScale.x;
        float rotationZ = ringOuter.transform.localRotation.z;
        Vector3 rotationXYZ = new Vector3(0f, 0f, 0f);
        float elapsedTime = 0;

        while (elapsedTime < ringFadeIn)
        {
            localColour.a = Mathf.Lerp(0f, 0.25f, elapsedTime / ringFadeIn); // fading the ring outer alpha, 0.25f is its max transparancy
            ringOuter.color = localColour;
            scaleXYZ = Mathf.Lerp(scaleStart, 1, elapsedTime / ringFadeIn);
            ringOuter.transform.localScale = new Vector3(scaleXYZ, scaleXYZ, scaleXYZ);
            rotationZ = Mathf.Lerp(0, 120, elapsedTime / ringFadeIn);
            rotationXYZ = new Vector3(0f, 0f, rotationZ);
            ringOuter.transform.rotation = quaternion.Euler(rotationXYZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RingContainerAnimateIn()
    {
        Color localColour = ringContainer.color;
        Vector3 ringScale = ringContainer.transform.localScale;
        float scaleXYZ = ringContainer.transform.localScale.x;
        float scaleStart = ringContainer.transform.localScale.x;
        float rotationZ = ringOuter.transform.localRotation.z;
        Vector3 rotationXYZ = new Vector3(0f, 0f, 0f);
        float elapsedTime = 0;

        while (elapsedTime < ringFadeIn)
        {
            localColour.a = Mathf.Lerp(0f, 1f, elapsedTime / ringFadeIn); // fading the ring container alpha, 1f is its max transparancy
            ringContainer.color = localColour;
            scaleXYZ = Mathf.Lerp(scaleStart, 1, elapsedTime / ringFadeIn);
            ringContainer.transform.localScale = new Vector3(scaleXYZ, scaleXYZ, scaleXYZ);
            rotationZ = Mathf.Lerp(0, -120, elapsedTime / ringFadeIn);
            rotationXYZ = new Vector3(0f, 0f, rotationZ);
            ringOuter.transform.rotation = quaternion.Euler(rotationXYZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RingInnerAnimateIn()
    {
        Color localColour = ringInner.color;
        Vector3 ringScale = ringInner.transform.localScale;
        float scaleXYZ = ringInner.transform.localScale.x;
        float scaleStart = ringInner.transform.localScale.x;
        float rotationZ = ringOuter.transform.localRotation.z;
        Vector3 rotationXYZ = new Vector3(0f, 0f, 0f);
        float elapsedTime = 0;

        while (elapsedTime < ringFadeIn)
        {
            localColour.a = Mathf.Lerp(0f, 1f, elapsedTime / ringFadeIn); // fading the ring outer alpha, 1f is its max transparancy
            ringInner.color = localColour;
            scaleXYZ = Mathf.Lerp(scaleStart, 1, elapsedTime / ringFadeIn);
            ringInner.transform.localScale = new Vector3(scaleXYZ, scaleXYZ, scaleXYZ);
            rotationZ = Mathf.Lerp(0, 60, elapsedTime / ringFadeIn);
            rotationXYZ = new Vector3(0f, 0f, rotationZ);
            ringOuter.transform.rotation = quaternion.Euler(rotationXYZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RingChargeProgressFill() // Sets the value of the ring charge progress bar from 0 - 1 with a lerp
    {
        float progressValue = ringCharge.fillAmount;
        float elapsedTime = 0;


        while (elapsedTime < ringChargeFillTime)
        {
            progressValue = Mathf.Lerp(0, 1, elapsedTime / ringChargeFillTime);
            ringCharge.fillAmount = progressValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator TextBorderAnimateIn()
    {
        float scaleY = textBorder.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < textBorderFadeIn)
        {
            scaleY = Mathf.Lerp(0, 1, elapsedTime / textBorderFadeIn);
            textBorder.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DelayText()
    {
        yield return new WaitForSeconds(delayTextTime); // The wait for seconds value is a delay on the text being triggered to start
        StartCoroutine(TriggerTextEvent());
        StartCoroutine(TriggerTextEventFinished());
    }

    IEnumerator TriggerTextEvent() // Triggers the text object to start animating
    {
        if (TriggerText != null)
        {
            TriggerText.SetActive(true);
            TriggerText.GetComponent<Script_UI_TMP_FadeInAndOut>().TriggerSelf = true;
            yield return null;
        }
    }

    IEnumerator TriggerTextEventFinished() //Delay to start the screen animating out when the text has finished
    {
        yield return new WaitForSeconds(delayTextFinished);
        StartCoroutine(AnimateOut());
    }

    IEnumerator AnimateOut()
    {
        StartCoroutine(TextBorderAnimateOut());
        StartCoroutine(RingOuterAnimateOut());
        StartCoroutine(RingContainerAnimateOut());
        StartCoroutine(RingInnerAnimateOut());
        StartCoroutine(RingChargeProgressFillOut());
        StartCoroutine(DividingLineTopAnimateOut());
        StartCoroutine(DividingLineBottomAnimateOut());
        StartCoroutine(BackgroundGridAnimateOut());
        StartCoroutine(DelayBackgroundOut());
        yield return null;
    }

    IEnumerator TextBorderAnimateOut() // Animating the text border out, scaling down to 0 in the Y axis
    {
        float scaleY = textBorder.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < textBorderFadeIn)
        {
            scaleY = Mathf.Lerp(1, 0, elapsedTime / textBorderFadeIn);
            textBorder.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RingOuterAnimateOut() // Animating the outer ring out
    {
        Color localColour = ringOuter.color;
        Vector3 ringScale = ringOuter.transform.localScale;
        float scaleXYZ = ringOuter.transform.localScale.x;
        float scaleStart = ringOuter.transform.localScale.x;
        float rotationZ = ringOuter.transform.localRotation.z;
        Vector3 rotationXYZ = new Vector3(0f, 0f, 0f);
        float elapsedTime = 0;

        while (elapsedTime < ringFadeIn)
        {
            localColour.a = Mathf.Lerp(0.25f, 0f, elapsedTime / ringFadeIn); // fading the ring outer alpha, 0.25f is its max transparancy
            ringOuter.color = localColour;
            scaleXYZ = Mathf.Lerp(scaleStart, 1.5f, elapsedTime / ringFadeIn);
            ringOuter.transform.localScale = new Vector3(scaleXYZ, scaleXYZ, scaleXYZ);
            rotationZ = Mathf.Lerp(0, 60, elapsedTime / ringFadeIn);
            rotationXYZ = new Vector3(0f, 0f, rotationZ);
            ringOuter.transform.rotation = quaternion.Euler(rotationXYZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RingContainerAnimateOut() // Animating the ring container out
    {
        Color localColour = ringContainer.color;
        Vector3 ringScale = ringContainer.transform.localScale;
        float scaleXYZ = ringContainer.transform.localScale.x;
        float scaleStart = ringContainer.transform.localScale.x;
        float rotationZ = ringContainer.transform.localRotation.z;
        Vector3 rotationXYZ = new Vector3(0f, 0f, 0f);
        float elapsedTime = 0;

        while (elapsedTime < ringFadeIn)
        {
            localColour.a = Mathf.Lerp(1f, 0f, elapsedTime / ringFadeIn); // fading the ring outer alpha, 0.25f is its max transparancy
            ringContainer.color = localColour;
            scaleXYZ = Mathf.Lerp(scaleStart, .5f, elapsedTime / ringFadeIn);
            ringContainer.transform.localScale = new Vector3(scaleXYZ, scaleXYZ, scaleXYZ);
            rotationZ = Mathf.Lerp(0, -60, elapsedTime / ringFadeIn);
            rotationXYZ = new Vector3(0f, 0f, rotationZ);
            ringContainer.transform.rotation = quaternion.Euler(rotationXYZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RingInnerAnimateOut() //// Animating the inner ring out
    {
        Color localColour = ringInner.color;
        Vector3 ringScale = ringInner.transform.localScale;
        float scaleXYZ = ringInner.transform.localScale.x;
        float scaleStart = ringInner.transform.localScale.x;
        float rotationZ = ringOuter.transform.localRotation.z;
        Vector3 rotationXYZ = new Vector3(0f, 0f, 0f);
        float elapsedTime = 0;

        while (elapsedTime < ringFadeIn)
        {
            localColour.a = Mathf.Lerp(1f, 0f, elapsedTime / ringFadeIn); // fading the ring outer alpha, 1f is its max transparancy
            ringInner.color = localColour;
            scaleXYZ = Mathf.Lerp(scaleStart, .5f, elapsedTime / ringFadeIn);
            ringInner.transform.localScale = new Vector3(scaleXYZ, scaleXYZ, scaleXYZ);
            rotationZ = Mathf.Lerp(0, 30, elapsedTime / ringFadeIn);
            rotationXYZ = new Vector3(0f, 0f, rotationZ);
            ringOuter.transform.rotation = quaternion.Euler(rotationXYZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RingChargeProgressFillOut() // Sets the value of the ring charge progress bar from 1 - 0 with a lerp
    {
        float progressValue = ringCharge.fillAmount;
        float elapsedTime = 0;

        while (elapsedTime < ringeChargeOutTime)
        {
            progressValue = Mathf.Lerp(1, 0, elapsedTime / ringeChargeOutTime);
            ringCharge.fillAmount = progressValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator DividingLineTopAnimateOut()
    {
        float newPosition = dividingLineTop.transform.localPosition.x;
        float startPosition = dividingLineTop.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, -3860f, elapsedTime / dlMovement);
            dividingLineTop.transform.localPosition = new Vector3(newPosition, 1080, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DividingLineBottomAnimateOut()
    {
        float newPosition = dividingLineBottom.transform.localPosition.x;
        float startPosition = dividingLineBottom.transform.localPosition.x;
        float elapsedTime = 0;

        while (elapsedTime < dlMovement)
        {
            newPosition = Mathf.Lerp(startPosition, 3860f, elapsedTime / dlMovement);
            dividingLineBottom.transform.localPosition = new Vector3(newPosition, -1080, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    IEnumerator BackgroundGridAnimateOut()
    {
        Color localColour = backgroundGrid.color;
        float scaleY = backgroundGrid.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < bgGridFadeIn)
        {
            localColour.a = Mathf.Lerp(bgGridAlpha, 0f, elapsedTime / bgGridFadeIn);
            backgroundGrid.color = localColour;
            scaleY = Mathf.Lerp(1f, 0f, elapsedTime / bgGridFadeIn);
            backgroundGrid.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DelayBackgroundOut()
    {
        yield return new WaitForSeconds(.75f);
        StartCoroutine(BackgroundAnimateOut());
    }

    IEnumerator BackgroundAnimateOut()
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

}