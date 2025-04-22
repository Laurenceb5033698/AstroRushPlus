using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
        StartCoroutine(MoveCard(true));

        Script_UI_CardSelectionManager.instance.LastSelected = gameObject;

        // Find the index
        for (int i = 0; i < Script_UI_CardSelectionManager.instance.Cards.Length; i++)
        {
            if (Script_UI_CardSelectionManager.instance.Cards[i] == gameObject)
            {
                Script_UI_CardSelectionManager.instance.LastSelectedIndex = i;
                return;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(false));
    }

}
