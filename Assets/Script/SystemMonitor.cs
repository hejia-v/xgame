using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class SystemMonitor : MonoBehaviour
{
    public bool showFps = true;

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    void Start()
    {
        if (!showFps)
        {
            Destroy(this);
        }

        //Application.targetFrameRate=60;
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }

    void Update()
    {
        if (!showFps)
            return;

        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }

    void OnGUI()
    {
        if (!showFps)
            return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = new Color(1, 1, 1);

        string text = "";
        text += "总内存：" + ByteToM(Profiler.GetTotalAllocatedMemoryLong()).ToString("F") + "M" + "\n";
        text += "堆内存：" + ByteToM(Profiler.GetMonoUsedSizeLong()).ToString("F") + "M" + "\n";
        text += "FPS: " + f_Fps.ToString("f2") + "\n";

        GUI.Label(new Rect(5, Screen.height - 75, 200, 200), text, style);
    }

    static float ByteToM(long byteCount)
    {
        return (float)(byteCount / (1024.0f * 1024.0f));
    }
}
