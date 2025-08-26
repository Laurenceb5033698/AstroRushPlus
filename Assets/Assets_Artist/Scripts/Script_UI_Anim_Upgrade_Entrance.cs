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
        yield return null;
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
        yield return null;
    }

    IEnumerator DelayCardCentre()
    {
        yield return new WaitForSeconds(.25f);
        StartCoroutine(UpgradeCentreOn());

        yield return new WaitForSeconds(.35f);
        StartCoroutine(UpgradeLeftOn());

        yield return new WaitForSeconds(.45f);
        StartCoroutine(UpgradeRightOn());
    }

    IEnumerator UpgradeCentreOn()
    {
        UpgradeCentre.SetActive(true);
        yield return null;
    }

    IEnumerator UpgradeLeftOn()
    {
        UpgradeLeft.SetActive(true);
        yield return null;
    }

    IEnumerator UpgradeRightOn()
    {
        UpgradeRight.SetActive(true);
        yield return null;
    }
}
