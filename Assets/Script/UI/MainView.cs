using MoleMole;
using UnityEngine;
using UnityEngine.UI;

public class MainView : BaseView
{
    private Joystick mJoystick;

    void Start()
    {
        //Text msgBox = transform.Find("MessageBox").GetComponent<Text>();
        Image bgImg = transform.Find("JoystickBg").GetComponent<Image>();
        mJoystick = new Joystick(bgImg);
        
        TouchManager.addListener(mJoystick, true, TouchPriority.joystick);

        for (int i = 0; i < 3; i++)
        {
            int btnIdx = i;
            string btnName = string.Format("btn_skill_{0:G}", i);
            Button skillBtn = transform.Find(btnName).GetComponent<Button>();
            skillBtn.onClick.AddListener(delegate () { this.OnSkillBtnClick(btnIdx); });
        }
    }

    void OnDestroy()
    {
        TouchManager.removeListener(mJoystick);
    }

    public override void OnEnter(BaseContext context)
    {
        base.OnEnter(context);
    }

    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
    }

    public override void OnPause(BaseContext context)
    {
        base.OnPause(context);
    }

    public override void OnResume(BaseContext context)
    {
        base.OnResume(context);
    }

    public void OnSettingBtnClick()
    {
    }

    public void OnSkillBtnClick(int btnIdx)
    {
        Debug.Log("click skill " + btnIdx);
    }
}
