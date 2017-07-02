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

    public void StartLocate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBS.BaiduLBSWrapper");
            jc.CallStatic("StartLocate");
        }
    }

    public void StopLocate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBS.BaiduLBSWrapper");
            jc.CallStatic("StopLocate");
        }
    }

    public void ClearMark()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBS.BaiduLBSWrapper");
            jc.CallStatic("ClearMark");
        }
    }

    public void StartLocationAutoNotify()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBS.BaiduLBSWrapper");
            jc.CallStatic("StartLocationAutoNotify");
        }
    }

    public void StartIndoorLocation()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBS.BaiduLBSWrapper");
            jc.CallStatic("StartIndoorLocation");
        }
    }

    public void IsHotWifi()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.hejia.android.client.BaiduLBS.BaiduLBSWrapper");
            jc.CallStatic("IsHotWifi");
        }
    }
}
