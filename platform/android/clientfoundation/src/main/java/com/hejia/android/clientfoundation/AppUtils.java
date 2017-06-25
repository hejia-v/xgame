package com.hejia.android.clientfoundation;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ActivityManager;
import android.app.ActivityManager.RunningTaskInfo;
import android.app.KeyguardManager;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Debug.MemoryInfo;
import android.os.Environment;
import android.os.StatFs;
import android.os.Vibrator;
import android.provider.Settings;
import android.telephony.SmsManager;
import android.telephony.TelephonyManager;
import android.text.TextUtils;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.WindowManager;

import java.io.File;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.util.Enumeration;
import java.util.List;
import java.util.Locale;


public class AppUtils {


    private static final String TAG = AppUtils.class.getSimpleName();

    /**
     * 默认振动时长
     */
    public static final long DEFAULT_VIBRATE_TIME = 800;

    /**
     * 用于格式化market url
     */
    private static final String MARKET_URL_FORMAT = "market://details?id=%s";

    /**
     * 获取应用程序名字
     *
     * @param context
     * @return
     */
    public static String getAppName(Context context) {
        return getAppName(context, context.getPackageName());
    }

    /**
     * 获取应用程序名字
     *
     * @param context
     * @return
     */
    public static String getAppName(Context context, String packageName) {
        String appName = "";
        final PackageManager packageManager = context.getPackageManager();
        try {
            final ApplicationInfo applicationInfo = packageManager
                    .getApplicationInfo(packageName, 0);
            appName = (String) applicationInfo.loadLabel(packageManager);
        } catch (final NameNotFoundException e) {
            e.printStackTrace();
        }
        return appName;
    }

    public static String getVersionName(Context context) {
        final PackageManager manager = context.getPackageManager();
        final String name = context.getPackageName();
        try {
            final PackageInfo info = manager.getPackageInfo(name, 0);
            return info.versionName;
        } catch (final NameNotFoundException e) {
            return "";
        } finally {
            System.gc();
        }
    }

    /**
     * 取得主程序version code
     *
     * @param context
     * @return
     */
    public static int getVersionCode(Context context) {
        return getVersionCode(context, context.getPackageName());
    }

    /**
     * 取得指定package name程序的version code
     *
     * @param context
     * @param packageName
     * @return
     */
    public static int getVersionCode(Context context, String packageName) {
        int versionCode = 0;
        final PackageManager pm = context.getPackageManager();
        try {
            final PackageInfo packageInfo = pm.getPackageInfo(packageName, 0);
            if (null == packageInfo) {
                return 0;
            }
            versionCode = packageInfo.versionCode;
        } catch (final NameNotFoundException e) {
            e.printStackTrace();
        }
        return versionCode;
    }

    // 震动,默认震动800ms
    public static void vibrate(Context context) {
        vibrate(context, DEFAULT_VIBRATE_TIME);
    }

    public static void vibrate(Context context, long milliseconds) {
        final Vibrator vib = (Vibrator) context
                .getSystemService(Context.VIBRATOR_SERVICE);
        vib.vibrate(milliseconds);
    }

    /**
     * 调用系统share
     *
     * @param context
     * @return
     */
    public static void share(Context context, String subject, String body) {
        final Intent i = new Intent(Intent.ACTION_SEND);
        i.putExtra(Intent.EXTRA_SUBJECT, subject);
        i.putExtra(Intent.EXTRA_TEXT, body);
        i.setType("text/plain");
        i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        try {
            context.startActivity(i);
        } catch (final ActivityNotFoundException e) {
        }
    }

    /*
     * 调用系统的web
     *
     * @param url
     * */
    public static void openBrowser(Context context, String url) {
        final Uri uri = Uri.parse(url);
        String action = Intent.ACTION_VIEW;
        final String scheme = uri.getScheme();
        if ("tel".equals(scheme)) {
            action = Intent.ACTION_DIAL;
        }
        final Intent intent = new Intent(action, uri);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        context.startActivity(intent);
    }

