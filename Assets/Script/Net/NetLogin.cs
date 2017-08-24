using KBEngine;
using System;
using System.Collections.Generic;

// KES 即 kbe event struct
public struct KES_Login : IKBEvent
{
    public string username;
    public string password;
    public byte[] datas;
}

public struct KES_Kicked : IKBEvent
{
    public UInt16 failedcode;
}

public struct KES_ConnState : IKBEvent
{
    public bool success;
}

public struct KBS_CreateAccountResult : IKBEvent
{
    public UInt16 retcode;
    public byte[] datas;
}

public struct KBS_LoginFailed : IKBEvent
{
    public UInt16 failedcode;
}

public struct KBS_VersionMatch : IKBEvent
{
    public string verInfo;
    public string serVerInfo;
}

public struct KBS_Failed : IKBEvent
{
    public UInt16 failedcode;
}

public struct KBS_LoginSuccess : IKBEvent
{
    public UInt64 rndUUID;
    public Int32 eid;
    public Account accountEntity;
}

public struct KBS_AvatarList : IKBEvent
{
    public Dictionary<UInt64, Dictionary<string, object>> avatarList;
}

public struct KBS_CreateAvatarResult : IKBEvent
{
    public Byte retcode;
    public object info;
    public Dictionary<UInt64, Dictionary<string, object>> avatarList;
}

public struct KBS_RemoveAvatar : IKBEvent
{
    public UInt64 dbid;
    public Dictionary<UInt64, Dictionary<string, object>> avatarList;
}

public struct KBS_ : IKBEvent
{

}
