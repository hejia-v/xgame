using UnityEngine;
using UnityEngine.EventSystems;


public class UnityEventDispatch : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
    void Update()
    {
        InputManager.Update();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TouchManager.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        TouchManager.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TouchManager.OnEndDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TouchManager.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TouchManager.OnPointerUp(eventData);
    }
}
