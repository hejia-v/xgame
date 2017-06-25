package com.hejia.android.sdkprotocols.login;


public interface ILoginListener {
    public void onLoginFinished(int provider, boolean result, String token, String uid, ThirdPartyUserInfo userinfo);

    public void onLoginCancel(int provider);
}
