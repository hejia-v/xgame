using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image mBgImg;
    private Image mJoystickImg;
    private Vector3 mInputVector;

    void Start()
    {
        Text msgBox = transform.Find("MessageBox").GetComponent<Text>();
        Image bgImg = transform.Find("JoystickBg").GetComponent<Image>();
        Image joystickImg = bgImg.transform.Find("joystick").GetComponent<Image>();

        mBgImg = bgImg;
        mJoystickImg = joystickImg;

        //bgImg.gameObject.SetActive(false);

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mBgImg.rectTransform,
            eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = pos.x / mBgImg.rectTransform.sizeDelta.x;
            pos.y = pos.y / mBgImg.rectTransform.sizeDelta.y;

            //Debug.Log("Hey");
            mInputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            mInputVector = (mInputVector.magnitude > 1.0f) ? mInputVector.normalized : mInputVector;

            mJoystickImg.rectTransform.anchoredPosition =
                new Vector3(mInputVector.x * (mBgImg.rectTransform.sizeDelta.x / 3)
                , mInputVector.z * (mBgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mInputVector = Vector3.zero;
        mJoystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (mInputVector.x != 0)
        {
            return mInputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (mInputVector.z != 0)
        {
            return mInputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
