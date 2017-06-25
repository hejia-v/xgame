package com.hejia.android.sdkprotocols.thirdparty;

import android.app.Activity;
import android.app.Application;
import android.content.Intent;

import java.util.Hashtable;


public interface IThirdPart {
    public boolean init(Activity activity);

    public boolean init(Application application);

    public void configDeveloperInfo(Hashtable<String, String> cpInfo);

    public void configDeveloperInfo(String cpInfo);

    public boolean uninit();

    public void onPause();

    public void onResume();

    public boolean onPayActivityResult(int requestCode, int resultCode, Intent data);
}
