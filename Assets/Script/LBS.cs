using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBS : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public void Startlocate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBSWrapper");
            jc.CallStatic("Startlocate");
        }
    }

    public void ClearMark()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBSWrapper");
            jc.CallStatic("ClearMark");
        }
    }

    public void StartLocationAutoNotify()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBSWrapper");
            jc.CallStatic("StartLocationAutoNotify");
        }
    }

    public void StartIndoorLocation()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBSWrapper");
            jc.CallStatic("StartIndoorLocation");
        }
    }

    public void IsHotWifi()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBSWrapper");
            jc.CallStatic("IsHotWifi");
        }
    }
}
