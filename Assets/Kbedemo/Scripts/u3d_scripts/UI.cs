using KBEngine;
using UnityEngine;
using System; 
using System.IO;  
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour 
{
	public static UI inst;
	
	public int ui_state = 0;
	private string stringAccount = "";
	private string stringPasswd = "";
	private string labelMsg = "";
	private Color labelColor = Color.green;
	
	private Dictionary<UInt64, Dictionary<string, object>> ui_avatarList = null;
	
	private string stringAvatarName = "";
	private bool startCreateAvatar = false;

	private UInt64 selAvatarDBID = 0;
	public bool showReliveGUI = false;
	
	bool startRelogin = false;
	
	void Awake() 
	 {
		inst = this;
		DontDestroyOnLoad(transform.gameObject);
	 }
	 
	// Use this for initialization
	void Start () 
	{
		//installEvents();
		Application.LoadLevel("login");
	}



	void OnDestroy()
	{
		KBEngine.Event.deregisterOut(this);
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetKeyUp(KeyCode.Space))
        {
			Debug.Log("KeyCode.Space");
			KBEngine.Event.fireIn("jump");
        }
	}
	
	void onSelAvatarUI()
	{
		if (startCreateAvatar == false && GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 40, 200, 30), "RemoveAvatar(删除角色)"))    
        {
			if(selAvatarDBID == 0)
			{
				err("Please select a Avatar!(请选择角色!)");
			}
			else
			{
				info("Please wait...(请稍后...)");
				
				if(ui_avatarList != null && ui_avatarList.Count > 0)
				{
					Dictionary<string, object> avatarinfo = ui_avatarList[selAvatarDBID];
					KBEngine.Event.fireIn("reqRemoveAvatar", (string)avatarinfo["name"]);
				}
			}
        }

		if (startCreateAvatar == false && GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 75, 200, 30), "CreateAvatar(创建角色)"))    
		{
			startCreateAvatar = !startCreateAvatar;
		}

        if (startCreateAvatar == false && GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 110, 200, 30), "EnterGame(进入游戏)"))    
        {
        	if(selAvatarDBID == 0)
        	{
        		err("Please select a Avatar!(请选择角色!)");
        	}
        	else
        	{
        		info("Please wait...(请稍后...)");
        		
				KBEngine.Event.fireIn("selectAvatarGame", selAvatarDBID);
				Application.LoadLevel("world");
				ui_state = 2;
			}
        }
		
		if(startCreateAvatar)
		{
	        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 40, 200, 30), "CreateAvatar-OK(创建完成)"))    
	        {
	        	if(stringAvatarName.Length > 1)
	        	{
		        	startCreateAvatar = !startCreateAvatar;
					KBEngine.Event.fireIn("reqCreateAvatar", (Byte)1, stringAvatarName);
				}
				else
				{
					err("avatar name is null(角色名称为空)!");
				}
	        }
	        
	        stringAvatarName = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height - 75, 200, 30), stringAvatarName, 20);
		}
		
		if(ui_avatarList != null && ui_avatarList.Count > 0)
		{
			int idx = 0;
			foreach(UInt64 dbid in ui_avatarList.Keys)
			{
				Dictionary<string, object> info = ui_avatarList[dbid];
			//	Byte roleType = (Byte)info["roleType"];
				string name = (string)info["name"];
			//	UInt16 level = (UInt16)info["level"];
				UInt64 idbid = (UInt64)info["dbid"];

				idx++;
				
				Color color = GUI.contentColor;
				if(selAvatarDBID == idbid)
				{
					GUI.contentColor = Color.red;
				}
				
				if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 120 - 35 * idx, 200, 30), name))    
				{
					Debug.Log("selAvatar:" + name);
					selAvatarDBID = idbid;
				}
				
				GUI.contentColor = color;
			}
		}
		else
		{
			if(KBEngineApp.app.entity_type == "Account")
			{
				KBEngine.Account account = (KBEngine.Account)KBEngineApp.app.player();
				if(account != null)
					ui_avatarList = new Dictionary<ulong, Dictionary<string, object>>(account.avatars);
			}
		}
	}
	
	void onLoginUI()
	{
		if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 30), "Login(登陆)"))  
        {  
        	Debug.Log("stringAccount:" + stringAccount);
        	Debug.Log("stringPasswd:" + stringPasswd);
        	
			if(stringAccount.Length > 0 && stringPasswd.Length > 5)
			{
				login();
			}
			else
			{
				err("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)");
			}
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 70, 200, 30), "CreateAccount(注册账号)"))  
        {  
			Debug.Log("stringAccount:" + stringAccount);
			Debug.Log("stringPasswd:" + stringPasswd);

			if(stringAccount.Length > 0 && stringPasswd.Length > 5)
			{
				createAccount();
			}
			else
			{
				err("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)");
			}
        }
        
		stringAccount = GUI.TextField(new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 30), stringAccount, 20);
		stringPasswd = GUI.PasswordField(new Rect (Screen.width / 2 - 100, Screen.height / 2 - 10, 200, 30), stringPasswd, '*');
	}

	void onWorldUI()
	{
		if(showReliveGUI)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 30), "Relive(复活)"))  
			{
				KBEngine.Event.fireIn("relive", (Byte)1);		        	
			}
		}
		
		UnityEngine.GameObject obj = UnityEngine.GameObject.Find("player(Clone)");
		if(obj != null)
		{
			GUI.Label(new Rect((Screen.width / 2) - 100, 20, 400, 100), "id=" + KBEngineApp.app.entity_id + ", position=" + obj.transform.position.ToString()); 
		}
	}

    void OnGUI()  
    {  
		if(ui_state == 1)
		{
			onSelAvatarUI();
   		}
   		else if(ui_state == 2)
   		{
			onWorldUI();
   		}
   		else
   		{
   			onLoginUI();
   		}
   		
		if(KBEngineApp.app != null && KBEngineApp.app.serverVersion != "" 
			&& KBEngineApp.app.serverVersion != KBEngineApp.app.clientVersion)
		{
			labelColor = Color.red;
			labelMsg = "version not match(curr=" + KBEngineApp.app.clientVersion + ", srv=" + KBEngineApp.app.serverVersion + " )(版本不匹配)";
		}
		else if(KBEngineApp.app != null && KBEngineApp.app.serverScriptVersion != "" 
			&& KBEngineApp.app.serverScriptVersion != KBEngineApp.app.clientScriptVersion)
		{
			labelColor = Color.red;
			labelMsg = "scriptVersion not match(curr=" + KBEngineApp.app.clientScriptVersion + ", srv=" + KBEngineApp.app.serverScriptVersion + " )(脚本版本不匹配)";
		}
		
		GUI.contentColor = labelColor;
		GUI.Label(new Rect((Screen.width / 2) - 100, 40, 400, 100), labelMsg);

		GUI.Label(new Rect(0, 5, 400, 100), "client version: " + KBEngine.KBEngineApp.app.clientVersion);
		GUI.Label(new Rect(0, 20, 400, 100), "client script version: " + KBEngine.KBEngineApp.app.clientScriptVersion);
		GUI.Label(new Rect(0, 35, 400, 100), "server version: " + KBEngine.KBEngineApp.app.serverVersion);
		GUI.Label(new Rect(0, 50, 400, 100), "server script version: " + KBEngine.KBEngineApp.app.serverScriptVersion);
	}  
	
	public void err(string s)
	{
		labelColor = Color.red;
		labelMsg = s;
	}
	
	public void info(string s)
	{
		labelColor = Color.green;
		labelMsg = s;
	}
	
	public void login()
	{
		info("connect to server...(连接到服务端...)");
		KBEngine.Event.fireIn("login", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
	}
	
	public void createAccount()
	{
		info("connect to server...(连接到服务端...)");
		
		KBEngine.Event.fireIn("createAccount", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
	}
	

	

	

	

	


	public void onKicked(UInt16 failedcode)
	{
		err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(failedcode));
		Application.LoadLevel("login");
		ui_state = 0;
	}




	


	

}
