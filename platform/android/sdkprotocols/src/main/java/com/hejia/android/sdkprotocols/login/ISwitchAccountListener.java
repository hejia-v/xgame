package com.hejia.android.sdkprotocols.login;

public interface ISwitchAccountListener {
    public void onSwitchAccountFinished(int provider, boolean result, String token, String uid, ThirdPartyUserInfo userinfo);
}
