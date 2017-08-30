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
			//onSelAvatarUI();
   		}
   		else if(ui_state == 2)
   		{
			onWorldUI();
   		}
   		else
   		{
   			//onLoginUI();
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




	


	

}
