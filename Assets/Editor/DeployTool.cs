using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class DeployTool
{
    static public void Build()
    {
        string outputPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + "_Out/";
        BuildOptions buildOption = BuildOptions.AcceptExternalModificationsToPlayer;
        string[] ss = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < ss.Length; i++)
        {
            if (ss[i] == "-outputPath")
            {
                outputPath = ss[i + 1];
            }
        }
        outputPath = outputPath.Replace('\\', '/');
        Debug.Log(outputPath + "======================");
        string ret = BuildPipeline.BuildPlayer(GetBuildScenes(), outputPath, BuildTarget.Android, buildOption);
        if (ret.Length > 0)
            throw new Exception("BuildPlayer failure: " + ret);
    }

    static string[] GetBuildScenes()
    {
        List<string> pathList = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                pathList.Add(scene.path);
            }
        }
        return pathList.ToArray();
    }
}
