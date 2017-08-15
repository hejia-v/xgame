using MoleMole;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();
        Singleton<Localization>.Create();
        //UIManager uimgr = SingletonProvider<UIManager>.Instance;

        Singleton<ContextManager>.Instance.Push(new MainViewContext());
    }

    void Update()
    {

    }
}
