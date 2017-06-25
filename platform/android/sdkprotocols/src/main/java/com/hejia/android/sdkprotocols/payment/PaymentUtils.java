package com.hejia.android.sdkprotocols.payment;

import android.app.Activity;
import android.app.ActivityManager;
import android.app.ActivityManager.RunningAppProcessInfo;
import android.app.ProgressDialog;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

import javax.crypto.Mac;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;


public class PaymentUtils {

    private static final String TAG = PaymentUtils.class.getSimpleName();
    private final static Pattern reponseSplit = Pattern.compile("&");
    private final static Pattern equalSplit = Pattern.compile("=");
    private static PaymentUtils mPaymentUtils;

    public static PaymentUtils getInstance() {
        if (mPaymentUtils == null) {
            mPaymentUtils = new PaymentUtils();
        }
        return mPaymentUtils;
    }


    /**
     * convert NumberString to a highly recognizable form. 200000->20万  2000->2千
     *
     * @param s       : The target of the optimization operation
     * @param unit    : messurement
     * @param strUnit : after optimization the unit will append the output string
     * @return : return optimized string
     */
    public String optimizeNumberString(String s, int unit, String strUnit) {
        s = s.trim();
        String resNum = "";
        String resStr = "";
        String res = "";
        for (int i = 0; i < s.length(); ++i) {
            if (Character.isDigit(s.charAt(i))) {
                resNum += s.charAt(i);
            } else {
                resStr += s.charAt(i);
            }
        }
        float count = Integer.parseInt(resNum);
        if (count > unit) {
            count /= unit;
            res = String.valueOf(count);
            res += strUnit;
        } else {
            res = String.valueOf(count);
        }
        res += resStr;
        return res;
    }

    /**
     * check if every character in a string is a digit
     *
     * @param str to check
     * @return return check result
     */
    public boolean isNumeric(String str) {
        for (int i = str.length(); --i >= 0; ) {
            if (!Character.isDigit(str.charAt(i))) {
                return false;
            }
        }
        return true;
    }

    /**
     * Show processing dialog
     *
     * @param progressDialog
     * @param activity
     * @param strToShow
     */
    public void showProgressDialog(ProgressDialog progressDialog, Activity activity, String strToShow) {
        if (progressDialog == null) {
            progressDialog = new ProgressDialog(activity);
            progressDialog.setIndeterminate(true);
            progressDialog.setMessage(strToShow);
        }
        if (!progressDialog.isShowing()) {
            progressDialog.show();
        }
    }

    public void dismissProgressDialog(ProgressDialog progressDialog) {
        if (progressDialog != null && progressDialog.isShowing()) {
            progressDialog.dismiss();
        }
    }


    /**
     * check if app is running foreground
     */
    public boolean isAppOnForeground(Activity activity) {
        ActivityManager activityManager = (ActivityManager) activity
                .getSystemService(Context.ACTIVITY_SERVICE);
        String packageName = activity.getPackageName();
        List<RunningAppProcessInfo> appProcesses = activityManager.getRunningAppProcesses();
        if (appProcesses == null) {
            return false;
        }
        for (RunningAppProcessInfo appProcess : appProcesses) {
            if (appProcess.processName.equals(packageName)
                    && appProcess.importance == RunningAppProcessInfo.IMPORTANCE_FOREGROUND) {
                return true;
            }
        }
        return false;
    }

    /**
     * check if app is running background
     */
    public boolean isAppOnBackground(Activity activity) {
        ActivityManager activityManager = (ActivityManager) activity
                .getSystemService(Context.ACTIVITY_SERVICE);
        String packageName = activity.getPackageName();
        List<RunningAppProcessInfo> appProcesses = activityManager.getRunningAppProcesses();
        if (appProcesses == null) {
            return false;
        }
        for (RunningAppProcessInfo appProcess : appProcesses) {
            if (appProcess.processName.equals(packageName)
                    && appProcess.importance == RunningAppProcessInfo.IMPORTANCE_BACKGROUND) {
                return true;
            }
        }
        return false;
    }

