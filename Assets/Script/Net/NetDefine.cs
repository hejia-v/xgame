
/// <summary>
/// kbe event type
/// </summary>
public enum KET
{
    None,
    login,
    createAccount,
    reqCreateAvatar,
    reqRemoveAvatar,
    selectAvatarGame,

    // common
    onKicked,
    onDisconnected,
    onConnectionState,

    // login
    onCreateAccountResult,
    onLoginFailed,
    onVersionNotMatch,
    onScriptVersionNotMatch,
    onLoginBaseappFailed,
    onLoginSuccessfully,
    onReloginBaseappFailed,
    onReloginBaseappSuccessfully,
    onLoginBaseapp,
    Loginapp_importClientMessages,
    Baseapp_importClientMessages,
    Baseapp_importClientEntityDef,

    // select-avatars(register by scripts)
    onReqAvatarList,
    onCreateAvatarResult,
    onRemoveAvatar,
}
