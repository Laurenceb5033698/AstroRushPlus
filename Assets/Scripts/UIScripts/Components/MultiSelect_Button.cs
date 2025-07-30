using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways]
public class MultiSelect_Button : Button
{
    
    //[SerializeField] public List<SelectionVisuals> m_ChildSelectables;

    //protected override void Awake()
    //{
    //    m_ChildSelectables = new List<SelectionVisuals>();
    //    base.Awake();
    //}

    protected override void DoStateTransition(SelectionState _state, bool _instant)
    {
        base.DoStateTransition(_state, _instant);
        foreach (SelectionVisuals selectable in this.GetComponentsInChildren<SelectionVisuals>())
        {
            //setting visual state of a SelectionVisuals causes colour transition.
            selectable.VisualState = (SelectionVisuals.SelectionState)_state;

            //Color temp = selectable.colors.normalColor;
            //switch (state) { 
            //    case SelectionState.Normal: temp = selectable.colors.normalColor; break;
            //    case SelectionState.Highlighted: temp = selectable.colors.highlightedColor; break;
            //    case SelectionState.Pressed: temp = selectable.colors.pressedColor; break;
            //    case SelectionState.Selected: temp = selectable.colors.selectedColor; break;
            //    case SelectionState.Disabled: temp = selectable.colors.disabledColor; break;
            //}

            //(selectable.image as Graphic).CrossFadeColor(temp, selectable.colors.fadeDuration, true, true);
        }
    }

}