    /**
     * check if network is available?
     */
    public boolean isMobileNetworkAvailable(Context connect) {
        final ConnectivityManager conMgr = (ConnectivityManager) connect
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        final NetworkInfo wifiInfo = conMgr.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
        final NetworkInfo mobileInfo = conMgr.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
        if (wifiInfo.isAvailable()) {
            return true;
        } else if (mobileInfo.isAvailable()) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * write key value to local path
     * TODO: Map<String, String> can be extended to a template,the type of key and value.
     *
     * @param activity
     * @param paramsMap: you can pass data you want to write to json file in Map<String, String> data structure.
     * @param fileName:  you can customize file name, don't add suffix like .json.
     */
    public void saveKeyAndValueToJsonFile(Activity activity, final Map<String, String> paramsMap, String fileName) {
        String jsonStr = "";
        try {
            File file = new File(activity.getFilesDir() + "/" + fileName + ".json");
            if (!file.exists()) {
                file.createNewFile();
            }

            FileInputStream fin = activity.openFileInput(fileName + ".json");
            int length = fin.available();
            byte[] bufferIn = new byte[length];
            fin.read(bufferIn);
            jsonStr = new String(bufferIn, "UTF-8");
            fin.close();

            if (jsonStr.equals("")) {
                jsonStr = "{}";
            }
            JSONObject jsonObj = new JSONObject(jsonStr);
            Iterator<Map.Entry<String, String>> it = paramsMap.entrySet().iterator();
            while (it.hasNext()) {
                Map.Entry<String, String> entry = it.next();
                if (entry.getKey() != null && entry.getValue() != null) {
                    jsonObj.putOpt(entry.getKey(), entry.getValue());
                } else {
                    jsonObj.put(entry.getKey(), entry.getValue());
                }
            }
            jsonStr = jsonObj.toString();
            byte[] bufferOut = jsonStr.getBytes("UTF-8");

            FileOutputStream fout = activity.openFileOutput(fileName + ".json", activity.MODE_PRIVATE);
            fout.write(bufferOut);
            fout.close();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * query key and value from the file fileName.json, the result will be stored in List<String>
     *
     * @param activity
     * @param fileName : the filename you want to read, don't add suffix like .json.
     * @return Map<String, String> all key and value pairs in json file will be store in Map
     */
    public Map<String, String> readKeyAndValueFromJsonFile(Activity activity, String fileName) {
        String jsonStr = "";
        String key = "";
        String value = "";
        Map<String, String> checkRes = new HashMap<String, String>();
        try {
            FileInputStream fin = activity.openFileInput(fileName + ".json");
            int length = fin.available();
            byte[] bufferIn = new byte[length];
            fin.read(bufferIn);
            jsonStr = new String(bufferIn, "UTF-8");
            fin.close();

            JSONObject jsonObj = new JSONObject(jsonStr);
            Iterator<String> it = jsonObj.keys();
            while (it.hasNext()) {
                key = (String) it.next();
                value = jsonObj.getString(key);
                if (key != null && value != null) {
                    checkRes.put(key, value);
                }
            }
            return checkRes;
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return checkRes;
    }

    /**
     * query value with the given key from the json file
     *
     * @param activity
     * @param key      : query key
     * @param filename : the filename you want to read, don't add suffix like .json.
     * @return : return value
     */
    public String queryValueByKeyFromJsonFile(Activity activity, String key, String filename) {
        String jsonStr = "";
        String value = "";
        if (key != "") {
            try {
                FileInputStream fin = activity.openFileInput(filename + ".json");
                int length = fin.available();
                byte[] bufferIn = new byte[length];
                fin.read(bufferIn);
                jsonStr = new String(bufferIn, "UTF-8");
                fin.close();

                JSONObject jsonObj = new JSONObject(jsonStr);
                Iterator<String> it = jsonObj.keys();
                while (it.hasNext()) {
                    if ((String) it.next() != "" && key == (String) it.next()) {
                        value = jsonObj.getString(key);
                    }
                }
            } catch (IOException e) {
                e.printStackTrace();
            } catch (JSONException e) {
                e.printStackTrace();
            }
        } else {
            Log.w(TAG, "null query key!");
        }

        return value;
    }

    /**
     * delete key and related value from the file with the given name
     *
     * @param activity
     * @param key      : the key that will be deleted from json file
     * @param fileName : don't add suffix like .json.
     */
    public void deleteKeyAndValueFromJsonFile(Activity activity, String key, String fileName) {
        String jsonStr = "";
        try {
            FileInputStream fin = activity.openFileInput(fileName + ".json");
            int length = fin.available();
            byte[] bufferIn = new byte[length];
            fin.read(bufferIn);
            jsonStr = new String(bufferIn, "UTF-8");
            fin.close();

            JSONObject jsonObj = new JSONObject(jsonStr);
            jsonObj.remove(key);
            jsonStr = jsonObj.toString();
            byte[] bufferOut = jsonStr.getBytes("UTF-8");

            FileOutputStream fout = activity.openFileOutput(fileName + ".json", activity.MODE_PRIVATE);
            fout.write(bufferOut);
            fout.close();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    private static final String MAC_NAME = "HmacSHA1";
    private static final String ENCODING = "UTF-8";

    /**
     * 使用 HMAC-SHA1 签名方法对对encryptText进行签名
     *
     * @param encryptText 被签名的字符串
     * @param encryptKey  密钥
     * @return
     * @throws Exception
     */
    public String hmacSHA1Encrypt(String encryptText, String encryptKey) throws Exception {
        byte[] data = encryptKey.getBytes(ENCODING);
        //根据给定的字节数组构造一个密钥,第二参数指定一个密钥算法的名称  
        SecretKey secretKey = new SecretKeySpec(data, MAC_NAME);
        //生成一个指定 Mac 算法 的 Mac 对象  
        Mac mac = Mac.getInstance(MAC_NAME);
        //用给定密钥初始化 Mac 对象  
        mac.init(secretKey);

        byte[] text = encryptText.getBytes(ENCODING);
        //完成 Mac 操作   
        return byteArrayToHexString(mac.doFinal(text));
    }


    private String byteArrayToHexString(byte[] bytes) {
        StringBuffer sb = new StringBuffer();
        for (int i = 0; i < bytes.length; i++) {
            sb.append(byteToHexString(bytes[i]));
        }
        return sb.toString();
    }


    /**
     * 将一个字节转化成十六进制形式的字符串
     *
     * @param b 字节数组
     * @return 字符串
     */
    private String byteToHexString(byte b) {
        int ret = b;
        //System.out.println("ret = " + ret);  
        if (ret < 0) {
            ret += 256;
        }
        int m = ret / 16;
        int n = ret % 16;
        return hexDigits[m] + hexDigits[n];
    }

    private final String[] hexDigits = {"0", "1", "2", "3", "4", "5",
            "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"};

}
