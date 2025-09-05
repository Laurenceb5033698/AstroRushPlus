using UnityEngine;
using DG.Tweening;
using System.Collections;
public class Script_UI_Component_DLine_1 : MonoBehaviour
{
    public GameObject dividingLineTop; // The game object that contains the bar at the top of the screen and its other parts
    public GameObject dividingLineBottom; // The game object that contains the bar at the bottom of the screen and its other parts
    [SerializeField] private Vector3 dlTopStartPos = new Vector3();
    [SerializeField] private Vector3 dlBottomStartPos = new Vector3();
    public float MoveInTime = 0; // How long it takes the object to move into position
    public float MoveOutTime = 0; // How long it takes the object to move into its exit position


    public

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Sets the diving lines position to offscreen
        dividingLineTop.transform.localPosition = dlTopStartPos; //Sets the top dividing line to its starting position offscreen
        dividingLineBottom.transform.localPosition = dlBottomStartPos; //Sets the bottom dividing line to its starting position offscreen

        // Starts the animation coroutines
        StartCoroutine(DLineTopAnimateIn());
        StartCoroutine(DLineBottomAnimateIn());
        StartCoroutine(DelayExit());

    }

    IEnumerator DLineTopAnimateIn()
    {
        dividingLineTop.transform.DOMoveX(0, MoveInTime);
        yield return null;
    }

    IEnumerator DLineBottomAnimateIn()
    {
        dividingLineBottom.transform.DOMoveX(0, MoveInTime);
        yield return null;
    }

    IEnumerator DelayExit()
    {
        yield return new WaitForSeconds(3f);
        {
            StartCoroutine(DLineTopAnimateOut());
            StartCoroutine(DLineBottomAnimateOut());
        }
    }
    
    IEnumerator DLineTopAnimateOut()
    {
        dividingLineTop.transform.DOMoveX(210, MoveOutTime);
        yield return null;
    }

    IEnumerator DLineBottomAnimateOut()
    {
        dividingLineBottom.transform.DOMoveX(-210, MoveOutTime);
        yield return null;
    }
    
}
