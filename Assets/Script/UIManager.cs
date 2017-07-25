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
    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        mJoystick.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mJoystick.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mJoystick.OnPointerUp(eventData);

    }

}
