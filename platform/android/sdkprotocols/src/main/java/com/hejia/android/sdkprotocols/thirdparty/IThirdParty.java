package com.hejia.android.sdkprotocols.thirdparty;

/**
 * Created by hejia on 2017/6/29.
 */

import android.app.Activity;
import android.app.Application;
import android.content.Intent;

import java.util.Hashtable;


public interface IThirdParty {
    public void setInfo(String cpInfo);

    public void onCreate(Application application);

    public void onCreate(Activity activity);

    public void onStart(Activity activity);

    public void onPause(Activity activity);

    public void onResume(Activity activity);

    public void onStop(Activity activity);

    public void onRestart(Activity activity);

    public void onNewIntent(Activity activity, Intent intent);

    public void onDestroy(Activity activity);

    public void onActivityResult(Activity activity, int requestCode, int resultCode, Intent data);
}