package com.hejia.android.sdkprotocols.thirdparty;

import android.app.Activity;
import android.app.Application;
import android.content.Intent;
import android.util.Log;
import android.util.SparseArray;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.regex.PatternSyntaxException;

public final class ThirdPartManager {
    private static final String TAG = ThirdPartManager.class.getSimpleName();

    private static SparseArray<IThirdPart> mThirdPartys = new SparseArray<IThirdPart>();

    private static ThirdPartManager mThirdPartyManager = null;


    public static ThirdPartManager getInstance() {
        if (mThirdPartyManager == null) {
            mThirdPartyManager = new ThirdPartManager();
        }
        return mThirdPartyManager;
    }

    public void initByApplication(Application application) {
        initThirdPartySdkAdapter(application);
    }

    public void initByActivity(Activity activity) {
        initThirdPartySdkAdapter(activity);
    }

    public IThirdPart getThirdPartyByProvider(int provider) {
        return mThirdPartys.get(provider);
    }


    public String getThirdPartyParamsByProvider(int provider) {
        return "";
    }

    public static String getThirdPartyNameByProvider(int provider) {
        return ThirdPartConfig.getInstance().getThirdPartyProperty(provider, ThirdPartConfig.CONFIG_CLASS_NAME);
    }

    public List<String> getRequestCodeByProvider(int provider) {
        List<String> list = null;

        String s = ThirdPartConfig.getInstance().getThirdPartyProperty(provider, ThirdPartConfig.CONFIG_REQUESTCODE);
        try {
            String[] array = s.split("\\|");
            list = Arrays.asList(array);
        } catch (NullPointerException e) {
            e.printStackTrace();
        } catch (PatternSyntaxException e) {
            e.printStackTrace();
        }

        return list;
    }

    private ThirdPartManager() {
        configThirdParts();
    }

    private void addThirdPart(int provider, IThirdPart thirdParty) {
        mThirdPartys.put(provider, thirdParty);
    }

    private void configThirdParts() {
        Log.i(TAG, "configThirdParts ...");
        ArrayList<Integer> providers = ThirdPartConfig.getInstance().getAllProviders();
        for (int i : providers) {
            Log.i(TAG, "configThirdParts provider:" + i);

            String thirdPartyName = ThirdPartConfig.getInstance().getThirdPartyProperty(i, ThirdPartConfig.CONFIG_CLASS_NAME);
            Log.i(TAG, "configThirdParts provider class:" + thirdPartyName);
            if (thirdPartyName.isEmpty()) {
                continue;
            }

            IThirdPart iThirdParty = ThirdPartCreator.create(thirdPartyName);
            Log.i(TAG, "configThirdParts IThirdPart obj:" + iThirdParty);

            // Config third part
            String config = getThirdPartyParamsByProvider(i);
            Log.i(TAG, "configThirdParts config:" + config);
            iThirdParty.configDeveloperInfo(config);
            addThirdPart(i, iThirdParty);
        }
    }

    private void initThirdPartySdkAdapter(Application application) {
        for (int i = 0; i < mThirdPartys.size(); ++i) {
            IThirdPart listener = mThirdPartys.valueAt(i);
            listener.init(application);
        }
    }

    private void initThirdPartySdkAdapter(Activity activity) {
        for (int i = 0; i < mThirdPartys.size(); ++i) {
            IThirdPart listener = mThirdPartys.valueAt(i);
            listener.init(activity);
        }
    }

    public static boolean onActivityResult(int requestCode, int resultCode, Intent data) {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            if (ThirdPartManager.getThirdPartyNameByProvider(mThirdPartys.keyAt(i)) != "") {
                List<String> list = ThirdPartManager.getInstance().getRequestCodeByProvider(mThirdPartys.keyAt(i));
                if (list != null && list.contains(String.valueOf(requestCode))) {
                    try {
                        IThirdPart listener = mThirdPartys.valueAt(i);
                        listener.onPayActivityResult(requestCode, resultCode, data);
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
            }
        }
        return true;
    }

    public static void onPause() {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdPart listener = mThirdPartys.valueAt(i);
                listener.onPause();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onResume() {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdPart listener = mThirdPartys.valueAt(i);
                listener.onResume();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static void onDestroy() {
        for (int i = 0; i < mThirdPartys.size(); i++) {
            try {
                IThirdPart listener = mThirdPartys.valueAt(i);
                listener.uninit();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static SparseArray<IThirdPart> getAllThirdPartys() {
        return mThirdPartys;
    }
}

