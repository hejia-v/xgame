using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KBEventData
{

}

public delegate bool KBEventDelegate(KBEventData eventData);

/*
KBEngine插件层与Unity3D表现层通过事件来交互
*/
public class KBEvent
{
    struct KBEventlistener
    {
        public object obj;
        public NET eventType;
        public KBEventDelegate funcDelegate;
    }

    struct EventObj
    {
        public KBEventlistener listener;
        public KBEventData eventData;
    };

    static Dictionary<NET, List<KBEventlistener>> events_out = new Dictionary<NET, List<KBEventlistener>>();

    static LinkedList<EventObj> firedEvents_out = new LinkedList<EventObj>();
    static LinkedList<EventObj> doingEvents_out = new LinkedList<EventObj>();

    static Dictionary<NET, List<KBEventlistener>> events_in = new Dictionary<NET, List<KBEventlistener>>();

    static LinkedList<EventObj> firedEvents_in = new LinkedList<EventObj>();
    static LinkedList<EventObj> doingEvents_in = new LinkedList<EventObj>();

    static bool _isPauseOut = false;

    public KBEvent()
    {
    }

    public static void clear()
    {
        events_out.Clear();
        events_in.Clear();
        clearFiredEvents();
    }

    public static void clearFiredEvents()
    {
        monitor_Enter(events_out);
        firedEvents_out.Clear();
        monitor_Exit(events_out);

        doingEvents_out.Clear();

        monitor_Enter(events_in);
        firedEvents_in.Clear();
        monitor_Exit(events_in);

        doingEvents_in.Clear();

        _isPauseOut = false;
    }

    public static void pause()
    {
        _isPauseOut = true;
    }

    public static void resume()
    {
        _isPauseOut = false;
    }

    public static bool isPause()
    {
        return _isPauseOut;
    }

    public static void monitor_Enter(object obj)
    {
        if (KBEngineApp.app == null)
            return;

        Monitor.Enter(obj);
    }

    public static void monitor_Exit(object obj)
    {
        if (KBEngineApp.app == null)
            return;

        Monitor.Exit(obj);
    }

    /*
    注册监听由kbe插件抛出的事件。(out = kbe->render)
    通常由渲染表现层来注册, 例如：监听角色血量属性的变化， 如果UI层注册这个事件，
    事件触发后就可以根据事件所附带的当前血量值来改变角色头顶的血条值。
    */
    public static bool registerOut(NET eventType, object obj, KBEventDelegate funcDelegate)
    {
        return register(events_out, eventType, obj, funcDelegate);
    }

    /*
    注册监听由渲染表现层抛出的事件(in = render->kbe)
    通常由kbe插件层来注册， 例如：UI层点击登录， 此时需要触发一个事件给kbe插件层进行与服务端交互的处理。
    */
    public static bool registerIn(NET eventType, object obj, KBEventDelegate funcDelegate)
    {
        return register(events_in, eventType, obj, funcDelegate);
    }

    private static bool register(Dictionary<NET, List<KBEventlistener>> events, NET eventType, object obj, KBEventDelegate funcDelegate)
    {
        deregister(events, eventType, obj, funcDelegate);
        List<KBEventlistener> lst = null;

        KBEventlistener listener = new KBEventlistener();
        listener.obj = obj;
        listener.eventType = eventType;
        listener.funcDelegate = funcDelegate;
        if (listener.funcDelegate == null)
        {
            Dbg.ERROR_MSG("Event::register: " + obj + "not found method[" + funcDelegate + "]");
            return false;
        }

        monitor_Enter(events);
        if (!events.TryGetValue(eventType, out lst))
        {
            lst = new List<KBEventlistener>();
            lst.Add(listener);
            events.Add(eventType, lst);
            monitor_Exit(events);
            return true;
        }

        lst.Add(listener);
        monitor_Exit(events);
        return true;
    }

    public static bool deregisterOut(NET eventType, object obj, KBEventDelegate funcDelegate)
    {
        return deregister(events_out, eventType, obj, funcDelegate);
    }

    public static bool deregisterIn(NET eventType, object obj, KBEventDelegate funcDelegate)
    {
        return deregister(events_in, eventType, obj, funcDelegate);
    }

