package com.hejia.android.clientfoundation;

import android.os.Handler;

public class UIThread {
    private static Handler sHandler = null;

    public static void init() {
        sHandler = new Handler();
    }

    public static void run(Runnable runnable) {
        if (sHandler == null) {
            throw new RuntimeException("please frist call init in Main thread");
        }
        if (runnable != null) {
            sHandler.post(runnable);
        }
    }

    public static void release() {
        sHandler = null;
    }

    public static void runDelayed(Runnable runnable, long delay) {
        if (sHandler == null) {
            throw new RuntimeException("please frist call init in Main thread");
        }
        if (runnable != null) {
            sHandler.postDelayed(runnable, delay);
        }
    }
}
