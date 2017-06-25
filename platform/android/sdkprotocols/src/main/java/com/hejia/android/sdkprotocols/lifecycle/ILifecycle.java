package com.hejia.android.sdkprotocols.lifecycle;

import android.app.Activity;
import android.app.Application;
import android.content.Intent;

public interface ILifecycle {

    public void onCreate(Application application);

    public void onCreate(Activity activity);

    public void onPause(Activity activity);

    public void onResume(Activity activity);

    public void onStop(Activity activity);

    public void onRestart(Activity activity);

    public void onNewIntent(Activity activity, Intent intent);

    public void onDestroy(Activity activity);

    public void onActivityResult(Activity activity, int requestCode, int resultCode, Intent data);
}