    /**
     * 打开音乐播放器
     */
    public static void openMusicPlayer(Context context, File musicFile) {
        final Intent intent = new Intent(Intent.ACTION_VIEW);
        final Uri uri = Uri.fromFile(musicFile);
        intent.setDataAndType(uri, "audio/*");
        try {
            context.startActivity(intent);
        } catch (final ActivityNotFoundException e) {
            e.printStackTrace();
        }
    }

    /**
     * 跳到market去下载或打分
     *
     * @param context
     */
    public static void doRating(Context context) {
        final Uri marketUri = Uri.parse(String.format(MARKET_URL_FORMAT,
                context.getPackageName()));
        final Intent intent = new Intent(Intent.ACTION_VIEW, marketUri);
        intent.addCategory(Intent.CATEGORY_BROWSABLE);
        try {
            context.startActivity(intent);
        } catch (final ActivityNotFoundException e) {
            e.printStackTrace();
        }
    }

    /**
     * 返回Android ID
     */
    @Deprecated
    public static String getAndroidId(Context context) {
        final String andoridId = Settings.Secure.getString(
                context.getContentResolver(), Settings.Secure.ANDROID_ID);
        return andoridId;
    }

    /**
     * 获取Device ID
     *
     * @param context
     * @return
     */
    public static String getDeviceId(Context context) {
        final DeviceUuidFactory uuidFactory = new DeviceUuidFactory(context);
        /**
         * use uuidfactory to get deviceId ,assure to get deviceid
         * android pad don't have simcard, can't get deviceId, use androidId instead
         */
        final String imei = uuidFactory.getDeviceId();
        return imei;
    }

    /**
     * 获取操作系统的版本号
     *
     * @return
     */
    public static String getAndroidVersion() {
        return android.os.Build.VERSION.RELEASE;
    }

    public static String getDeviceModel() {
        return android.os.Build.MODEL;
    }

    /**
     * 获取当前手机使用的语言
     *
     * @return
     */
    public static String getDeviceLanguage() {
        return Locale.getDefault().getLanguage();
    }

    /**
     * Calculates the free memory of the device. This is based on an inspection
     * of the filesystem, which in android devices is stored in RAM.
     *
     * @return Number of bytes available.
     */
    public static long getAvailableInternalMemorySize() {
        final File path = Environment.getDataDirectory();
        final StatFs stat = new StatFs(path.getPath());
        final long blockSize = stat.getBlockSize();
        final long availableBlocks = stat.getAvailableBlocks();
        return availableBlocks * blockSize;
    }

    /**
     * Calculates the total memory of the device. This is based on an inspection
     * of the filesystem, which in android devices is stored in RAM.
     *
     * @return Total number of bytes.
     */
    public static long getTotalInternalMemorySize() {
        final File path = Environment.getDataDirectory();
        final StatFs stat = new StatFs(path.getPath());
        final long blockSize = stat.getBlockSize();
        final long totalBlocks = stat.getBlockCount();
        return totalBlocks * blockSize;
    }

    /**
     * 获得程序使用的内存
     */
    public static long getProcessUsedMemory(Context context) {
        int pid = android.os.Process.myPid();
        ActivityManager activityManager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        MemoryInfo[] memoryInfo = activityManager.getProcessMemoryInfo(new int[]{pid});
        return memoryInfo[0].getTotalPrivateDirty();
    }