    private static bool deregister(Dictionary<NET, List<KBEventlistener>> events, NET eventType, object obj, KBEventDelegate funcDelegate)
    {
        monitor_Enter(events);
        List<KBEventlistener> lst = null;

        if (!events.TryGetValue(eventType, out lst))
        {
            monitor_Exit(events);
            return false;
        }

        for (int i = 0; i < lst.Count; i++)
        {
            if (obj == lst[i].obj && lst[i].funcDelegate == funcDelegate)
            {
                lst.RemoveAt(i);
                monitor_Exit(events);
                return true;
            }
        }

        monitor_Exit(events);
        return false;
    }

    public static bool deregisterOut(object obj)
    {
        return deregister(events_out, obj);
    }

    public static bool deregisterIn(object obj)
    {
        return deregister(events_in, obj);
    }

    private static bool deregister(Dictionary<NET, List<KBEventlistener>> events, object obj)
    {
        monitor_Enter(events);

        var iter = events.GetEnumerator();
        while (iter.MoveNext())
        {
            List<KBEventlistener> lst = iter.Current.Value;
            // 从后往前遍历，以避免中途删除的问题
            for (int i = lst.Count - 1; i >= 0; i--)
            {
                if (obj == lst[i].obj)
                {
                    lst.RemoveAt(i);
                }
            }
        }

        monitor_Exit(events);
        return true;
    }

    /*
    kbe插件触发事件(out = kbe->render)
    通常由渲染表现层来注册, 例如：监听角色血量属性的变化， 如果UI层注册这个事件，
    事件触发后就可以根据事件所附带的当前血量值来改变角色头顶的血条值。
    */
    public static void fireOut(NET eventType, KBEventData eventData)
    {
        fire_(events_out, firedEvents_out, eventType, eventData);
    }

    /*
    渲染表现层抛出事件(in = render->kbe)
    通常由kbe插件层来注册， 例如：UI层点击登录， 此时需要触发一个事件给kbe插件层进行与服务端交互的处理。
    */
    public static void fireIn(NET eventType, KBEventData eventData)
    {
        fire_(events_in, firedEvents_in, eventType, eventData);
    }

    /*
    触发kbe插件和渲染表现层都能够收到的事件
    */
    public static void fireAll(NET eventType, KBEventData eventData)
    {
        fire_(events_in, firedEvents_in, eventType, eventData);
        fire_(events_out, firedEvents_out, eventType, eventData);
    }

    private static void fire_(Dictionary<NET, List<KBEventlistener>> events, LinkedList<EventObj> firedEvents, NET eventType, KBEventData eventData)
    {
        monitor_Enter(events);
        List<KBEventlistener> lst = null;

        if (!events.TryGetValue(eventType, out lst))
        {
            if (events == events_in)
                Dbg.WARNING_MSG("Event::fireIn: event(" + eventType + ") not found!");
            else
                Dbg.WARNING_MSG("Event::fireOut: event(" + eventType + ") not found!");

            monitor_Exit(events);
            return;
        }

        for (int i = 0; i < lst.Count; i++)
        {
            EventObj eobj = new EventObj();
            eobj.listener = lst[i];
            eobj.eventData = eventData;
            firedEvents.AddLast(eobj);
        }

        monitor_Exit(events);
    }

    public static void processOutEvents()
    {
        monitor_Enter(events_out);

        if (firedEvents_out.Count > 0)
        {
            var iter = firedEvents_out.GetEnumerator();
            while (iter.MoveNext())
            {
                doingEvents_out.AddLast(iter.Current);
            }

            firedEvents_out.Clear();
        }

        monitor_Exit(events_out);

        while (doingEvents_out.Count > 0 && !_isPauseOut)
        {

            EventObj eobj = doingEvents_out.First.Value;

            try
            {
                eobj.listener.funcDelegate(eobj.eventData);
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG("Event::processOutEvents: event=" + eobj.listener.funcDelegate + "\n" + e.ToString());
            }

            if (doingEvents_out.Count > 0)
                doingEvents_out.RemoveFirst();
        }
    }

    public static void processInEvents()
    {
        monitor_Enter(events_in);

        if (firedEvents_in.Count > 0)
        {
            var iter = firedEvents_in.GetEnumerator();
            while (iter.MoveNext())
            {
                doingEvents_in.AddLast(iter.Current);
            }

            firedEvents_in.Clear();
        }

        monitor_Exit(events_in);

        while (doingEvents_in.Count > 0)
        {

            EventObj eobj = doingEvents_in.First.Value;

            try
            {
                eobj.listener.funcDelegate(eobj.eventData);
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG("Event::processInEvents: event=" + eobj.listener.funcDelegate + "\n" + e.ToString());
            }

            if (doingEvents_in.Count > 0)
                doingEvents_in.RemoveFirst();
        }
    }

}
