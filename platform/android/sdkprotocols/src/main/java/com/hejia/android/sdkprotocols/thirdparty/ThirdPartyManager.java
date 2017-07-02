package com.hejia.android.sdkprotocols.thirdparty;

import android.app.Activity;
import android.app.Application;
import android.content.Intent;
import android.util.Log;
import android.util.SparseArray;

import com.hejia.android.clientfoundation.JsonUtils;
import com.hejia.android.clientfoundation.ResourcesManager;

import java.util.List;

/**
 * Created by hejia on 2017/6/28.
 */

public class ThirdPartyManager {
    private static final String TAG = ThirdPartyManager.class.getSimpleName();
    private static SparseArray<IThirdParty> mThirdPartys = new SparseArray<IThirdParty>();


    public static void init() {
        String jsonString = ResourcesManager.readFile("thirdparty.json");
        ThirdPartyConfig config = JsonUtils.parseObject(jsonString, ThirdPartyConfig.class);

        List<ThirdPartyConfig.Thirdparty> list = config.getInstallThirdparties();
        for (ThirdPartyConfig.Thirdparty p : list) {
            String thirdPartyName = p.getName();
            Log.i(TAG, "setup thirdparty provider:" + thirdPartyName);

            String thirdPartyClassName = p.getClassName();
            Log.i(TAG, "setup thirdparty provider class:" + thirdPartyClassName);

            IThirdParty iThirdParty = createThirdParty(thirdPartyClassName);
            Log.i(TAG, "setup thirdparty IThirdPart obj:" + iThirdParty);

            int provider = p.getProvider();
            String info = getThirdPartyParamsByProvider(provider);
            iThirdParty.setInfo(info);
            addThirdPart(provider, iThirdParty);
        }
    }

    private static IThirdParty createThirdParty(String className) {
        try {
            return (IThirdParty) Class.forName(className).newInstance();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (InstantiationException e) {
            e.printStackTrace();
        } catch (IllegalAccessException e) {
            e.printStackTrace();
        }
        return null;
    }

    private static String getThirdPartyParamsByProvider(int provider) {
        return "";
    }

    private static void addThirdPart(int provider, IThirdParty thirdParty) {
        mThirdPartys.put(provider, thirdParty);
    }

    public static void onCreate(Application application) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onCreate(application);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onCreate(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onCreate(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onStart(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onStart(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onPause(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onPause(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onResume(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onResume(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onStop(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onStop(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onRestart(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onRestart(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onNewIntent(Activity activity, Intent intent) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onNewIntent(activity, intent);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onDestroy(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onDestroy(activity);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onActivityResult(Activity activity, int requestCode, int resultCode, Intent data) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdParty listener = mThirdPartys.valueAt(i);
                listener.onActivityResult(activity, requestCode, resultCode, data);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }
}
