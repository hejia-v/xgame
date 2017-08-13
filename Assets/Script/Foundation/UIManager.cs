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

        TouchManager.addListener(mJoystick, true, TouchPriority.joystick);
    }

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
