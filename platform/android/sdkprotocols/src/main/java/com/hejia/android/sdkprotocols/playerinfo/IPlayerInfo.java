package com.hejia.android.sdkprotocols.playerinfo;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;

public interface IPlayerInfo extends IThirdPart {
    public void onRefreshPlayerInfo(final PlayerInfo info);
}