    /**
     * 保持屏幕唤醒状态（即背景灯不熄灭）
     *
     * @param on 是否唤醒
     */
    @SuppressLint("Wakelock")
    @SuppressWarnings("deprecation")
    public static void keepScreenOn(Context context, boolean on) {
        final Activity act = (Activity) context;
        if (on) {
            act.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    act.getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
                }
            });


        } else {
            act.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    act.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
                }
            });
        }
    }

    /**
     * 需要权限<uses-permission
     * android:name="android.permission.ACCESS_WIFI_STATE"></uses-permission>
     * <uses-permission
     * android:name="android.permission.INTERNET"></uses-permission> 获取本机IP地址
     *
     * @return
     */
    @Deprecated
    public static String getLocalIpAddress() {
        try {
            for (final Enumeration<NetworkInterface> en = NetworkInterface
                    .getNetworkInterfaces(); en.hasMoreElements(); ) {
                final NetworkInterface intf = en.nextElement();
                for (final Enumeration<InetAddress> enumIpAddr = intf
                        .getInetAddresses(); enumIpAddr.hasMoreElements(); ) {
                    final InetAddress inetAddress = enumIpAddr.nextElement();
                    if (!inetAddress.isLoopbackAddress()) {
                        return inetAddress.getHostAddress().toString();
                    }
                }
            }
        } catch (final SocketException ex) {
            Log.e("ifo", ex.toString());
        }
        return "";
    }

    /**
     * 需要权限<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
     *
     * @return
     */
    public static String getIpAddress(final Context context) {
        WifiManager wifiManager = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);
        WifiInfo wifiInfo = wifiManager.getConnectionInfo();
        return intToIp(wifiInfo.getIpAddress());
    }

    private static String intToIp(int i) {
        return (i & 0xFF) + "." + ((i >> 8) & 0xFF) + "." + ((i >> 16) & 0xFF)
                + "." + (i >> 24 & 0xFF);
    }

    /**
     * 获取本机mac地址
     *
     * @param context
     * @return
     */
    public static String getLocalMacAddress(Context context) {
        final WifiManager wifi = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);
        final WifiInfo info = wifi.getConnectionInfo();
        return info.getMacAddress();
    }

    /**
     * 获取手机当前网络类型
     *
     * @param context
     * @return
     */
    public static String getNetworkTypeName(Context context) {
        final ConnectivityManager cm = (ConnectivityManager) context
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        final NetworkInfo info = cm.getActiveNetworkInfo();
        String typeName = null;
        if (info != null) {
            typeName = info.getTypeName();
        }
        return typeName;
    }

    public static int getNetworkType(Context context) {

        final ConnectivityManager cm = (ConnectivityManager) context
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        final NetworkInfo info = cm.getActiveNetworkInfo();
        int type = 0;
        if (info != null) {
            type = info.getType();
        }
        return type;
    }

    public static boolean isAppOnForeground(Context context) {
        final ActivityManager manager = (ActivityManager) context
                .getSystemService(Context.ACTIVITY_SERVICE);
        final List<RunningTaskInfo> taskInfo = manager.getRunningTasks(1);
        if (taskInfo.size() == 0) {
            return false;
        }
        final RunningTaskInfo task = taskInfo.get(0);
        final String topAppPackageName = task.topActivity.getPackageName();
        return topAppPackageName.equals(context.getPackageName());
    }

    public static int getNetworkSubtype(Context context) {

        final ConnectivityManager cm = (ConnectivityManager) context
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        final NetworkInfo info = cm.getActiveNetworkInfo();
        int type = 0;
        if (info != null) {
            type = info.getSubtype();
        }
        return type;
    }

    public static String getNetworkSubtypeName(Context context) {
        final ConnectivityManager cm = (ConnectivityManager) context
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        final NetworkInfo info = cm.getActiveNetworkInfo();
        String typeName = null;
        if (info != null) {
            typeName = info.getSubtypeName();
        }
        return typeName;
    }

    public static boolean isScreenLocked(Context c) {
        final KeyguardManager km = (KeyguardManager) c
                .getSystemService(Context.KEYGUARD_SERVICE);
        return km.inKeyguardRestrictedInputMode();
    }

    public static int[] getScreenWH(Context context) {
        final WindowManager wm = (WindowManager) context
                .getSystemService(Context.WINDOW_SERVICE);
        final DisplayMetrics outMetrics = new DisplayMetrics();
        wm.getDefaultDisplay().getMetrics(outMetrics);
        final int wh[] = new int[2];
        wh[0] = outMetrics.widthPixels;
        wh[1] = outMetrics.heightPixels;
        return wh;
    }

    @Deprecated
    public static boolean isChinaMobile2() {
        final Context context = AppContext.getInstance();
        // android 获取sim卡运营商信息
        final TelephonyManager tm = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        // TelephonyManager 的使用 TelephonyManager
        // 提供设备上获取通讯服务信息的入口，应用程序使用这个类的方法来获取电话的服务商或者状态。程序也可以注册一个监听器来监听电话状态的改变。不需要直接实例化这个类，使用Context.getSystemService(Context.TELEPHONY_SERVICE)来获取这个类的实例。
        // 注意：一些电话信息需要相应的权限。方法无效
        // getSimOperatorName() Returns the Service Provider Name (SPN). //
        // 获取服务提供商名字，比如电信，联通，移动用下面的方法第一种方法: 获取手机的IMSI码,并判断是中国移动\中国联通\中国电信
        // /** 获取SIM卡的IMSI码 * SIM卡唯一标识：IMSI 国际移动用户识别码（IMSI：International Mobile
        // Subscriber Identification Number）是区别移动用户的标志， *
        // 储存在SIM卡中，可用于区别移动用户的有效信息。
        // IMSI由MCC、MNC、MSIN组成，其中MCC为移动国家号码，由3位数字组成， *
        // 唯一地识别移动客户所属的国家，我国为460；MNC为网络id，由2位数字组成， *
        // 用于识别移动客户所归属的移动网络，中国移动为00，中国联通为01,中国电信为03；
        // MSIN为移动客户识别码，采用等长11位数字构成。 *
        // 唯一地识别国内GSM移动通信网中移动客户。所以要区分是移动还是联通，只需取得SIM卡中的MNC字段即可 */
        final String imsi = tm.getSubscriberId();
        if (imsi != null) {
            if (imsi.startsWith("46000") || imsi.startsWith("46002")) {// 因为移动网络编号46000下的IMSI已经用完，所以虚拟了一个46002编号，134/159号段使用了此编号
                return true;
            } else if (imsi.startsWith("46001")) {
                // 中国联通
            } else if (imsi.startsWith("46003")) {
                // 中国电信
            }
        }
        return false;
    }

    public static boolean isChinaMobile() {
        // 第二种方法
        final Context context = AppContext.getInstance();
        ;
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String operator = telManager.getSimOperator();
        if (operator != null && (operator.equals("46000") || operator.equals("46002")
                || operator.equals("46007"))) {
            // 中国移动
            return true;
        }
        return false;
    }

    public static boolean isChinaUnicon() {
        // 第二种方法
        final Context context = AppContext.getInstance();
        ;
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String operator = telManager.getSimOperator();
        if (operator != null && (operator.equals("46001") || operator.equals("46006"))) {
            // 中国联通
            return true;
        }
        return false;
    }

    public static boolean isChinaTelcom() {
        // 第二种方法
        final Context context = AppContext.getInstance();
        ;
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String operator = telManager.getSimOperator();
        if (operator != null && (operator.equals("46003") || operator.equals("46005"))) {
            // 中国电信
            return true;
        }
        return false;
    }

    public static String getOperatorName() {
        // 第二种方法
        String operatorNameString = "";
        final Context context = AppContext.getInstance();
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String operator = telManager.getSimOperator();
        operatorNameString = telManager.getSimOperatorName();
        if (operator != null) {
            if (operator.equals("46000") || operator.equals("46002")
                    || operator.equals("46007")) {
                // 中国移动
                operatorNameString = "ChinaMobile";
            } else if (operator.equals("46001")
                    || operator.equals("46006")) {
                // 中国联通
                operatorNameString = "ChinaUnicon";
            } else if (operator.equals("46003")
                    || operator.equals("46005")
                    ) {
                // 中国电信
                operatorNameString = "ChinaTelecom";
            }
        }
        Log.d(TAG, "getOperatorName complete with:" + operatorNameString);
        return operatorNameString;
    }

    public static long getFirstInstallTime(Context context) {
        final PackageManager pm = context.getPackageManager();
        try {
            PackageInfo packageInfo = pm.getPackageInfo(context.getPackageName(), 0);
            long time = packageInfo.firstInstallTime;//api 9
            return time;
        } catch (final NameNotFoundException e) {
            // TODO Add exception handling here.
            e.printStackTrace();
        }
        return -1;
    }

    //if this app is first install,it's equal to install time
    public static long getLastUpdateTime(Context context) {
        final ApplicationInfo appInfo = context.getApplicationInfo();
        String appFile = appInfo.sourceDir;
        long time = new File(appFile).lastModified();
        return time;
    }

    public static String getSIMNumber() {
        final Context context = AppContext.getInstance();
        ;
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String number = telManager.getLine1Number();
        return number;
    }

    public static String getSIMIMEI() {
        final Context context = AppContext.getInstance();
        ;
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String number = telManager.getSimSerialNumber();
        return number;
    }

    public static String getSIMIMSI() {
        final Context context = AppContext.getInstance();
        ;
        final TelephonyManager telManager = (TelephonyManager) context
                .getSystemService(Context.TELEPHONY_SERVICE);
        final String number = telManager.getSubscriberId();
        return number;
    }

    /**
     * send sms.this function need permission
     * <uses-permission android:name="android.permission.SEND_SMS" />
     *
     * @param context
     * @param type:0  start a sms activity; 1 send sms local
     * @param number
     * @param message
     */
    public static void sendSMS(final Context context, final int type, final String number, final String message) {
        if (!TextUtils.isEmpty(message)) {
            switch (type) {
                case 0:
                    sendSMSWithActivity((Activity) context, number, message);
                    break;
                case 1:
                    sendSMSBackstage(number, message);
                    break;
                default:
                    break;
            }
        }
    }

    /**
     * send sms in local activity, not in back stage
     *
     * @param activity
     * @param number
     * @param message
     */
    public static void sendSMSWithActivity(final Activity activity, final String number, final String message) {
        String numStr = TextUtils.isEmpty(number) ? "smsto:" : "smsto:" + number;
        Uri uri = Uri.parse(numStr);
        Intent it = new Intent(Intent.ACTION_SENDTO, uri);
        it.putExtra("sms_body", message);
        activity.startActivity(it);
    }

    /**
     * send sms with a service , it's send in background and user may not know
     *
     * @param number
     * @param message
     */
    public static void sendSMSBackstage(final String number, final String message) {
        if (TextUtils.isEmpty(number)) {
            Log.i(TAG, "send msg with out number.");
        } else {
            SmsManager manager = SmsManager.getDefault();
            List<String> devideContents = manager.divideMessage(message);
            for (String msg : devideContents) {
                manager.sendTextMessage(number, null, msg, null, null);
            }
        }
    }

    /**
     * check the packageName app is installed or not
     *
     * @param context
     * @param packagename
     * @return
     */
    public static boolean isPackageInstalled(final Context context, final String packagename) {
        PackageManager pm = context.getPackageManager();
        try {
            pm.getPackageInfo(packagename, PackageManager.GET_ACTIVITIES);
            return true;
        } catch (NameNotFoundException e) {
            return false;
        }
    }

    public static int getAppIcon(Context context) {
        int ret = 0;
        try {
            PackageManager pm = context.getPackageManager();
            ApplicationInfo ai = pm.getApplicationInfo(context.getPackageName(), 0);
            ret = ai.icon;
        } catch (NameNotFoundException e) {
            // ...
            e.printStackTrace();
        }
        return ret;
    }
}
