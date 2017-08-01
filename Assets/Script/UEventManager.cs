using System.Collections.Generic;
using UnityEngine.EventSystems;


// TODO: 编写测试用例
public class UEventManager
{
    public delegate void VoidDelegate(PointerEventData eventData);
    public delegate bool BoolDelegate(PointerEventData eventData);

    // 消息传递的顺序按照优先级, 默认优先级为0, 优先级属性相同则按照添加的顺序
    // isSwallow:如果是true, 并且OnPointerDown返回true, 则消息不会向下传递
    // priority: 优先级
    public struct REItem<T>
    {
        public T funcDelegate;
        public object obj;
        public bool isSwallow;
        public int priority;
    };

    static object targetObj = null;
    static VoidDelegate targetUpDelegate = null;
    static VoidDelegate targetDragDelegate = null;

    static List<REItem<VoidDelegate>> OnBeginDragDelegateList = new List<REItem<VoidDelegate>>();
    static List<REItem<VoidDelegate>> OnDragDelegateList = new List<REItem<VoidDelegate>>();
    static List<REItem<VoidDelegate>> OnEndDragDelegateList = new List<REItem<VoidDelegate>>();
    static List<REItem<BoolDelegate>> OnPointerDownDelegateList = new List<REItem<BoolDelegate>>();
    static List<REItem<VoidDelegate>> OnPointerUpDelegateList = new List<REItem<VoidDelegate>>();

    public enum EventType
    {
        OnBeginDrag,
        OnDrag,
        OnEndDrag,
        OnPointerDown,
        OnPointerUp,
    }

    private static int SortListCompare<T>(REItem<T> obj1, REItem<T> obj2)
    {
        int res = 0;

        // 降序
        if (obj1.priority > obj2.priority)
        {
            res = -1;
        }
        else if (obj1.priority < obj2.priority)
        {
            res = 1;
        }
        return res;
    }

    public static void register(EventType eventType, VoidDelegate funcDelegate, object obj, bool isSwallow = true, int priority = 0)
    {
        switch (eventType)
        {
            case EventType.OnBeginDrag:
                addObserverItemToList(funcDelegate, obj, isSwallow, priority, OnBeginDragDelegateList);
                break;
            case EventType.OnDrag:
                addObserverItemToList(funcDelegate, obj, isSwallow, priority, OnDragDelegateList);
                break;
            case EventType.OnEndDrag:
                addObserverItemToList(funcDelegate, obj, isSwallow, priority, OnEndDragDelegateList);
                break;
            case EventType.OnPointerDown:
                break;
            case EventType.OnPointerUp:
                addObserverItemToList(funcDelegate, obj, isSwallow, priority, OnPointerUpDelegateList);
                break;
            default:
                break;
        }
    }

    public static void register(EventType eventType, BoolDelegate funcDelegate, object obj, bool isSwallow = true, int priority = 0)
    {
        switch (eventType)
        {
            case EventType.OnBeginDrag:
                break;
            case EventType.OnDrag:
                break;
            case EventType.OnEndDrag:
                break;
            case EventType.OnPointerDown:
                addObserverItemToList(funcDelegate, obj, isSwallow, priority, OnPointerDownDelegateList);
                break;
            case EventType.OnPointerUp:
                break;
            default:
                break;
        }
    }

    private static void addObserverItemToList<T>(T funcDelegate, object obj, bool isSwallow, int priority, List<REItem<T>> list)
    {
        REItem<T> item = new REItem<T>();
        item.funcDelegate = funcDelegate;
        item.obj = obj;
        item.isSwallow = isSwallow;
        item.priority = priority;

        list.Add(item);
        list.Sort(SortListCompare);
    }

    public static T GetDelegeteByObj<T>(object obj, List<REItem<T>> list)
    {
        foreach (REItem<T> item in list)
        {
            if (targetObj == item.obj)
            {
                return item.funcDelegate;
            }
        }
        return default(T);
    }

    public static void OnBeginDrag(PointerEventData eventData)
    {
        // TODO: 如果某个被代理的方法已销毁，会怎样?
        foreach (REItem<VoidDelegate> item in OnBeginDragDelegateList)
        {
            item.funcDelegate(eventData);
        }
    }

    public static void OnDrag(PointerEventData eventData)
    {
        if (targetDragDelegate==null)
        {
            foreach (REItem<VoidDelegate> item in OnDragDelegateList)
            {
                item.funcDelegate(eventData);
            }
        }
        else
        {
            targetDragDelegate(eventData);
        }
    }

    public static void OnEndDrag(PointerEventData eventData)
    {
        foreach (REItem<VoidDelegate> item in OnEndDragDelegateList)
        {
            item.funcDelegate(eventData);
        }
    }

    public static void OnPointerDown(PointerEventData eventData)
    {
        targetObj = null;
        targetDragDelegate = null;
        targetUpDelegate = null;

        foreach (REItem<BoolDelegate> item in OnPointerDownDelegateList)
        {
            if (targetObj != null)
                return;

            bool isTarget = item.funcDelegate(eventData);

            if (item.isSwallow)
            {
                if (isTarget)
                {
                    targetObj = item.obj;
                    targetDragDelegate = GetDelegeteByObj(item.obj, OnDragDelegateList);
                    targetUpDelegate= GetDelegeteByObj(item.obj, OnPointerUpDelegateList);
                    return;
                }
            }
        }
    }

    public static void OnPointerUp(PointerEventData eventData)
    {
        if (targetUpDelegate==null)
        {
            foreach (REItem<VoidDelegate> item in OnPointerUpDelegateList)
            {
                item.funcDelegate(eventData);
            }
        }
        else
        {
            targetUpDelegate(eventData);
        }

        targetObj = null;
        targetDragDelegate = null;
        targetUpDelegate = null;
    }
}
