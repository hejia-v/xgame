using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick
{
    private Image mBgImg;
    private Image mJoystickImg;
    private Image mThumbImg;
    private Vector3 mInputVector;
    private Vector2 mInitPos;
    private float mBgRadius;
    private float mJoystickRadius;
    private float mMoveRadius;
    private Tweener mTweener;
    private bool mIsDraging;

    public Joystick(Image bgImg)
    {
        mBgImg = bgImg;
        mJoystickImg = mBgImg.transform.Find("joystick").GetComponent<Image>();
        mThumbImg = mBgImg.transform.Find("thumb").GetComponent<Image>();

        mThumbImg.gameObject.SetActive(false);

        mInitPos = Vector2.zero;

        mIsDraging = false;

        mBgRadius = mBgImg.rectTransform.sizeDelta.x / 2;
        mJoystickRadius = mJoystickImg.rectTransform.sizeDelta.x / 2;
        mMoveRadius = 200;

        Texture2D texture2d = (Texture2D)Resources.Load("none");
        Rect rect = new Rect(0, 0, texture2d.width, texture2d.height);
        Sprite sp = Sprite.Create(texture2d, rect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值    
        mBgImg.sprite = sp;
    }

    public bool OnPointerDown(PointerEventData eventData)
    {
        if (mTweener != null)
        {
            mTweener.Kill();
            mTweener = null;
        }

        mIsDraging = false;

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mBgImg.rectTransform,
            eventData.position, eventData.pressEventCamera, out pos))
        {
            mInitPos = pos;

            if (pos.magnitude + mJoystickRadius < mBgRadius)
            {
                mIsDraging = true;

                mJoystickImg.rectTransform.anchoredPosition = new Vector2(pos.x, pos.y);

                mThumbImg.gameObject.SetActive(true);

                float degrees = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI;
                mThumbImg.rectTransform.eulerAngles = new Vector3(0, 0, degrees - 90);
                mThumbImg.rectTransform.anchoredPosition = new Vector2(pos.x, pos.y);

                return true;
            }
        }

        return false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!mIsDraging)
            return;

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mBgImg.rectTransform,
            eventData.position, eventData.pressEventCamera, out pos))
        {
            Vector2 deltaPos = pos - mInitPos;
            deltaPos = Vector2.ClampMagnitude(deltaPos, mMoveRadius);
            pos = deltaPos + mInitPos;
            mThumbImg.rectTransform.anchoredPosition = new Vector2(pos.x, pos.y);

            float degrees = Mathf.Atan2(deltaPos.y, deltaPos.x) * 180 / Mathf.PI;
            mThumbImg.rectTransform.eulerAngles = new Vector3(0, 0, degrees - 90);

            OnJoystickMoved(deltaPos, degrees);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!mIsDraging)
            return;
        //mInputVector = Vector3.zero;
        bool _pixelSnapping = true;

        mJoystickImg.gameObject.SetActive(false);

        Vector2 pos = mThumbImg.rectTransform.anchoredPosition;
        float degrees = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI;
        mThumbImg.rectTransform.eulerAngles = new Vector3(0, 0, degrees - 90 + 180);

        mTweener = DOTween.To(() => mThumbImg.rectTransform.anchoredPosition, x => mThumbImg.rectTransform.anchoredPosition = x, Vector2.zero, 0.3f)
                .SetOptions(_pixelSnapping)
                .SetUpdate(true)
                .SetTarget(this)
                .OnComplete(() =>
                {
                    mTweener = null;

                    mJoystickImg.gameObject.SetActive(true);
                    mJoystickImg.rectTransform.anchoredPosition = Vector2.zero;

                    mThumbImg.gameObject.SetActive(false);
                    mThumbImg.rectTransform.eulerAngles = new Vector3(0, 0, 0);
                    mThumbImg.rectTransform.anchoredPosition = Vector2.zero;
                });
    }

    void OnJoystickMoved(Vector2 deltaPos, float degrees)
    {
        deltaPos = deltaPos / mMoveRadius;
        Debug.Log(deltaPos);
        InputManager.OnJoystickMoved(deltaPos, degrees);
    }

    public float Horizontal()
    {
        if (mInputVector.x != 0)
        {
            return mInputVector.x;
        }
        else
        {
            return Input.GetAxisRaw("Horizontal");
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
            return Input.GetAxisRaw("Vertical");
        }
    }
}
