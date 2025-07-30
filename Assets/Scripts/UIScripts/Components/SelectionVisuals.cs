using UnityEngine;
using UnityEngine.UI;


//component to replace the need for selection components on child objects of button in heirarchy
// use on a component that is a child of a button and needs colour transition states
public class SelectionVisuals : MonoBehaviour
{

    //mirror of Selectable.SelectionState;
    public enum SelectionState
    {
        Normal,
        Highlighted,
        Pressed,
        Selected,
        Disabled,
    }
    [SerializeField]
    private Graphic m_TargetGraphic;
    private SelectionState m_CurrentSelectionState = SelectionState.Normal;
    public ColorBlock m_Colors = ColorBlock.defaultColorBlock;

    public SelectionState VisualState 
    { 
        get { return m_CurrentSelectionState; } 
        set 
        {
            m_CurrentSelectionState = value; 
            OnSetProperty(); 
        } 
    }

    private void OnEnable()
    {
        DoStateTransition(m_CurrentSelectionState, true);
    }
    private void OnDisable()
    {
        InstantClearState();
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        m_Colors.fadeDuration = Mathf.Max(m_Colors.fadeDuration, 0.0f);
        if (isActiveAndEnabled)
        {
            //if transition is implemented, need to set all transitions to defualt, then dotransition to current;
            StartColorTween(Color.white, true);

            DoStateTransition(m_CurrentSelectionState, true);
        }
    }
    private void Reset()
    {
        m_TargetGraphic= GetComponent<Graphic>();
    }
#endif  // if UNITY_EDITOR

    private void OnSetProperty()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            DoStateTransition(m_CurrentSelectionState, true);
        else
#endif  // if UNITY_EDITOR
            DoStateTransition(m_CurrentSelectionState, false);
    }

    public void DoStateTransition(SelectionState _state, bool _instant)
    {
        if ((!gameObject.activeInHierarchy))
            return;

        Color tintColour;
        switch (_state)
        {
            case SelectionState.Normal:
                tintColour = m_Colors.normalColor;
                break;
            case SelectionState.Highlighted:
                tintColour = m_Colors.highlightedColor;
                break;
            case SelectionState.Pressed:
                tintColour = m_Colors.pressedColor;
                break;
            case SelectionState.Selected:
                    tintColour = m_Colors.selectedColor;
                break;
            case SelectionState.Disabled:
                tintColour = m_Colors.disabledColor;
                break;
            default:
                tintColour = Color.black;
                break;
        }

        //only colourtint atm
        StartColorTween(tintColour, _instant);

    }

    private void StartColorTween(Color _targetColour, bool _instant)
    {
        if (m_TargetGraphic == null)
            return;
        m_TargetGraphic.CrossFadeColor(_targetColour, _instant ? 0f : m_Colors.fadeDuration, true, true);

    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            InstantClearState();
        }
    }
    private void InstantClearState()
    {
        StartColorTween(Color.white, true);
    }
}