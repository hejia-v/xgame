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

        UEventManager.register("OnPointerDown", mJoystick.OnPointerDown, mJoystick);
        UEventManager.register("OnDrag", mJoystick.OnDrag, mJoystick);
        UEventManager.register("OnPointerUp", mJoystick.OnPointerUp, mJoystick);
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
