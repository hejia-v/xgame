package com.hejia.android.sdkprotocols.payment;

import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.regex.Pattern;

import android.os.Bundle;
import android.util.Log;
import android.util.SparseArray;

import com.hejia.android.clientfoundation.AppUtils;
import com.hejia.android.clientfoundation.CalledByJNI;
import com.hejia.android.clientfoundation.UIThread;
import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;
import com.hejia.android.sdkprotocols.thirdparty.PluginWrapper;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartManager;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartConfig;


@CalledByJNI
public final class PaymentManager {

    private static final String TAG = PaymentManager.class.getSimpleName();

    public static final int PayOrder_Result_OK = 0;
    public static final int PayOrder_Result_Failed = 1;
    public static final int PayOrder_Result_Cancelled = 2;
    public static final int PayOrder_Result_Unspecified = 3;

    public static final int PayOrder_Result_StatusMask = 0x00FFFFFF;

    public static final int PayOrder_Result_Pending = 0x01000000;
    public static final int PayOrder_Result_FlagMask = 0xFF000000;


    public static final int PayOrder_Result_PendingOK = PayOrder_Result_OK | PayOrder_Result_Pending;

    public static final Pattern PRODUCT_ID_SPLITTER = Pattern.compile("\\|");
    public static final Pattern ORDER_ID_SPLITTER = Pattern.compile(":");

    private static SparseArray<Class<?>> mPaymentProviderListener = null;
    private static PaymentManager mPaymentManager = null;


    private static void addProviderListener(int provider, Class<?> listener) {
        if (mPaymentProviderListener == null) {
            mPaymentProviderListener = new SparseArray<Class<?>>();
        }
        mPaymentProviderListener.put(provider, listener);
    }

    private static void invokeMethod(String methodName) {
        try {
            for (int i = 0; i < mPaymentProviderListener.size(); ++i) {
                Class<?> listener = mPaymentProviderListener.valueAt(i);
                Method method = listener.getMethod(methodName);
                method.invoke(null);
            }
        } catch (Exception e) {
            Log.e(TAG, e.getMessage());
        }
    }


    // Called by C layer through jni
    public static void payByProvider(final int provider, final BuyInfo buyInfo) throws Exception {
        Log.d(TAG, "payByProvider:" + provider);

        final IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(provider);

        if (thirdPart == null) {
            Log.e(TAG, "Failed to find sdk:" + provider);
            return;
        }

        final Runnable runnable = new Runnable() {
            @Override
            public void run() {
                if (thirdPart instanceof IPayable) {
                    IPayable aPay = (IPayable) thirdPart;


                    aPay.pay(buyInfo, new IPaymentListener() {
                        @Override
                        public void onPaymentFinished(final int provider, final int result, final Bundle extras) {


                            PluginWrapper.runOnGLThread(new Runnable() {
                                @Override
                                public void run() {
                                    Log.d(TAG, "nativeOnPayResult:" + provider + " result:" + result);
                                    nativeOnPayResult(provider, result, extras);
                                }
                            });
                        }
                    });
                } else {
                    try {
                        throw new Exception("not IPayable:" + provider);
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
            }

        };

        UIThread.runDelayed(runnable, 100);

    }

    // Call C layer through jni
    private static native void nativeOnPayResult(int provider, int result, Bundle extras);


    public static int getOperator() {
        Log.d(TAG, AppUtils.getOperatorName());
        return AppUtils.isChinaMobile() ? 1 : AppUtils.isChinaUnicon() ? 2 : AppUtils.isChinaTelcom() ? 3 : -1;
    }


    public static void handleUnfinishedPayment() {
        ArrayList<Integer> list = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : list) {
            IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(i);
            if (thirdPart instanceof IPayable) {
                IPayable aPay = (IPayable) thirdPart;
                aPay.handleUnfinishedPayment();
            }
        }
    }


    public static String getPaymentExtendStr(int provider) {
        String extendStr = "";
        IPayable thirdPart = (IPayable) ThirdPartManager.getInstance().getThirdPartyByProvider(provider);
        if (thirdPart instanceof IPayable) {
            IPayable uniPay = (IPayable) thirdPart;
            extendStr = uniPay.getExtendStr();
            if (extendStr != "") {
                return extendStr;
            }
        }
        return extendStr;
    }
}