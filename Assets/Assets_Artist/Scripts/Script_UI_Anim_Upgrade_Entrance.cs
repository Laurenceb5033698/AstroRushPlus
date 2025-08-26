using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Script_UI_Anim_Upgrade_Entrance : MonoBehaviour
{
    public Image background; //The image with the background colour
    public GameObject dividingLineLeft; // The game object that contains the bar on the left of the screen and its other parts
    public GameObject dividingLineRight; // The game object that contains the bar on the right of the screen and its other parts
    public GameObject header; // The game object that contains the header and all its other parts
    public GameObject UpgradeCentre; // The central upgrade gameobject
    public GameObject UpgradeLeft; // The central upgrade gameobject 
    public GameObject UpgradeRight; // The central upgrade gameobject 
    public float bgFadeIn = 0.5f; // How long it takes for the background colour to fade in
    public float bgAlpha = 0.5f; // The opacity of the background, the value scales between 0 - 1, 0 being transparent, and 1 being opaque
    public float dlMovement = 0.5f; // How long it takes the dividing lines to move into place
    public float cardFadeTime = 0.25f; // How long it takes for the upgrade cards to transition out
    public bool upgradeChosen = false; //If the upgrade has been selected, triggers the exit animations

    void Start()
    {
        // Sets the background alpha to 0, as it needs to fade in from 0 next
        Color bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;

        // Sets the position of the dividing lines, and header to offscreen
        dividingLineLeft.transform.localPosition = new Vector3(-1920, 2160, 0); // This sets it to off screen left
        dividingLineRight.transform.localPosition = new Vector3(1920, -2160, 0); // This sets it to off screen right
        header.transform.localPosition = new Vector3(0, 1320, 0); // This sets it to off screen north

        // Sets the upgrade cards active to off
        UpgradeCentre.SetActive(false);
        UpgradeLeft.SetActive(false);
        UpgradeRight.SetActive(false);


        // Starts off the animation 
        StartCoroutine(BackgroundAnimateIn());
        StartCoroutine(DividingLineLeftAnimateIn());
        StartCoroutine(DividingLineRightAnimateIn());
        StartCoroutine(HeaderAnimateIn());
        StartCoroutine(DelayCardCentre());
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

    IEnumerator DelayCardCentre() // Wait to turn on the upgrade cards, then turn them on in sequence
    {
        yield return new WaitForSeconds(.25f);
        StartCoroutine(UpgradeCentreOn());

        yield return new WaitForSeconds(.35f);
        StartCoroutine(UpgradeLeftOn());

        yield return new WaitForSeconds(.45f);
        StartCoroutine(UpgradeRightOn());
    }

    IEnumerator UpgradeCentreOn() // Turn on on the centre card
    {
        UpgradeCentre.SetActive(true);
        yield return null;
    }

    IEnumerator UpgradeLeftOn() // Turn on on the centre card
    {
        UpgradeLeft.SetActive(true);
        yield return null;
    }

    IEnumerator UpgradeRightOn() // Turn on on the centre card
    {
        UpgradeRight.SetActive(true);
        yield return null;
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
        StartCoroutine(UpgradeCentreAnimateOff());
        StartCoroutine(UpgradeLeftAnimateOff());
        StartCoroutine(UpgradeRightAnimateOff());
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

    IEnumerator UpgradeCentreAnimateOff() // Animates the central upgrade card to shrink on its Y-axis
    {
        float scaleY = UpgradeCentre.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < cardFadeTime)
        {
            scaleY = Mathf.Lerp(1, 0, elapsedTime / cardFadeTime);
            UpgradeCentre.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UpgradeCentre.SetActive(false);

    }

    IEnumerator UpgradeLeftAnimateOff() // Animates the central upgrade card to shrink on its Y-axis
    {
        float scaleY = UpgradeLeft.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < cardFadeTime)
        {
            scaleY = Mathf.Lerp(1, 0, elapsedTime / cardFadeTime);
            UpgradeLeft.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UpgradeLeft.SetActive(false);

    }

        IEnumerator UpgradeRightAnimateOff() // Animates the central upgrade card to shrink on its Y-axis
    {
        float scaleY = UpgradeRight.transform.localScale.y;
        float elapsedTime = 0;

        while (elapsedTime < cardFadeTime)
        {
            scaleY = Mathf.Lerp(1, 0, elapsedTime / cardFadeTime);
            UpgradeRight.transform.localScale = new Vector3(1, scaleY, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UpgradeRight.SetActive(false);

    }

}


