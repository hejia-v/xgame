package com.hejia.android.sdkprotocols.uninstall;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;

public interface IUninstallable extends IThirdPart {
    public void setUninstallOpenUrl(final String url);
}
