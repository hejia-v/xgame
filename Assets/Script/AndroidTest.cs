﻿using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class AndroidTest : MonoBehaviour
{
    private Text mText;

    void Start()
    {
        //int ret = MyAddFunc(200, 200);
        //Debug.LogFormat("--- ret:{0}", ret);
        mText = GameObject.Find("MsgText").GetComponent<Text>();
    }

    public void MyShowDialog()
    {
        // Android的Java接口
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        // 参数
        string[] mObject = new string[2];
        mObject[0] = "Jar4Android";
        mObject[1] = "Wow,Amazing!It's worked!";
        // 调用方法
        string ret = jo.Call<string>("ShowDialog", mObject);
        setMsg(ref ret);
    }

    public void MyShowToast()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("ShowToast", "Showing on Toast");
    }

    /// <summary>
    /// 测试 unity->java->unity
    /// </summary>
    public void MyInteraction()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("callUnityFunc", "U2J2U", "BeCallFunc", "yangx");
    }

    public void BeCallFunc(string _content)
    {
        setMsg(ref _content);
    }

    private void setMsg(ref string _str)
    {
        mText.text = _str;
    }
}