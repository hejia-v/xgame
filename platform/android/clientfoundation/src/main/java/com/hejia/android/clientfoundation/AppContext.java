package com.hejia.android.clientfoundation;

import android.content.Context;
import android.content.ContextWrapper;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Looper;

public class AppContext extends ContextWrapper {
    private static AppContext sInstance;

    private CharSequence mAppLabel;

    private AppContext(Context context) {
        super(context);
    }

    public static synchronized void init(Context context) {
        if (sInstance == null) {
            try {
                sInstance = new AppContext(context.createPackageContext(
                        context.getPackageName(), Context.CONTEXT_INCLUDE_CODE));
            } catch (NameNotFoundException e) {
                throw new RuntimeException(e);
            }
        }
    }

    public static AppContext getInstance() {
        return sInstance;
    }

    public static void checkThread() {
        if (Thread.currentThread() != Looper.getMainLooper().getThread()) {
            throw new IllegalAccessError(
                    "this method should be called in main thread");
        }
    }

    public CharSequence getAppLabel() {
        if (null == mAppLabel) {
            final PackageManager pm = getPackageManager();
            mAppLabel = getApplicationInfo().loadLabel(pm);
        }
        return mAppLabel;
    }

}
