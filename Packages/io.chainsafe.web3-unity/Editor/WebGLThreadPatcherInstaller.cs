using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class WebGLThreadPatcherInstaller
{
    private const string WebGLThreadPatchedInstalled = "IsWebGLThreadPatcherInstalled";

    private const string ManifestPath = "Packages/manifest.json";

    private const string WebGLThreadPatcherPackageName = "com.tools.webglthreadingpatcher";

    private const string WebGLThreadPatcherPackageLink = "https://github.com/VolodymyrBS/WebGLThreadingPatcher.git";

    static WebGLThreadPatcherInstaller()
    {
#if UNITY_WEBGL
        if (SessionState.GetBool(WebGLThreadPatchedInstalled, false))
        {
            return;
        }

        TryInstallThreadPatcher();

        SessionState.SetBool(WebGLThreadPatchedInstalled, true);
#else
        // reset.
        SessionState.SetBool(WebGLThreadPatchedInstalled, false);
#endif
    }

    [MenuItem("ChainSafe SDK/Install WebGLThreadingPatcher")]
    public static void TryInstallThreadPatcher()
    {
        Manifest manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(ManifestPath));

        // check if ThreadPatcher is already installed.
        if (manifest.Dependencies.ContainsKey(WebGLThreadPatcherPackageName))
        {
            return;
        }

        if (EditorUtility.DisplayDialog("Web3.Unity", "For Web3.Unity to fully work on a WebGL build you need to install a WebGLThreadingPatcher, this will make sure async operations can run to completion.\nInstall WebGLThreadingPatcher?", "Yes", "No"))
        {
            // Add the package as a dependency.
            manifest.Dependencies.Add(WebGLThreadPatcherPackageName, WebGLThreadPatcherPackageLink);

            File.WriteAllText(ManifestPath, JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }
    }

    private struct Manifest
    {
        [JsonProperty("dependencies")]
        public Dictionary<string, string> Dependencies { get; private set; }
    }
}