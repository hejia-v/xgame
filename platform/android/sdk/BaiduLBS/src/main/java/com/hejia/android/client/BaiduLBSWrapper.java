package com.hejia.android.client;

/**
 * Created by hejia on 2017/6/26.
 */

import android.app.Activity;
import android.app.Application;
import android.content.Intent;

import com.hejia.android.clientfoundation.JsonUtils;
import com.hejia.android.clientfoundation.ResourcesManager;
import com.hejia.android.sdkprotocols.thirdparty.IThirdParty;

import java.util.Hashtable;

public class BaiduLBSWrapper implements IThirdParty {

    @Override
    public void setInfo(Hashtable<String, String> cpInfo) {

    }

    @Override
    public void setInfo(String cpInfo) {

    }

    @Override
    public void onCreate(Application application) {

    }

    @Override
    public void onCreate(Activity activity) {
        String jsonString = ResourcesManager.readFile("BaiduLBS.json");
        BaiduLBSConfig config = JsonUtils.parseObject(jsonString, BaiduLBSConfig.class);
    }

    @Override
    public void onPause(Activity activity) {

    }

    @Override
    public void onResume(Activity activity) {

    }

    @Override
    public void onStop(Activity activity) {

    }

    @Override
    public void onRestart(Activity activity) {

    }

    @Override
    public void onNewIntent(Activity activity, Intent intent) {

    }

    @Override
    public void onDestroy(Activity activity) {

    }

    @Override
    public void onActivityResult(Activity activity, int requestCode, int resultCode, Intent data) {

    }

    public static void Startlocate() {

    }

    public static void ClearMark() {

    }

    public static void StartLocationAutoNotify() {

    }

    public static void StartIndoorLocation() {

    }

    public static void IsHotWifi() {

    }
}
