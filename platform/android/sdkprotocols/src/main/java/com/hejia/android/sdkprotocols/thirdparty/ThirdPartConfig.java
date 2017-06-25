package com.hejia.android.sdkprotocols.thirdparty;

import android.util.Log;
import android.util.SparseArray;

import com.hejia.android.clientfoundation.JsonParser;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;


public class ThirdPartConfig {
    private static final String TAG = ThirdPartConfig.class.getSimpleName();
    private static ThirdPartConfig mThirdPartyConfig = null;
    private JsonParser mJsonParser = null;

    private SparseArray<Map<String, String>> mThirdPartyInfo = null;
    private static final String CONFIG_FILE_NAME = "thirdparty.json";
    private static String CONFIG_PROVIDER = "install";
    private static String CONFIG_DPAY_PROVIDER = "dpay";
    private static String CONFIG_SDK_NAME = "name";

    public static String CONFIG_CLASS_NAME = "className";
    public static String CONFIG_PARAMS = "params";
    public static String CONFIG_REQUESTCODE = "requestCode";
    public static String CONFIG_TYPE = "type";
    public static String CONFIG_GAME_NAME = "game_name";

    private static String[] ConfigProperties = {"className", "params", "requestCode", "game_name"};

    public static ThirdPartConfig getInstance() {
        if (mThirdPartyConfig == null) {
            mThirdPartyConfig = new ThirdPartConfig();
        }
        return mThirdPartyConfig;
    }

    public ArrayList<Integer> getAllProviders() {
        return mJsonParser.getSubJsonArrayValue(CONFIG_PROVIDER);
    }

    public ArrayList<Integer> getDpayProviders() {
        return mJsonParser.getSubJsonArrayValue(CONFIG_DPAY_PROVIDER);
    }

    public String getThirdPartyProperty(int provider, String propertykey) {
        if (mThirdPartyInfo.indexOfKey(provider) < 0) {
            Log.d(TAG, "There is not any info about the provider of " + String.valueOf(provider) + " in the thirdparty.json");
            return "";
        }
        if (!mThirdPartyInfo.get(provider).containsKey(propertykey)) {
            Log.d(TAG, "There is no config info about " + propertykey + " in the provider of " + String.valueOf(provider));
            return "";
        }
        if (mThirdPartyInfo.get(provider).get(propertykey) == null) {
            return "";
        }
        return mThirdPartyInfo.get(provider).get(propertykey);
    }

    private ThirdPartConfig() {
        mJsonParser = new JsonParser(CONFIG_FILE_NAME);
        initThirdPartyInfo();
    }

    private String getThirdValue(String provider, String key) {
        return mJsonParser.getSubValue(provider, key);
    }

    private void addThirdPartyInfo(int provider, Map<String, String> thirdPartyInfo) {
        if (mThirdPartyInfo == null) {
            mThirdPartyInfo = new SparseArray<Map<String, String>>();
        }
        mThirdPartyInfo.put(provider, thirdPartyInfo);
    }

    private void initThirdPartyInfo() {
        if (mThirdPartyInfo == null) {
            ArrayList<Integer> listKey = getAllProviders();
            for (int i : listKey) {
                Map<String, String> thirdPartyInfo = new HashMap<String, String>();
                for (int j = 0; j < ConfigProperties.length; j++) {
                    thirdPartyInfo.put(ConfigProperties[j], getThirdValue(String.valueOf(i), ConfigProperties[j]));
                }
                addThirdPartyInfo(i, thirdPartyInfo);
            }
        }
    }
}
