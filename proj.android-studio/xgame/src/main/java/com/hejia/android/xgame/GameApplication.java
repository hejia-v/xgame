package com.hejia.android.xgame;

import android.app.Application;
import android.util.Log;

import com.hejia.android.clientfoundation.AppContext;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartyManager;

/**
 * Created by hejia on 2017/6/29.
 */

public class GameApplication extends Application {
    private static final String TAG = GameApplication.class.getSimpleName();
    @Override
    public void onCreate() {
        super.onCreate();
        Log.d(TAG, "onCreate");

        AppContext.init(this);

        Long time1 = System.currentTimeMillis();

        String curTimeStr = Long.toString(time1);
        Log.d(TAG, curTimeStr);

        ThirdPartyManager.init();

        Long time2 = System.currentTimeMillis();

        curTimeStr = Long.toString(time2);
        Log.d(TAG, curTimeStr);

        String info = "time of thirdparty init application " + Long.toString(time2 - time1) + "\n";
        Log.d(TAG, info);

        ThirdPartyManager.onCreate(this);
    }
}
