package com.hejia.android.sdkprotocols.exit;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;


public interface IExitable extends IThirdPart {

    public void onExit(IExitListener listener);

}