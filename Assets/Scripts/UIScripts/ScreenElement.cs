using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class ScreenElement : MonoBehaviour {

    private Canvas cv = null;
    [SerializeField] protected List<Selectable> ButtonList;
    protected int selector = 0;
    protected Inputs controls;


    protected void Awake()
    {
        cv = GetComponent<Canvas>();
    }

    protected void OnEnable()
    {
        cv.enabled = true;
    }
    protected void OnDisable()
    {
        cv.enabled = false;
    }

    public void OnScreenOpen()
    {   
        OnScreenOpenInternal();
    }

    protected virtual void OnScreenOpenInternal()
    {
        //called from UIManager when screenElement is swapped to.
        selector = 0;
        SelectButton();
        if (GameManager.instance != null)//if there is a game active
            controls = GameManager.instance.GlobalInputs;
        else
            if (MainMenu.instance != null)
        {//if there is a main menu active
            controls = MainMenu.instance.GlobalInputs;
        }
    }


    ///////////////////////
    //UI Button interaction
    virtual public void AdvanceSelector()
    {
        if (selector == (ButtonList.Count - 1))
            selector = 0;
        else
            ++selector;
        SelectButton();
    }

    virtual public void RetreatSelector()
    {
        if (selector == 0)
            selector = (ButtonList.Count - 1);
        else
            --selector;
        SelectButton();
    }
    virtual public void SubmitSelection()
    {
        ((Button)ButtonList[selector]).onClick.Invoke();
    }

    virtual protected void SelectButton()
    {
        if (ButtonList.Count > 0)
            ButtonList[selector].Select();
    }
}
