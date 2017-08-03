using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void VoidTouchDelegate(PointerEventData eventData);
public delegate bool BoolTouchDelegate(PointerEventData eventData);

// 消息传递的顺序按照优先级, 默认优先级为0, 优先级属性相同则按照添加的顺序
// isSwallow:如果是true, 并且OnPointerDown返回true, 则消息不会向下传递
// priority: 优先级
class TouchListener
{
    public object obj = null;
    public bool isSwallow = true;
    public TouchPriority priority = 0;
    public int order = 0;

    public VoidTouchDelegate onBeginDragDelegate = null;
    public VoidTouchDelegate onDragDelegate = null;
    public VoidTouchDelegate onEndDragDelegate = null;
    public BoolTouchDelegate onPointerDownDelegate = null;
    public VoidTouchDelegate onPointerUpDelegate = null;
}

public class TouchManager
{
    static List<TouchListener> listenerList = new List<TouchListener>();
    static TouchListener targetListener = null;

    public static void addListener(object obj, bool isSwallow = true, TouchPriority priority = 0)
    {
        removeListener(obj);

        TouchListener listener = new TouchListener();
        listener.obj = obj;
        listener.isSwallow = isSwallow;
        listener.priority = priority;

        MethodInfo method1 = obj.GetType().GetMethod("OnBeginDrag");
        if (method1 != null)
        {
            // 如果被反射获取的方法签名不匹配, 会报错
            listener.onBeginDragDelegate = (VoidTouchDelegate)Delegate.CreateDelegate(typeof(VoidTouchDelegate), obj, method1);
        }

        MethodInfo method2 = obj.GetType().GetMethod("OnDrag");
        if (method2 != null)
        {
            listener.onDragDelegate = (VoidTouchDelegate)Delegate.CreateDelegate(typeof(VoidTouchDelegate), obj, method2);
        }

        MethodInfo method3 = obj.GetType().GetMethod("OnEndDrag");
        if (method3 != null)
        {
            listener.onEndDragDelegate = (VoidTouchDelegate)Delegate.CreateDelegate(typeof(VoidTouchDelegate), obj, method3);
        }

        MethodInfo method4 = obj.GetType().GetMethod("OnPointerDown");
        if (method4 != null)
        {
            listener.onPointerDownDelegate = (BoolTouchDelegate)Delegate.CreateDelegate(typeof(BoolTouchDelegate), obj, method4);
        }

        MethodInfo method5 = obj.GetType().GetMethod("OnPointerUp");
        if (method5 != null)
        {
            listener.onPointerUpDelegate = (VoidTouchDelegate)Delegate.CreateDelegate(typeof(VoidTouchDelegate), obj, method5);
        }

        Debug.Assert(listener.onDragDelegate != null, "obj must has OnDrag.");
        Debug.Assert(listener.onPointerDownDelegate != null, "obj must has OnPointerDown.");
        Debug.Assert(listener.onPointerUpDelegate != null, "obj must has OnPointerUp.");

        // 在头部添加
        listenerList.Insert(0, listener);
        for (int i = 0; i < listenerList.Count; i++)
        {
            listenerList[i].order = i;
        }
        listenerList.Sort(SortListCompare);
        for (int i = 0; i < listenerList.Count; i++)
        {
            listenerList[i].order = i;
        }
    }

    public static bool removeListener(object obj)
    {
        foreach (TouchListener listener in listenerList)
        {
            if (listener.obj == obj)
            {
                listenerList.Remove(listener);
                return true;
            }
        }
        return false;
    }

    private static int SortListCompare(TouchListener obj1, TouchListener obj2)
    {
        int res = 0;

        // 降序, 优先级高的排在前面
        if (obj1.priority > obj2.priority)
        {
            res = -1;
        }
        else if (obj1.priority < obj2.priority)
        {
            res = 1;
        }
        else
        {
            // 升序, 后添加的排在前面
            if (obj1.order < obj2.order)
            {
                res = -1;
            }
            else if (obj1.order > obj2.order)
            {
                res = 1;
            }
        }
        return res;
    }

    public static void OnBeginDrag(PointerEventData eventData)
    {
        // TODO: 如果某个被委托的方法已销毁，会怎样?
        //foreach (TouchListener listener in listenerList)
        //{
        //    if (listener.onBeginDragDelegate!=null)
        //    {
        //        listener.onBeginDragDelegate(eventData);
        //    }
        //}
    }

    public static void OnDrag(PointerEventData eventData)
    {
        if (targetListener == null)
        {
            foreach (TouchListener listener in listenerList)
            {
                listener.onDragDelegate(eventData);
            }
        }
        else
        {
            targetListener.onDragDelegate(eventData);
        }
    }

    public static void OnEndDrag(PointerEventData eventData)
    {
        //foreach (TouchListener listener in listenerList)
        //{
        //    if (listener.onEndDragDelegate!=null)
        //    {
        //    listener.onEndDragDelegate(eventData);
        //    }
        //}
    }

    public static void OnPointerDown(PointerEventData eventData)
    {
        targetListener = null;

        foreach (TouchListener listener in listenerList)
        {
            if (targetListener != null)
                return;

            bool isTarget = listener.onPointerDownDelegate(eventData);

            if (listener.isSwallow)
            {
                if (isTarget)
                {
                    targetListener = listener;
                    return;
                }
            }
        }
    }

    public static void OnPointerUp(PointerEventData eventData)
    {
        if (targetListener == null)
        {
            foreach (TouchListener listener in listenerList)
            {
                listener.onPointerUpDelegate(eventData);
            }
        }
        else
        {
            targetListener.onPointerUpDelegate(eventData);
        }

        targetListener = null;
    }
}
