using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class ScreenElement : MonoBehaviour {

    private Canvas cv = null;
    [SerializeField] protected List<Button> ButtonList;
    private int selector = 0;

    virtual public void Awake()
    {
        cv = GetComponent<Canvas>();
    }

    virtual public void OnEnable()
    {
        cv.enabled = true;
    }
    virtual public void OnDisable()
    {
        cv.enabled = false;
    }

    public void OnScreenOpen()
    {   //called from UIManager when screenElement is swapped to.
        selector = 0;
        SelectButton();
    }
    public void AdvanceSelector()
    {
        if (selector == (ButtonList.Count - 1))
            selector = 0;
        else
            ++selector;
        SelectButton();
    }

    public void RetreatSelector()
    {
        if (selector == 0)
            selector = (ButtonList.Count - 1);
        else
            --selector;
        SelectButton();
    }
    public void SubmitSelection()
    {
        ButtonList[selector].onClick.Invoke();
    }

    private void SelectButton()
    {
        if (ButtonList.Count > 0)
            ButtonList[selector].Select();
    }
}
