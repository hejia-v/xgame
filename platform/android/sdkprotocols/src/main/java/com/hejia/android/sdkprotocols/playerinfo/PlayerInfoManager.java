package com.hejia.android.sdkprotocols.playerinfo;

import android.util.SparseArray;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartManager;

public class PlayerInfoManager {
    public static PlayerInfo playerInfo = new PlayerInfo();

    public static void onRefreshPlayerInfo(final String playerId, final String playerName, final String playerCTime) {
        playerInfo.playerId = playerId;
        playerInfo.playerName = playerName;
        playerInfo.playerCTime = Long.parseLong(playerCTime);

        SparseArray<IThirdPart> thirdpart = ThirdPartManager.getAllThirdPartys();

        for (int i = 0; i < thirdpart.size(); ++i) {
            if (thirdpart.valueAt(i) instanceof IPlayerInfo) {
                IPlayerInfo instance = (IPlayerInfo) thirdpart.valueAt(i);
                instance.onRefreshPlayerInfo(playerInfo);
            }
        }
    }
}
