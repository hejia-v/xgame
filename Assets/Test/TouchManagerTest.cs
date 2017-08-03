using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class TMT1
{
    public int v = 0;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("v = " + v);
    }
}


public class TouchManagerTest
{

    public void Test()
    {
        TMT1 obj1 = new TMT1();
        obj1.v = 2;
        TMT1 obj2 = new TMT1();
        obj2.v = 5;

        TouchManager.addListener(obj1);
        TouchManager.addListener(obj2);
        TouchManager.OnBeginDrag(null);
    }

}
