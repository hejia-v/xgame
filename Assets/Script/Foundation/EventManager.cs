using System;
using System.Collections.Generic;
using System.Reflection;

public class EventData
{

}

public delegate bool EventDelegate(GEventType eventType, EventData eventData);

class Eventlistener
{
    public object obj = null;
    public GEventType eventType = GEventType.None;
    public EventDelegate funcDelegate = null;
}

public class EventManager
{
    static Dictionary<GEventType, List<Eventlistener>> mListeners = new Dictionary<GEventType, List<Eventlistener>>();

    public static void addListener(object obj, GEventType eventType)
    {
        removeListener(obj, eventType);

        Eventlistener listener = new Eventlistener();
        listener.obj = obj;
        listener.eventType = eventType;

        MethodInfo method1 = obj.GetType().GetMethod("OnEvent");
        if (method1 != null)
        {
            // TODO: 如果被反射获取的方法签名不匹配, 会报错. 需改进成不报错，但输出log
            listener.funcDelegate = (EventDelegate)Delegate.CreateDelegate(typeof(EventDelegate), obj, method1);
            if (listener.funcDelegate == null)
                return;
        }

        List<Eventlistener> lst = null;

        if (!mListeners.TryGetValue(eventType, out lst))
        {
            lst = new List<Eventlistener>();
            lst.Add(listener);
            mListeners.Add(eventType, lst);
            return;
        }

        lst.Add(listener);
    }

    public static bool removeListener(object obj, GEventType eventType)
    {
        List<Eventlistener> lst = null;

        if (!mListeners.TryGetValue(eventType, out lst))
        {
            return false;
        }

        for (int i = 0; i < lst.Count; i++)
        {
            if (obj == lst[i].obj && lst[i].eventType == eventType)
            {
                lst.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public static void sendEvent(GEventType eventType, EventData eventData)
    {
        List<Eventlistener> lst = null;

        if (!mListeners.TryGetValue(eventType, out lst))
        {
            return;
        }

        foreach (Eventlistener listener in lst)
        {
            listener.funcDelegate(eventType, eventData);
        }
    }
}
