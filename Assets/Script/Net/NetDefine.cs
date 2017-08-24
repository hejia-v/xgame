
/// <summary>
/// net event type
/// </summary>
public enum NET
{
    None,
    login,

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
