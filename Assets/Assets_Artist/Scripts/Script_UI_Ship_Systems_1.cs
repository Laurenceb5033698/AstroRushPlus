
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Script_UI_Ship_Systems_1 : MonoBehaviour
{
    public Image background; //The image with the background colour
    public RawImage backgroundGrid; //The raw image containing the background grid
    public GameObject dividingLineBottom; //The game object that contains the bar at the bottom of the screen and its other parts
    public GameObject dividingLineTop; //The game object that contains the bar at the top of the screen and its other parts
    

    public float bgFadeIn = .75f; // how long it takes for the background colour to fade in
    public float bgAlpha = 0.75f; // the opacity of the background, the value scales between 0 - 1, 0 being transparent, and 1 being opaque
    public float bgGridFadeIn = .5f; // how long it takes for the background grid colour to fade in
    public float bgGridAlpha = 0.25f; // the opacity of the background grid, the value scales between 0 - 1, 0 being transparent, and 1 being opaque    
    public float dlMovement = 1f; // how long it takes the dividing lines to move into place

    void Start()
    {
        //Sets the background alpha to 0, as it needs to fade in from 0 next
        Color bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;

        //Sets the background grid alpha to 0, as it needs to fade in from 0
        Color bgGridColor = backgroundGrid.color;
        bgGridColor.a = 0;
        backgroundGrid.color = bgGridColor;

        //Sets the background grid scale to 0 on the y axis
        backgroundGrid.transform.localScale = new Vector3(1, 0, 1);

        //Sets the diving lines position to offscreen
        dividingLineBottom.transform.localPosition = new Vector3(3840, -1080, 0); //This sets it to off screen left
        dividingLineTop.transform.localPosition = new Vector3(-3840, 1080, 0); //this sets it to off screen right

        StartCoroutine(BackgroundAnimateIn());
        StartCoroutine(DividingLineTopAnimateIn());
        StartCoroutine(DividingLineBottomAnimateIn());
        StartCoroutine(DelayBackgroundGrid());
        
    }


    IEnumerator BackgroundAnimateIn() //Fades the backgrounds alpha in with a lerp
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

    IEnumerator DividingLineTopAnimateIn() //Moves the top dividing line across the screen from left to right with a lerp
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

    IEnumerator DividingLineBottomAnimateIn() //Moves the bottom dividing line across the screen from left to right with a lerp
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


    IEnumerator DelayBackgroundGrid() //Delays the start of the background grid for a given time
    {
        yield return new WaitForSeconds(.5f); //the 0.5f is a value to delay the grid, this value is chosen to sync the grid finishing at the same time as the background, if the background time changes, change this value
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





    // Update is called once per frame
    void Update()
    {
        
    }
}
