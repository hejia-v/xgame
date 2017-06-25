package com.hejia.android.sdkprotocols.exit;

import android.util.Log;

import com.hejia.android.clientfoundation.CalledByJNI;
import com.hejia.android.clientfoundation.UIThread;
import com.hejia.android.sdkprotocols.payment.PaymentManager;
import com.hejia.android.sdkprotocols.thirdparty.PluginWrapper;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartManager;


@CalledByJNI
public final class ExitManager {

    public static int ExitAction_Quit = 1;
    public static int ExitAction_PlayAgain = 2;
    public static int ExitAction_Cancel = 3;

    private static final String TAG = PaymentManager.class.getSimpleName();

    public static void onExit(final int provider) {
        Log.d(TAG, "onExit:" + provider);

        final IExitable thirdPart = (IExitable) ThirdPartManager.getInstance().getThirdPartyByProvider(provider);
        if (thirdPart instanceof IExitable) {

            final IExitable aExit = (IExitable) thirdPart;

            final Runnable runnable = new Runnable() {
                @Override
                public void run() {

                    aExit.onExit(new IExitListener() {
                        @Override
                        public void onExitFinished(final int provider, final int actionType) {
                            PluginWrapper.runOnGLThread(new Runnable() {
                                @Override
                                public void run() {
                                    Log.d(TAG, "onExitFinished:" + provider + " actionType:" + actionType);
                                    nativeOnExitFinished(provider, actionType);
                                }
                            });
                        }
                    });
                }

            };

            UIThread.runDelayed(runnable, 100);

        } else {
            try {
                throw new Exception("not IExitable:" + provider);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    /**
     * @param actionType: enum ExitAction
     *                    {
     *                    //退出
     *                    kExitActionExit = 1,
     *                    //快速开始
     *                    kExitActionPlayAgain,
     *                    //取消退出
     *                    kExitActionClose,
     *                    };
     */
    private static native void nativeOnExitFinished(int provider, int actionType);

}