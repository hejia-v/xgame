package com.hejia.android.sdkprotocols.login;

public interface ILogoutListener {
    public void onLogoutFinished(final int provider, final boolean result, final String msg);
}
