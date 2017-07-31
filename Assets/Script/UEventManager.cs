using System.Collections.Generic;
using UnityEngine.EventSystems;


// TODO: 编写测试用例
public class UEventManager
{
    public delegate void UEventDelegate(PointerEventData eventData);

    // 消息传递的顺序按照优先级, 默认优先级为0, 优先级属性相同则按照添加的顺序
    // isSwallow:如果是true, 并且OnPointerDown返回true, 则消息不会向下传递
    // priority: 优先级
    public struct REItem
    {
        public UEventDelegate funcDelegate;
        public object obj;
        public bool isSwallow;
        public int priority;
    };

    static object currObj = null;

    static List<REItem> OnBeginDragDelegateList = new List<REItem>();
    static List<REItem> OnDragDelegateList = new List<REItem>();
    static List<REItem> OnEndDragDelegateList = new List<REItem>();
    static List<REItem> OnPointerDownDelegateList = new List<REItem>();
    static List<REItem> OnPointerUpDelegateList = new List<REItem>();


    private static int SortListCompare(REItem obj1, REItem obj2)
    {
        int res = 0;

        if (obj1.priority > obj2.priority)
        {
            res = 1;
        }
        else if (obj1.priority < obj2.priority)
        {
            res = -1;
        }
        return res;
    }

    // 重载一下
    public static void register(string eventName, UEventDelegate f, object obj, bool isSwallow = false, int priority = 0)
    {
        REItem item = new REItem();
        item.funcDelegate = f;
        item.obj = obj;
        item.isSwallow = isSwallow;
        item.priority = priority;

        switch (eventName)
        {
            case "OnBeginDrag":
                OnBeginDragDelegateList.Add(item);
                OnBeginDragDelegateList.Sort(SortListCompare);
                break;
            case "OnDrag":
                OnDragDelegateList.Add(item);
                OnDragDelegateList.Sort(SortListCompare);
                break;
            case "OnEndDrag":
                OnEndDragDelegateList.Add(item);
                OnEndDragDelegateList.Sort(SortListCompare);
                break;
            case "OnPointerDown":
                OnPointerDownDelegateList.Add(item);
                OnPointerDownDelegateList.Sort(SortListCompare);
                break;
            case "OnPointerUp":
                OnPointerUpDelegateList.Add(item);
                OnPointerUpDelegateList.Sort(SortListCompare);
                break;
            default:
                break;
        }
    }

    public static void OnBeginDrag(PointerEventData eventData)
    {
        // TODO: 如果某个被代理的方法已销毁，会怎样?
        foreach (REItem item in OnBeginDragDelegateList)
        {
            item.funcDelegate(eventData);
        }
    }

    public static void OnDrag(PointerEventData eventData)
    {

        foreach (REItem item in OnDragDelegateList)
        {
            item.funcDelegate(eventData);
        }

    }

    public static void OnEndDrag(PointerEventData eventData)
    {

        foreach (REItem item in OnEndDragDelegateList)
        {
            item.funcDelegate(eventData);
        }

    }

    public static void OnPointerDown(PointerEventData eventData)
    {


        currObj = null;

        foreach (REItem item in OnPointerDownDelegateList)
        {
            if (currObj != null)
            {
                return;
            }
        }


    }

    public static void OnPointerUp(PointerEventData eventData)
    {

        foreach (REItem item in OnPointerUpDelegateList)
        {
            item.funcDelegate(eventData);
        }
    }
}
