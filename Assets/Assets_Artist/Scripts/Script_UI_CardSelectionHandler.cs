using System.Collections;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.EventSystems;

/// <summary>
/// Handles actual mechanics of what happens when a card is hovered or selected by the player.
/// </summary>
public class Script_UI_CardSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    //[SerializeField] private float _verticalMoveAmount = 30f;
    [SerializeField] private float m_moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float m_scaleAmount = 1.1f;


    [SerializeField] private Vector3 m_startPosition;
    [SerializeField] private Vector3 m_startScale;


    private void Start()
    {
        //_startPosition = transform.position;
        m_startScale = transform.localScale;
    }
    private void OnEnable()
    {
        m_startPosition = transform.position;
    }

    private IEnumerator MoveCard(bool _startingAnimation)
    {
        //Vector3 endPosition;
        Vector3 endScale;
        //Debug.Log("Starting move card");
        float elapsedTime = 0f;
        while (elapsedTime < m_moveTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            //Debug.Log("move card loop");
            if (_startingAnimation)
            {
                //endPosition = _startPosition + new Vector3(0f, _verticalMoveAmount, 0f);
                endScale = m_startScale * m_scaleAmount;
            }
            else
            {
                //endPosition = _startPosition;
                endScale = m_startScale;
            }

            //Calculate the lerped amounts
            //Vector3 lerpedPosition = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / m_moveTime));

            //Actually apply the changes to the position and scale
            //transform.position = lerpedPosition;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       //Select the card
       eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Deselect the card
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("Card selected: " + gameObject.name);
        StartCoroutine(MoveCard(true));
        //UIManager.instance.GetCurrentElement().SetLastSelected(gameObject);

        //Script_UI_CardSelectionManager.instance.SetLastSelected(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(false));
    }

}
