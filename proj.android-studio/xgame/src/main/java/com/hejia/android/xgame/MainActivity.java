package com.hejia.android.xgame;

/**
 * Created by hejia on 2017/5/28.
 */

import android.app.AlertDialog;
import android.os.Bundle;
import android.os.Vibrator;
import android.util.Log;
import android.widget.Toast;

import com.hejia.android.clientfoundation.AppInfo;
import com.hejia.android.clientfoundation.UIThread;
import com.hejia.android.xgame.unity.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;

public class MainActivity extends UnityPlayerActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        String sha1 = AppInfo.getCertificateSHA1Fingerprint(this);
        Log.i("32131232131","eeqweqeqew");
        UIThread.init();
    }

    public String ShowDialog(final String _title, final String _content) {

        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.this);
                builder.setTitle(_title).setMessage(_content).setPositiveButton("Down", null);
                builder.show();
            }
        });

        return "Java return";
    }

    // 定义一个显示Toast的方法，在Unity中调用此方法
    public void ShowToast(final String mStr2Show) {
        // 同样需要在UI线程下执行
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(getApplicationContext(), mStr2Show, Toast.LENGTH_LONG).show();
            }
        });
    }


    //  定义一个手机振动的方法，在Unity中调用此方法
    public void SetVibrator() {
        Vibrator mVibrator = (Vibrator) getSystemService(VIBRATOR_SERVICE);
        mVibrator.vibrate(new long[]{200, 2000, 2000, 200, 200, 200}, -1); //-1：表示不重复 0：循环的震动
    }

    // 第一个参数是unity中的对象名字，记住是对象名字，不是脚本类名
    // 第二个参数是函数名
    // 第三个参数是传给函数的参数，目前只看到一个参数，并且是string的，自己传进去转吧
    public void callUnityFunc(String _objName, String _funcStr, String _content) {
        UnityPlayer.UnitySendMessage(_objName, _funcStr, "Come from:" + _content);
    }
}
