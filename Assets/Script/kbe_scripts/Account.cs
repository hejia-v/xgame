namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Account : KBEngine.KBEGameObject
    {
        public Dictionary<UInt64, Dictionary<string, object>> avatars = new Dictionary<UInt64, Dictionary<string, object>>();

        public Account()
        {
        }

        public override void __init__()
        {
            // 注册事件
            KBEvent.registerIn(KET.reqCreateAvatar, this, reqCreateAvatar);
            KBEvent.registerIn(KET.reqRemoveAvatar, this, reqRemoveAvatar);
            KBEvent.registerIn(KET.selectAvatarGame, this, selectAvatarGame);

            // 触发登陆成功事件
            KBS_LoginSuccess e = new KBS_LoginSuccess();
            e.rndUUID = KBEngineApp.app.entity_uuid;
            e.eid = id;
            e.accountEntity = this;
            KBEvent.fireOut(KET.onLoginSuccessfully, e);

            // 向服务端请求获得角色列表
            baseCall("reqAvatarList");
        }

        public override void onDestroy()
        {
            KBEngine.Event.deregisterIn(this);
        }

        public void onCreateAvatarResult(Byte retcode, object info)
        {
            if (retcode == 0)
            {
                avatars.Add((UInt64)((Dictionary<string, object>)info)["dbid"], (Dictionary<string, object>)info);
                Dbg.DEBUG_MSG("Account::onCreateAvatarResult: name=" + (string)((Dictionary<string, object>)info)["name"]);
            }
            else
            {
                Dbg.ERROR_MSG("Account::onCreateAvatarResult: retcode=" + retcode);
            }

            // ui event
            KBS_CreateAvatarResult e = new KBS_CreateAvatarResult();
            e.retcode = retcode;
            e.info = info;
            e.avatarList = avatars;
            KBEvent.fireOut(KET.onCreateAvatarResult, e);
        }

        public void onRemoveAvatar(UInt64 dbid)
        {
            Dbg.DEBUG_MSG("Account::onRemoveAvatar: dbid=" + dbid);

            avatars.Remove(dbid);

            // ui event
            KBS_RemoveAvatarResp e = new KBS_RemoveAvatarResp();
            e.dbid = dbid;
            e.avatarList = avatars;
            KBEvent.fireOut(KET.onRemoveAvatar, e);
        }

        public void onReqAvatarList(Dictionary<string, object> infos)
        {
            avatars.Clear();

            List<object> listinfos = (List<object>)infos["values"];

            Dbg.DEBUG_MSG("Account::onReqAvatarList: avatarsize=" + listinfos.Count);
            for (int i = 0; i < listinfos.Count; i++)
            {
                Dictionary<string, object> info = (Dictionary<string, object>)listinfos[i];
                Dbg.DEBUG_MSG("Account::onReqAvatarList: name" + i + "=" + (string)info["name"]);
                avatars.Add((UInt64)info["dbid"], info);
            }

            // ui event
            Dictionary<UInt64, Dictionary<string, object>> avatarList = new Dictionary<ulong, Dictionary<string, object>>(avatars);
            KBS_AvatarList e = new KBS_AvatarList();
            e.avatarList = avatarList;
            KBEvent.fireOut(KET.onReqAvatarList, e);

            if (listinfos.Count == 0)
                return;

            // selectAvatarGame(avatars.Keys.ToList()[0]);
        }

        public void reqCreateAvatar(IKBEvent eventData)
        {
            KBS_CreateAvatar data = (KBS_CreateAvatar)eventData;
            Dbg.DEBUG_MSG("Account::reqCreateAvatar: roleType=" + data.roleType);
            baseCall("reqCreateAvatar", data.roleType, data.name);
        }

        public void reqRemoveAvatar(IKBEvent eventData)
        {
            KBS_RemoveAvatar e = (KBS_RemoveAvatar)eventData;
            Dbg.DEBUG_MSG("Account::reqRemoveAvatar: name=" + e.name);
            baseCall("reqRemoveAvatar", e.name);
        }

        public void selectAvatarGame(IKBEvent eventData)
        {
            KBS_EnterGame e = (KBS_EnterGame)eventData;
            Dbg.DEBUG_MSG("Account::selectAvatarGame: dbid=" + e.dbid);
            baseCall("selectAvatarGame", e.dbid);
        }
    }
}
