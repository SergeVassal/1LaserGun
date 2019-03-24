using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class InitializeDefines
{ 
    private static BuildTargetGroup[] buildTargetPlatforms = new BuildTargetGroup[]
    {
        BuildTargetGroup.Standalone,
        BuildTargetGroup.Android,
        BuildTargetGroup.iOS
    };

    private static BuildTargetGroup[] mobileBuildTargetPlatforms = new BuildTargetGroup[]
    {
        BuildTargetGroup.Android,
        BuildTargetGroup.iOS
    };
        
    
    static InitializeDefines()
    {
        AddDefineToAllPlatforms("CROSS_PLATFORM_INPUT");
        AddDefineOnlyToMobilePlatforms("MOBILE_INPUT");        
    }    

    private static void AddDefineToAllPlatforms(string defineName)
    {
        foreach(BuildTargetGroup group in buildTargetPlatforms)
        {
            List<string> defines = GetDefinesList(group);

            if (defines.Contains(defineName))
            {
                return;
            }
            else
            {
                defines.Add(defineName);
            }

            SetScriptingDefineSymbols(group, defines);                   
        }
    }

    private static void RemoveDefineFromAllPlatforms(string defineName)
    {
        foreach (BuildTargetGroup group in buildTargetPlatforms)
        {
            List<string> defines = GetDefinesList(group);

            if (!defines.Contains(defineName))
            {
                return;
            }
            while (defines.Contains(defineName))
            {
                defines.Remove(defineName);
            }

            SetScriptingDefineSymbols(group, defines);
        }
    }

    private static void AddDefineOnlyToMobilePlatforms(string defineName)
    {
        foreach (BuildTargetGroup group in mobileBuildTargetPlatforms)
        {
            List<string> defines = GetDefinesList(group);

            if (defines.Contains(defineName))
            {
                return;
            }
            else
            {
                defines.Add(defineName);
            }

            SetScriptingDefineSymbols(group, defines);
        }

    }

    private static void RemoveDefineOnlyFromMobilePlatforms(string defineName)
    {
        foreach (BuildTargetGroup group in mobileBuildTargetPlatforms)
        {
            List<string> defines = GetDefinesList(group);

            if (!defines.Contains(defineName))
            {
                return;
            }
            while (defines.Contains(defineName))
            {
                defines.Remove(defineName);
            }

            SetScriptingDefineSymbols(group, defines);
        }
    }

    private static List<string> GetDefinesList(BuildTargetGroup targetGroup)
    {
        return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Split(';'));
    }

    private static void SetScriptingDefineSymbols(BuildTargetGroup group,List<string> definesList)
    {
        string definesString = string.Join(";", definesList.ToArray());
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
    }

    [MenuItem("Mobile Input/Enable")]
    private static void Enable()
    {
        AddDefineOnlyToMobilePlatforms("MOBILE_INPUT");

        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
            case BuildTarget.iOS:
                EditorUtility.DisplayDialog("Mobile Input",
                                            "You have enabled Mobile Input. You'll need to use the Unity Remote app on a connected device to control your game in the Editor.",
                                            "OK");
                break;

            default:
                EditorUtility.DisplayDialog("Mobile Input",
                                            "You have enabled Mobile Input, but you have a non-mobile build target selected in your build settings. The mobile control rigs won't be active or visible on-screen until you switch the build target to a mobile platform.",
                                            "OK");
                break;
        }
    }

    [MenuItem("Mobile Input/Enable",true)]
    private static bool EnableValidate()
    {
        List<string> defines = GetDefinesList(mobileBuildTargetPlatforms[0]);
        return !defines.Contains("MOBILE_INPUT");
    }

    [MenuItem("Mobile Input/Disable")]
    private static void Disable()
    {
        RemoveDefineOnlyFromMobilePlatforms("MOBILE_INPUT");

        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
            case BuildTarget.iOS:
                EditorUtility.DisplayDialog("Mobile Input",
                                            "You have disabled Mobile Input. Mobile control rigs won't be visible, and the Cross Platform Input functions will always return standalone controls.",
                                            "OK");
                break;
        }
    }

    [MenuItem("Mobile Input/Disable", true)]
    private static bool DisableValidate()
    {
        List<string> defines = GetDefinesList(mobileBuildTargetPlatforms[0]);
        return defines.Contains("MOBILE_INPUT");
    }

}
