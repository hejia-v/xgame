package com.hejia.android.sdkprotocols.login;

import java.util.HashMap;
import java.util.Map;

import android.util.Log;

import com.hejia.android.clientfoundation.CalledByJNI;
import com.hejia.android.clientfoundation.UIThread;
import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;
import com.hejia.android.sdkprotocols.thirdparty.PluginWrapper;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartManager;


@CalledByJNI
public final class LoginManager {
    private static final String TAG = LoginManager.class.getSimpleName();

    public static final int AccountAction_Login = 1;
    public static final int AccountAction_Logout = 2;
    public static final int AccountAction_switch = 3;
    public static final int AccountAction_update = 4;

    public static final boolean NeedThirdExitMenu = true;
    public static final boolean NoThirdExitMenu = false;


    private LoginManager() {

    }


    public static ILoginListener mIListener = new ILoginListener() {
        @Override
        public void onLoginFinished(final int provider,
                                    final boolean result,
                                    final String token,
                                    final String uid,
                                    final ThirdPartyUserInfo userinfo) {

            PluginWrapper.runOnGLThread(new Runnable() {
                @Override
                public void run() {
                    Log.d(TAG, "nativeOnActionResult:" + provider + " result:" + result + " token:" + token + " uid:" + uid);
                    nativeOnActionResult(AccountAction_Login, provider, result, token, uid, userinfo);
                }
            });
        }

        @Override
        public void onLoginCancel(int provider) {

        }
    };


    public static ILoginListener getLoginListener() {
        return mIListener;
    }


    /**
     * Called by C layer through jni
     */
    public static void login(final int provider) {
        Log.d(TAG, "login with provider:" + provider);
        final Runnable runnable = new Runnable() {
            @Override
            public void run() {

                try {
                    login(provider, new ILoginListener() {
                        @Override
                        public void onLoginFinished(final int provider,
                                                    final boolean result,
                                                    final String token,
                                                    final String uid,
                                                    final ThirdPartyUserInfo userinfo) {

                            PluginWrapper.runOnGLThread(new Runnable() {
                                @Override
                                public void run() {
                                    Log.d(TAG, "nativeOnActionResult:" + provider + " result:" + result + " token:" + token + " uid:" + uid);
                                    nativeOnActionResult(AccountAction_Login, provider, result, token, uid, userinfo);
                                }
                            });
                        }

                        @Override
                        public void onLoginCancel(int provider) {

                        }
                    });
                } catch (NotLoginProviderException e) {
                    Log.e(TAG, "NotLoginProviderException:" + e.getMessage());
                    e.printStackTrace();
                }
            }
        };
        UIThread.runDelayed(runnable, 100);
    }

    private static Boolean login(int provider, ILoginListener listener) throws NotLoginProviderException {

        IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(provider);

        if (thirdPart instanceof ILoginable) {
            ILoginable login = (ILoginable) thirdPart;
            return login.login(listener);
        } else {
            throw new NotLoginProviderException("not loginable:" + provider);
        }
    }

    public static void logout(final int provider) {
        Log.d(TAG, "logout with provider:" + provider);
        final Runnable runnable = new Runnable() {

            @Override
            public void run() {

                try {
                    logout(provider, new ILogoutListener() {

                        @Override
                        public void onLogoutFinished(final int provider,
                                                     final boolean result,
                                                     final String msg) {
                            PluginWrapper.runOnGLThread(new Runnable() {

                                @Override
                                public void run() {

                                    nativeOnActionResult(AccountAction_Logout, provider, result, msg, null, null);
                                }

                            });
                        }

                    });
                } catch (NotLoginProviderException e) {

                    e.printStackTrace();
                }
            }
        };
        UIThread.runDelayed(runnable, 100);
    }

    private static Boolean logout(int provider, ILogoutListener listener) throws NotLoginProviderException {

        IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(provider);
        if (thirdPart instanceof ILoginable) {
            ILoginable logout = (ILoginable) thirdPart;
            return logout.logout(listener);
        } else {
            throw new NotLoginProviderException("not logoutable:" + provider);
        }
    }

    public static void switchAccount(final int provider) {
        Log.d(TAG, "login with provider:" + provider);
        final Runnable runnable = new Runnable() {
            @Override
            public void run() {

                try {
                    switchAccount(provider, new ISwitchAccountListener() {
                        @Override
                        public void onSwitchAccountFinished(final int provider,
                                                            final boolean result, final String token, final String uid, final ThirdPartyUserInfo userinfo) {
                            // TODO Auto-generated method stub
                            PluginWrapper.runOnGLThread(new Runnable() {
                                @Override
                                public void run() {
                                    Log.d(TAG, "nativeOnActionResult:" + provider + " result:" + result + " token:" + token + " uid:" + uid);
                                    nativeOnActionResult(AccountAction_switch, provider, result, token, uid, userinfo);
                                }
                            });
                        }
                    });
                } catch (NotLoginProviderException e) {
                    Log.e(TAG, "NotLoginProviderException:" + e.getMessage());
                    e.printStackTrace();
                }
            }
        };
        UIThread.runDelayed(runnable, 100);
    }

    private static Boolean switchAccount(int provider, ISwitchAccountListener listener) throws NotLoginProviderException {
        IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(provider);

        if (thirdPart instanceof ILoginable) {
            ILoginable switchAccount = (ILoginable) thirdPart;
            return switchAccount.switchAccount(listener);
        } else {
            throw new NotLoginProviderException("not loginable:" + provider);
        }
    }

    public static void addPlayerInfo(final int provider, final String userName) {

        Map<String, String> userInfo = new HashMap<String, String>();
        userInfo.put("userName", userName);

        IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(provider);
        if (thirdPart instanceof IUserable) {
            IUserable userable = (IUserable) thirdPart;
            userable.submitUserInfo(userInfo);
        } else {
            Log.e(TAG, "provider " + String.valueOf(provider) + "not the userable provider!");
        }
    }

    public static void update(final int provider) {
        Log.d(TAG, "login with provider:" + provider);
        final Runnable runnable = new Runnable() {
            @Override
            public void run() {

                try {
                    update(provider, new IUpdateListener() {
                        @Override
                        public void onUpdateFinished(final int provider,
                                                     final boolean result,
                                                     final String token,
                                                     final String uid,
                                                     final ThirdPartyUserInfo userinfo) {

                            PluginWrapper.runOnGLThread(new Runnable() {
                                @Override
                                public void run() {
                                    Log.d(TAG, "nativeOnActionResult:" + provider + " result:" + result + " token:" + token + " uid:" + uid);
                                    nativeOnActionResult(AccountAction_update, provider, result, token, uid, userinfo);
                                }
                            });
                        }
                    });
                } catch (NotLoginProviderException e) {
                    Log.e(TAG, "NotLoginProviderException:" + e.getMessage());
                    e.printStackTrace();
                }
            }
        };
        UIThread.runDelayed(runnable, 100);
    }

    private static Boolean update(int provider, IUpdateListener listener) throws NotLoginProviderException {
        IThirdPart thirdPart = ThirdPartManager.getInstance().getThirdPartyByProvider(provider);

        if (thirdPart instanceof ILoginable) {
            ILoginable update = (ILoginable) thirdPart;
            return update.update(listener);
        } else {
            throw new NotLoginProviderException("not loginable:" + provider);
        }
    }

    /**
     * Called by C layer through jni
     */
    private static native void nativeOnActionResult(int action, int provider, boolean result, String token, String uid, ThirdPartyUserInfo userinfo);

}
