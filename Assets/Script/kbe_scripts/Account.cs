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
            Event.registerIn("reqCreateAvatar", this, "reqCreateAvatar");
            Event.registerIn("reqRemoveAvatar", this, "reqRemoveAvatar");
            Event.registerIn("selectAvatarGame", this, "selectAvatarGame");

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
            KBS_RemoveAvatar e = new KBS_RemoveAvatar();
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

        public void reqCreateAvatar(Byte roleType, string name)
        {
            Dbg.DEBUG_MSG("Account::reqCreateAvatar: roleType=" + roleType);
            baseCall("reqCreateAvatar", roleType, name);
        }

        public void reqRemoveAvatar(string name)
        {
            Dbg.DEBUG_MSG("Account::reqRemoveAvatar: name=" + name);
            baseCall("reqRemoveAvatar", name);
        }

        public void selectAvatarGame(UInt64 dbid)
        {
            Dbg.DEBUG_MSG("Account::selectAvatarGame: dbid=" + dbid);
            baseCall("selectAvatarGame", dbid);
        }
    }
}
