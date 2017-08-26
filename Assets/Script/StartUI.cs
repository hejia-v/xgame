﻿using KBEngine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MainUIStatus
{
    Login,
    SelAvatarUI,
    WorldUI,
}

public class StartUI : MonoBehaviour
{
    Text mVersionLabel;
    Text mMessageLabel;
    GameObject mLoginUI;
    InputField mAccountText;
    InputField mPasswordText;

    bool startRelogin = false;
    private Dictionary<UInt64, Dictionary<string, object>> ui_avatarList = null;

    private MainUIStatus uiStatus = MainUIStatus.Login;
    [ExposeProperty]
    public MainUIStatus UIStatus
    {
        get
        {
            return uiStatus;
        }
        set
        {
            if (uiStatus != value)
            {
                onUIStatusChanged();
            }
            uiStatus = value;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        installEvents();
        GameObject canvas = GameObject.Find("Canvas");

        mVersionLabel = canvas.transform.Find("Version").GetComponent<Text>();
        mMessageLabel = canvas.transform.Find("Message").GetComponent<Text>();
        mLoginUI = canvas.transform.Find("LoginUI").gameObject;

        mAccountText = mLoginUI.transform.Find("Account").GetComponent<InputField>();
        mPasswordText = mLoginUI.transform.Find("Password").GetComponent<InputField>();

        Button loginBtn = mLoginUI.transform.Find("BtnLogin").GetComponent<Button>();
        loginBtn.onClick.AddListener(this.login);

        Button signupBtn = mLoginUI.transform.Find("BtnSignup").GetComponent<Button>();
        signupBtn.onClick.AddListener(this.createAccount);

        //SceneManager.LoadScene("login");
    }

    void Update()
    {

    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }

    void installEvents()
    {
        // common
        KBEvent.registerOut(KET.onKicked, this, onKicked);
        KBEvent.registerOut(KET.onDisconnected, this, onDisconnected);
        KBEvent.registerOut(KET.onConnectionState, this, onConnectionState);

        // login
        KBEvent.registerOut(KET.onCreateAccountResult, this, onCreateAccountResult);
        KBEvent.registerOut(KET.onLoginFailed, this, onLoginFailed);
        KBEvent.registerOut(KET.onVersionNotMatch, this, onVersionNotMatch);
        KBEvent.registerOut(KET.onScriptVersionNotMatch, this, onScriptVersionNotMatch);
        KBEvent.registerOut(KET.onLoginBaseappFailed, this, onLoginBaseappFailed);
        KBEvent.registerOut(KET.onLoginSuccessfully, this, onLoginSuccessfully);
        KBEvent.registerOut(KET.onReloginBaseappFailed, this, onReloginBaseappFailed);
        KBEvent.registerOut(KET.onReloginBaseappSuccessfully, this, onReloginBaseappSuccessfully);
        KBEvent.registerOut(KET.onLoginBaseapp, this, onLoginBaseapp);
        KBEvent.registerOut(KET.Loginapp_importClientMessages, this, Loginapp_importClientMessages);
        KBEvent.registerOut(KET.Baseapp_importClientMessages, this, Baseapp_importClientMessages);
        KBEvent.registerOut(KET.Baseapp_importClientEntityDef, this, Baseapp_importClientEntityDef);

        // select-avatars(register by scripts)
        KBEvent.registerOut(KET.onReqAvatarList, this, onReqAvatarList);
        KBEvent.registerOut(KET.onCreateAvatarResult, this, onCreateAvatarResult);
        KBEvent.registerOut(KET.onRemoveAvatar, this, onRemoveAvatar);
    }

    void onUIStatusChanged()
    {
        Debug.Log("[StartUI] onUIStatusChanged!");
    }

    public void info(string s)
    {
        mMessageLabel.color = Color.green;
        mMessageLabel.text = s;
    }

    public void err(string s)
    {
        mMessageLabel.color = Color.red;
        mMessageLabel.text = s;
    }

    public void login()
    {
        string sAccount = mAccountText.text;
        string sPasswd = mPasswordText.text;
        if (sAccount.Length < 0 || sPasswd.Length <= 5)
        {
            err("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)");
            return;
        }

        info("connect to server...(连接到服务端...)");

        KES_Login eventData = new KES_Login();
        eventData.username = sAccount;
        eventData.password = sPasswd;
        eventData.datas = System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo");
        KBEvent.fireIn(KET.login, eventData);
    }

    public void createAccount()
    {
        info("connect to server...(连接到服务端...)");

        //KBEngine.Event.fireIn("createAccount", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
    }

    public void onKicked(IKBEvent eventData)
    {
        KES_Kicked data = (KES_Kicked)eventData;
        err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(data.failedcode));
        //SceneManager.LoadScene("login");
        UIStatus = MainUIStatus.Login;
    }

