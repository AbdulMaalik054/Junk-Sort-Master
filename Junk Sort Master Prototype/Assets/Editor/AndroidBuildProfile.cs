using UnityEditor;
using UnityEngine;

public static class AndroidBuildProfile
{
    [MenuItem("Build/Android/One-Click Dev Build")]
    public static void BuildAndRunAndroid()
    {
        // Ensure Android platform
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Android,
                BuildTarget.Android
            );
        }

        // Apply build settings
        EditorUserBuildSettings.development = true;
        EditorUserBuildSettings.connectProfiler = false;
        EditorUserBuildSettings.allowDebugging = true;
        EditorUserBuildSettings.buildAppBundle = false;

        PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetScriptingBackend(
            UnityEditor.Build.NamedBuildTarget.Android,
            ScriptingImplementation.IL2CPP
        );

        PlayerSettings.Android.targetArchitectures =
            AndroidArchitecture.ARM64;

        // Output path
        string buildPath = "Builds/Android/DevBuild.apk";

        // Build
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.Development | BuildOptions.AutoRunPlayer
        };

        BuildPipeline.BuildPlayer(options);

        Debug.Log("Android Dev Build completed.");
    }

    private static string[] GetEnabledScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        var enabledScenes = new System.Collections.Generic.List<string>();

        foreach (var scene in scenes)
        {
            if (scene.enabled)
                enabledScenes.Add(scene.path);
        }

        return enabledScenes.ToArray();
    }
}
