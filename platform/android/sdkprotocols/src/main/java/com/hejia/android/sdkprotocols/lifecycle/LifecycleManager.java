package com.hejia.android.sdkprotocols.lifecycle;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Intent;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartConfig;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartManager;

public class LifecycleManager {

    private static Activity mActivity = null;

    public static void onCreate(Activity activity) {
        mActivity = activity;
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onCreate(activity);
            }
        }
    }

    public static void onPause(Activity activity) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onPause(activity);
            }
        }
    }

    public static void onResume(Activity activity) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onResume(activity);
            }
        }
    }

    public static void onStop(Activity activity) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onStop(activity);
            }
        }
    }

    public static void onRestart(Activity activity) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onRestart(activity);
            }
        }
    }

    public static void onDestroy(Activity activity) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onDestroy(activity);
            }
        }
    }

    public static void onNewIntent(Intent intent) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onNewIntent(mActivity, intent);
            }
        }
    }

    public static void onActivityResult(int requestCode, int resultCode, Intent data) {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof ILifecycle) {
                ILifecycle android = (ILifecycle) thirdPart;
                android.onActivityResult(mActivity, requestCode, resultCode, data);
            }
        }
    }
}
