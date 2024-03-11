using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class SampleProjectEditorUtil
{

    private const string ChainsafeGamingPath = "../../ChainSafe.Gaming.sln";
    private const string _publishToUnityPackagePath = "../../scripts";

    private static string _publishToUnityPackageName = $"publish-to-unity-package{_unityPackageExtension}";

    private static string _unityPackageExtension = (Application.platform == RuntimePlatform.WindowsEditor ? ".bat" : ".sh");


    /// <summary>
    /// This is only needed in the actual UnitySampleProject. Doesn't make sense for it to be in the samples.
    /// That's why it's only in the UnitySampleProject/Scripts folder.
    /// </summary>
    [MenuItem("Assets/Open Chainsafe.Gaming Solution")]
    private static void OpenQuantumProject()
    {
        var path = System.IO.Path.GetFullPath(ChainsafeGamingPath);

        if (!System.IO.File.Exists(path))
        {
            EditorUtility.DisplayDialog("Open Quantum Project", "Solution file '" + path + "' not found. Check QuantumProjectPath in your QuantumEditorSettings.", "Ok");
        }

        var uri = new Uri(path);
        Application.OpenURL(uri.AbsoluteUri);
    }

    [MenuItem("Assets/Build & Publish Web3.Unity to Unity")]
    private static void PublishSolutionsToUnityPackage()
    {
        var path = System.IO.Path.GetFullPath(_publishToUnityPackagePath);

        ProcessStartInfo startInfo = new ProcessStartInfo();

#if UNITY_EDITOR_WIN
        // Windows configuration
        startInfo.FileName = "cmd.exe";
        // Including drive switch here
        string driveLetter = path.Substring(0, 2); // Extracts 'D:' from the path
        startInfo.Arguments = $"/C {driveLetter} && cd {path} && .\\{_publishToUnityPackageName}";
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
        // macOS and Linux configuration
        startInfo.FileName = "/bin/bash";
        startInfo.Arguments = $"-c \"cd {path} && ./{_publishToUnityPackageName}\"";
#endif

        // Configure process
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        using (Process process = Process.Start(startInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Debug.Log(result);
            }

            using (StreamReader reader = process.StandardError)
            {
                string error = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError(error);
                }
            }
        }
        AssetDatabase.Refresh();
    }
}
