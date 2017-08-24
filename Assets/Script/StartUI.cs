using KBEngine;
using System;
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
    public MainUIStatus uiStatus = MainUIStatus.Login;

    Text mVersionLabel;
    Text mMessageLabel;
    InputField mAccountText;
    InputField mPasswordText;

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
        mAccountText = canvas.transform.Find("Account").GetComponent<InputField>();
        mPasswordText = canvas.transform.Find("Password").GetComponent<InputField>();

        Button loginBtn = canvas.transform.Find("BtnLogin").GetComponent<Button>();
        loginBtn.onClick.AddListener(this.login);

        Button signupBtn = canvas.transform.Find("BtnSignup").GetComponent<Button>();
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
        //KBEngine.Event.registerOut("onKicked", this, "onKicked");
        //KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        //KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
        KBEvent.registerOut(NET.onKicked, this, onKicked);

        // login
        //KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
        //KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        //KBEngine.Event.registerOut("onVersionNotMatch", this, "onVersionNotMatch");
        //KBEngine.Event.registerOut("onScriptVersionNotMatch", this, "onScriptVersionNotMatch");
        //KBEngine.Event.registerOut("onLoginBaseappFailed", this, "onLoginBaseappFailed");
        //KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
        //KBEngine.Event.registerOut("onReloginBaseappFailed", this, "onReloginBaseappFailed");
        //KBEngine.Event.registerOut("onReloginBaseappSuccessfully", this, "onReloginBaseappSuccessfully");
        //KBEngine.Event.registerOut("onLoginBaseapp", this, "onLoginBaseapp");
        //KBEngine.Event.registerOut("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
        //KBEngine.Event.registerOut("Baseapp_importClientMessages", this, "Baseapp_importClientMessages");
        //KBEngine.Event.registerOut("Baseapp_importClientEntityDef", this, "Baseapp_importClientEntityDef");

        // select-avatars(register by scripts)
        //KBEngine.Event.registerOut("onReqAvatarList", this, "onReqAvatarList");
        //KBEngine.Event.registerOut("onCreateAvatarResult", this, "onCreateAvatarResult");
        //KBEngine.Event.registerOut("onRemoveAvatar", this, "onRemoveAvatar");
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

        C2S_Login eventData = new C2S_Login(sAccount, sPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
        KBEvent.fireIn(NET.login, eventData);
    }

    public void createAccount()
    {
        info("connect to server...(连接到服务端...)");

        //KBEngine.Event.fireIn("createAccount", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
    }

    public void onKicked(KBEventData eventData)
    {
        S2C_Kicked data = (S2C_Kicked)eventData;
        err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(data.failedcode));
        //SceneManager.LoadScene("login");
        uiStatus = MainUIStatus.Login;
    }
}
