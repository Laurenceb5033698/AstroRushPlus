using UnityEngine;
using UnityEngine.EventSystems;

//Credit to @CristinaCreatesGames on yt: https://www.youtube.com/watch?v=sQpjgcX-jH8

public class RememberLastSelectedUIObject : MonoBehaviour
{
    RememberLastSelectedUIObject instance;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject lastSelectedElement;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(this);
    }

    private void Reset()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();

        if (!eventSystem)
        {
            Debug.Log("Did not find an Event System in this scene.", this);
            return;
        }
        lastSelectedElement = eventSystem.firstSelectedGameObject;
    }

    private void Update()
    {
        if (!eventSystem)
            return;

        //if current selected changes, then save it to last selected.
        if(eventSystem.currentSelectedGameObject &&
            lastSelectedElement != eventSystem.currentSelectedGameObject)
        {
            lastSelectedElement = eventSystem.currentSelectedGameObject;
        }

        //auto select last object if current gets unset.
        if(!eventSystem.currentSelectedGameObject && lastSelectedElement)
        {
            eventSystem.SetSelectedGameObject(lastSelectedElement);
            //propagate new selected object to uimanager and screens
            UIManager.instance.passNewSelectedObject(lastSelectedElement);
        }
    }
}
