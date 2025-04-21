using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_UI_CardSelectionManager : MonoBehaviour
{
    public static Script_UI_CardSelectionManager instance;

    public GameObject[] Cards;

    public GameObject LastSelected { get; set; }

    public int LastSelectedIndex { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void OnEnable()
    {
        StartCoroutine(SetSelectedAfterOneFrame());
    }

    void Update()
    {
        if (Script_UI_InputManager.instance.NavigationInput.x > 0)
        {
            //Select the next card
            HandleNextCardSelection(1);
        }

        if (Script_UI_InputManager.instance.NavigationInput.x < 0)
        {
            //Select the previous card
            HandleNextCardSelection(-1);
        }
    }

    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(Cards[0]);
    }

    private void HandleNextCardSelection(int addition)
    {
        if (EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, Cards.Length - 1);
            EventSystem.current.SetSelectedGameObject(Cards[newIndex]);
        }
    }
}