    public void onDisconnected(IKBEvent eventData)
    {
        err("disconnect! will try to reconnect...(你已掉线，尝试重连中!)");
        startRelogin = true;
        Invoke("onReloginBaseappTimer", 1.0f);
    }

    public void onReloginBaseappTimer()
    {
        if (UIStatus == MainUIStatus.Login)
        {
            err("disconnect! (你已掉线!)");
            return;
        }

        KBEngineApp.app.reloginBaseapp();

        if (startRelogin)
            Invoke("onReloginBaseappTimer", 3.0f);
    }

    public void onConnectionState(IKBEvent eventData)
    {
        KES_ConnState data = (KES_ConnState)eventData;
        if (!data.success)
            err("connect(" + KBEngineApp.app.getInitArgs().ip + ":" + KBEngineApp.app.getInitArgs().port + ") is error! (连接错误)");
        else
            info("connect successfully, please wait...(连接成功，请等候...)");
    }

    public void onCreateAccountResult(IKBEvent eventData)
    {
        KBS_CreateAccountResult data = (KBS_CreateAccountResult)eventData;
        if (data.retcode != 0)
        {
            err("createAccount is error(注册账号错误)! err=" + KBEngineApp.app.serverErr(data.retcode));
            return;
        }

        string sAccount = mAccountText.text;
        if (KBEngineApp.validEmail(sAccount))
        {
            info("createAccount is successfully, Please activate your Email!(注册账号成功，请激活Email!)");
        }
        else
        {
            info("createAccount is successfully!(注册账号成功!)");
        }
    }

    public void onLoginFailed(IKBEvent eventData)
    {
        KBS_Failed data = (KBS_Failed)eventData;
        if (data.failedcode == 20)
        {
            err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(data.failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
        }
        else
        {
            err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(data.failedcode));
        }
    }

    public void onVersionNotMatch(IKBEvent eventData)
    {
        //KBS_VersionMatch data = (KBS_VersionMatch)eventData;
        err("");
    }

    public void onScriptVersionNotMatch(IKBEvent eventData)
    {
        //KBS_VersionMatch data = (KBS_VersionMatch)eventData;
        err("");
    }

    public void onLoginBaseappFailed(IKBEvent eventData)
    {
        KBS_Failed data = (KBS_Failed)eventData;
        err("loginBaseapp is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr(data.failedcode));
    }

    public void onLoginSuccessfully(IKBEvent eventData)
    {
        //KBS_LoginSuccess data = (KBS_LoginSuccess)eventData;
        info("login is successfully!(登陆成功!)");
        UIStatus = MainUIStatus.SelAvatarUI;

        //Application.LoadLevel("selavatars");
    }

    public void onReloginBaseappFailed(IKBEvent eventData)
    {
        KBS_Failed data = (KBS_Failed)eventData;
        err("relogin is failed(重连网关失败), err=" + KBEngineApp.app.serverErr(data.failedcode));
        startRelogin = false;
    }

    public void onReloginBaseappSuccessfully(IKBEvent eventData)
    {
        info("relogin is successfully!(重连成功!)");
        startRelogin = false;
    }

    public void onLoginBaseapp(IKBEvent eventData)
    {
        info("connect to loginBaseapp, please wait...(连接到网关， 请稍后...)");
    }

    public void Loginapp_importClientMessages(IKBEvent eventData)
    {
        info("Loginapp_importClientMessages ...");
    }

    public void Baseapp_importClientMessages(IKBEvent eventData)
    {
        info("Baseapp_importClientMessages ...");
    }

    public void Baseapp_importClientEntityDef(IKBEvent eventData)
    {
        info("importClientEntityDef ...");
    }

    public void onReqAvatarList(IKBEvent eventData)
    {
        KBS_AvatarList data = (KBS_AvatarList)eventData;
        ui_avatarList = data.avatarList;
    }

    public void onCreateAvatarResult(IKBEvent eventData)
    {
        KBS_CreateAvatarResult data = (KBS_CreateAvatarResult)eventData;
        if (data.retcode != 0)
        {
            err("Error creating avatar, errcode=" + data.retcode);
            return;
        }

        KBS_AvatarList listData = new KBS_AvatarList();
        listData.avatarList = data.avatarList;
        onReqAvatarList(listData);
    }

    public void onRemoveAvatar(IKBEvent eventData)
    {
        KBS_RemoveAvatar data = (KBS_RemoveAvatar)eventData;
        if (data.dbid == 0)
        {
            err("Delete the avatar error!(删除角色错误!)");
            return;
        }

        KBS_AvatarList listData = new KBS_AvatarList();
        listData.avatarList = data.avatarList;
        onReqAvatarList(listData);
    }


}
