using System;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCardUI : MonoBehaviour
{
    Image mPanel;
    Image mIcon;
    Text mNameLabel;
    Text mLevelLabel;

    private string iconPath;
    public string Icon
    {
        get
        {
            return iconPath;
        }
        set
        {
            iconPath = value;

            Texture2D texture2d = (Texture2D)Resources.Load(value);
            Rect rect = new Rect(0, 0, texture2d.width, texture2d.height);
            Sprite sp = Sprite.Create(texture2d, rect, new Vector2(0.5f, 0.5f));  //注意居中显示采用0.5f值    
            mIcon.sprite = sp;
        }
    }

    public string Name
    {
        get
        {
            return mNameLabel.text;
        }
        set
        {
            mNameLabel.text = value;
        }
    }

    private int level = 0;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            mLevelLabel.text = value.ToString() + "级";
        }
    }

    private bool isSelected = false;
    public bool Select
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
            if (isSelected)
            {
                mPanel.color = new Color(0.9f, 0.9f, 1, 1);
            }
            else
            {
                mPanel.color = new Color(1, 1, 1, 1);
            }
        }
    }

    private UInt64 idbid;
    public UInt64 DBId
    {
        get
        {
            return idbid;
        }
        set
        {
            idbid = value;
        }
    }

    void Start()
    {
        //why do not work?
        mPanel = GetComponent<Image>();
        mIcon = transform.Find("Icon").GetComponent<Image>();
        mNameLabel = transform.Find("Name").GetComponent<Text>();
        mLevelLabel = transform.Find("Level").GetComponent<Text>();

        //uiIcon.sprite = Resources.Load<Sprite>((Mathf.Abs(count) % 30 + 1).ToString("icon000"));
    }

    public void Init()
    {
        mPanel = GetComponent<Image>();
        mIcon = transform.Find("Icon").GetComponent<Image>();
        mNameLabel = transform.Find("Name").GetComponent<Text>();
        mLevelLabel = transform.Find("Level").GetComponent<Text>();
    }
}
