using System.Collections;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.EventSystems;

/// <summary>
/// Handles actual mechanics of what happens when a card is hovered or selected by the player.
/// </summary>
public class Script_UI_CardSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _verticalMoveAmount = 30f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;


    private Vector3 _startPosition;
    private Vector3 _startScale;


    private void Start()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
    }

    private IEnumerator MoveCard(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;

            if (startingAnimation)
            {
                endPosition = _startPosition + new Vector3(0f, _verticalMoveAmount, 0f);
                endScale = _startScale * _scaleAmount;
            }

            else
            {
                endPosition = _startPosition;
                endScale = _startScale;
            }

            //Calculate the lerped amounts
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime));

            //Actually apply the changes to the position and scale
            transform.position = lerpedPosition;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       //Select the card
       eventData.selectedObject = gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Deselect the card
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(true));
        UIManager.instance.GetCurrentElement().SetLastSelected(gameObject);

        //Script_UI_CardSelectionManager.instance.SetLastSelected(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(false));
    }

}
