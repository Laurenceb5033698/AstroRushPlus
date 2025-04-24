using System.Collections;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

/// <summary>
/// Represents a UI menu / view that can be enabled/disabled form UIManager.
/// handles user inputs and how that changes the UI.
/// is derived from for specific screens.
/// </summary>
abstract public class ScreenElement : MonoBehaviour {

    private Canvas cv = null;
    [SerializeField] protected GameObject[] SelectableList;
    GameObject LastSelected;
    protected int selector { get; set; }

    protected Inputs controls;
    protected Script_UI_InputManager uiInputManager;


    //when a screen object is enabled/disabled, also enables/disables the canvas for this screen.
    protected void Awake()
    {
        cv = GetComponent<Canvas>();
        selector = 0;
    }

    protected void OnEnable()
    {
        cv.enabled = true;
        StartCoroutine(SetSelectedAfterOneFrame());
    }
    protected void OnDisable()
    {
        cv.enabled = false;
    }

    //wait one frame before selecting first object.
    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        if(SelectableList.Length > 0)
            EventSystem.current.SetSelectedGameObject(SelectableList[0]);
    }

    /// <summary>
    /// by default Checks submit and back/cancel inputs.
    /// each derived screen has navigation setup on the buttons themselves.
    /// </summary>
    virtual public void Update()
    {
        HandleSubmit();
        HandleCancel();        
    }

    //##########################
    //Input to selector Handling
    protected void HandleSubmit()
    {
        if (Script_UI_InputManager.instance.SubmitInput)
        {
            SubmitSelection();
        }
    }
    protected void HandleCancel()
    {
        if (Script_UI_InputManager.instance.CancelInput)
        {
            Cancel();
        }
    }
    protected void HandleNavigateUp()
    {
        if (Script_UI_InputManager.instance.NavigationInput.y > 0)
        {   //move selector up
            HandleNextSelection(1);
        }
        
    }
    protected void HandleNavigateDown()
    {
        if (Script_UI_InputManager.instance.NavigationInput.y < 0)
        {   //move selector down
            HandleNextSelection(-1);
        }
    }
    //!!! INCOMPLETE
    //Left and right use same selector as up/down, but with horizontal inputs.
    //later will need proper 2d selection handling
    protected void HandleNavigateLeft()
    {
        if (Script_UI_InputManager.instance.NavigationInput.x < 0)
        {   //move selector right
            HandleNextSelection(-1);
        }
    }
    protected void HandleNavigateRight()
    {
        if (Script_UI_InputManager.instance.NavigationInput.x < 0)
        {   //move selector right
            HandleNextSelection(1);
        }
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
    ///<summary>
    /// passed from rememberLastSelectedUIObject.
    /// a new object selected, so update screen indexs
    /// </summary>
    /// <param name="_newSelected"></param>
    public void SetNewSelectedObject(GameObject _newSelected)
    {
        SetLastSelected(_newSelected);
    }

    public void SetLastSelected(GameObject _cardObject)
    {
        LastSelected = gameObject;

        // Find the index
        for (int i = 0; i < SelectableList.Length; i++)
        {
            if (SelectableList[i] == gameObject)
            {
                selector = i;
                return;
            }
        }
    }

    ///////////////////////
    //UI Button interaction
    virtual public void AdvanceSelector()
    {
        if (selector == (SelectableList.Length - 1))
            selector = 0;
        else
            ++selector;
        SelectButton();
    }

    virtual public void RetreatSelector()
    {
        if (selector == 0)
            selector = (SelectableList.Length - 1);
        else
            --selector;
        SelectButton();
    }

    /// <summary>
    /// moves selector forward or backward by 'addition' amount.
    /// sets new object selected using event system.
    /// </summary>
    protected void HandleNextSelection(int addition)
    {
        if (EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = selector + addition;
            newIndex = Mathf.Clamp(newIndex, 0, SelectableList.Length - 1);
            EventSystem.current.SetSelectedGameObject(SelectableList[newIndex]);
        }
    }

    /// <summary>
    /// invokes onclick event for buttons, toggles toggles, could do other things if needed
    /// </summary>
    public void SubmitSelection()
    {
        Selectable selectedObject = SelectableList[selector].GetComponent<Selectable>();
        if (selectedObject)
        {
            if(selectedObject is Button)
            {
                ((Button)selectedObject).onClick.Invoke();
            }
            if (selectedObject is Toggle)
            {
                ((Toggle)selectedObject).isOn = !((Toggle)selectedObject).isOn;
            }
            //sliders are handled using nav controls.
        }
    }

    virtual protected void Cancel()
    {
        //goes back a screen, handled by derived?
    }



    virtual protected void SelectButton()
    {
        if (SelectableList.Length > 0)
            EventSystem.current.SetSelectedGameObject(SelectableList[selector]);

    }
}
