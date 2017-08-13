using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInputListener
{
    bool OnInputEvent();
    void OnJoystickEvent(Vector2 deltaPos, float degrees);
}

public class InputManager
{
    static List<IInputListener> mListeners = new List<IInputListener>();

    public static void addListener(IInputListener obj)
    {
        removeListener(obj);

        mListeners.Add(obj);
    }

    public static void OnJoystickMoved(Vector2 deltaPos, float degrees)
    {
        foreach (IInputListener item in mListeners)
        {
            item.OnJoystickEvent(deltaPos, degrees);
        }
    }

    public static void removeListener(IInputListener obj)
    {
        foreach (IInputListener item in mListeners)
        {
            if (item==obj)
            {
                mListeners.Remove(item);
                return;
            }
        }
    }

    public static void Update()
    {
        foreach (IInputListener item in mListeners)
        {
            item.OnInputEvent();
        }
    }
}
