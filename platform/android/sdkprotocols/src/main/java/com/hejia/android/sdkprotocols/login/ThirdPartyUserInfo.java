package com.hejia.android.sdkprotocols.login;

public class ThirdPartyUserInfo {
    private String mUsername;
    private String mRealname;
    private String mNickname;
    private String mAvatarUrl;

    public ThirdPartyUserInfo(String username, String realname, String nickname, String avatarUrl) {
        mUsername = username;
        mRealname = realname;
        mNickname = nickname;
        mAvatarUrl = avatarUrl;
    }

    public String getUsername() {
        return mUsername;
    }

    public String getRealname() {
        return mRealname;
    }

    public String getNickname() {
        return mNickname;
    }

    public String getAvatarUrl() {
        return mAvatarUrl;
    }
}
