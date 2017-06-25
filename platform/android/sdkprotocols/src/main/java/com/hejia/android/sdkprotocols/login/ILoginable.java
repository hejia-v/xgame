package com.hejia.android.sdkprotocols.login;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;

public interface ILoginable extends IThirdPart {
    public Boolean login(ILoginListener listener);

    public Boolean logout(ILogoutListener listener);

    public Boolean switchAccount(ISwitchAccountListener listener);

    public Boolean update(IUpdateListener listener);
}
