package com.hejia.android.sdkprotocols.login;

import java.util.Map;

public interface IUserable {
    public void submitUserInfo(Map<String, String> userInfo);

    public void registerRealName();

    public void addictionQuery();
}
