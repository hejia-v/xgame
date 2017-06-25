package com.hejia.android.sdkprotocols.login;

public interface IUpdateListener {
    public void onUpdateFinished(int provider, boolean result, String token, String uid, ThirdPartyUserInfo userinfo);
}
