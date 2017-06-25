package com.hejia.android.sdkprotocols.thirdparty;

import java.lang.reflect.Field;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.LinkedHashSet;
import java.util.Set;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.opengl.GLSurfaceView;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;


public class PluginWrapper {

    protected static Context sContext = null;
    protected static GLSurfaceView sGLSurfaceView = null;
    protected static Handler sMainThreadHandler = null;
    protected static Handler sGLThreadHandler = null;
    protected static Set<PluginListener> sListeners = new LinkedHashSet<PluginListener>();
    private static final String TAG = "PluginWrapper";

    public static void init(Context context) {
        sContext = context;
        if (null == sMainThreadHandler) {
            sMainThreadHandler = new Handler();
        }
    }

    public static void setGLSurfaceView(GLSurfaceView value) {
        sGLSurfaceView = value;
    }

    protected static void initFromNativeActivity(Activity act) {
        sContext = act;
    }

    public static void onResume() {
        for (PluginListener listener : sListeners) {
            listener.onResume();
        }
    }

    public static void onPause() {
        for (PluginListener listener : sListeners) {
            listener.onPause();
        }
    }

    public static void onDestroy() {
        Iterator<PluginListener> i = sListeners.iterator();
        while (i.hasNext()) {
            PluginListener p = i.next();
            p.onDestroy();
        }
    }

    public static boolean onActivityResult(int requestCode, int resultCode, Intent data) {
        boolean result = true;

        for (PluginListener listener : sListeners) {
            result = result && listener.onActivityResult(requestCode, resultCode, data);
        }

        return result;
    }

    public static void addListener(PluginListener listener) {
        sListeners.add(listener);
    }

    public static void removeListener(PluginListener listener) {
        sListeners.remove(listener);
    }

    protected static Object initPlugin(String classFullName) {
        Log.i(TAG, "class name : ----" + classFullName + "----");
        Class<?> c = null;
        try {
            String fullName = classFullName.replace('/', '.');
            c = Class.forName(fullName);
        } catch (ClassNotFoundException e) {
            Log.e(TAG, "Class " + classFullName + " not found.");
            e.printStackTrace();
            return null;
        }

        try {
            Context ctx = getContext();
            if (ctx != null) {
                Object o = c.getDeclaredConstructor(Context.class).newInstance(ctx);
                return o;
            } else {
                Log.e(TAG, "Plugin " + classFullName + " wasn't initialized.");
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    protected static int getPluginType(Object obj) {
        int nRet = -1;
        try {
            Field filedID = obj.getClass().getField("PluginType");
            Integer nObj = (Integer) filedID.get(obj);
            nRet = nObj.intValue();
        } catch (Exception e) {
            e.printStackTrace();
        }

        return nRet;
    }

    public static Context getContext() {
        return sContext;
    }

    public static void runOnGLThread(Runnable r) {
        if (null != sGLSurfaceView) {
            sGLSurfaceView.queueEvent(r);
        } else if (null != sGLThreadHandler) {
            sGLThreadHandler.post(r);
        } else {
            Log.i(TAG, "call back invoked on main thread");
            r.run();
        }
    }

    public static void runOnMainThread(Runnable r) {
        if (null != sMainThreadHandler) {
            sMainThreadHandler.post(r);
        } else if (null != sContext && sContext instanceof Activity) {
            Activity act = (Activity) sContext;
            act.runOnUiThread(r);
        }
    }

    private static String[] arrPlugins = {"PluginUser", "PluginShare", "PluginSocial", "PluginAds", "PluginAnalytics", "PluginIAP"};

    public static Hashtable<String, String> getPluginConfigure() {
        Hashtable<String, String> ht = new Hashtable<String, String>();
        try {
            ApplicationInfo ai = sContext.getPackageManager().getApplicationInfo(sContext.getPackageName(), PackageManager.GET_META_DATA);
            Bundle bundle = ai.metaData;

            for (int i = 0; i < arrPlugins.length; ++i) {
                String pluginName = bundle.getString(arrPlugins[i]);
                if (null != pluginName && !"".equals(pluginName))
                    ht.put(arrPlugins[i], pluginName);
            }
        } catch (NameNotFoundException e) {
            e.printStackTrace();
        }

        return ht;
    }
}
