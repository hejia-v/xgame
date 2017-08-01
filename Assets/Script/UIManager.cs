using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Joystick mJoystick;

    void Start()
    {
        //Text msgBox = transform.Find("MessageBox").GetComponent<Text>();
        Image bgImg = transform.Find("JoystickBg").GetComponent<Image>();
        mJoystick = new Joystick(bgImg);

        UEventManager.register(UEventManager.EventType.OnPointerDown, new UEventManager.BoolDelegate(mJoystick.OnPointerDown), mJoystick, true, UEventPriority.joystick);
        UEventManager.register(UEventManager.EventType.OnDrag, new UEventManager.VoidDelegate(mJoystick.OnDrag), mJoystick, true, UEventPriority.joystick);
        UEventManager.register(UEventManager.EventType.OnPointerUp, new UEventManager.VoidDelegate(mJoystick.OnPointerUp), mJoystick, true, UEventPriority.joystick);
    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        UEventManager.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UEventManager.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UEventManager.OnEndDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UEventManager.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UEventManager.OnPointerUp(eventData);
    }
}
