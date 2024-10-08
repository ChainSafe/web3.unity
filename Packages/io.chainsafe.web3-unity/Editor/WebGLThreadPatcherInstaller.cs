using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class WebGLThreadPatcherInstaller
{
    private const string AsyncToolsInstalled = "AreAsyncUtilitiesInstalled";

    private const string ManifestPath = "Packages/manifest.json";

    private const string AsyncToolsPackageName = "com.utilities.async";

    private const string AsyncToolPackageLink = "https://github.com/RageAgainstThePixel/com.utilities.async.git#upm";

    private const string WebGLThreadingPatcherLink = "https://github.com/VolodymyrBS/WebGLThreadingPatcher.git";
    private const string WebGLThreadingPatcherName = "com.tools.webglthreadingpatcher";


    static WebGLThreadPatcherInstaller()
    {
#if UNITY_WEBGL
        if (SessionState.GetBool(AsyncToolsInstalled, false))
        {
            return;
        }

        TryInstallThreadPatcher();

        SessionState.SetBool(AsyncToolsInstalled, true);
#else
        // reset.
        SessionState.SetBool(AsyncToolsInstalled, false);
#endif
    }

    [MenuItem("ChainSafe SDK/Install WebGLThreadingPatcher", priority = 0)]
    public static void TryInstallThreadPatcher()
    {
        string json = File.ReadAllText(ManifestPath);

        Manifest manifest = JsonConvert.DeserializeObject<Manifest>(json);

        // check if ThreadPatcher & AsyncUtilities are already installed.
        if (manifest.Dependencies.ContainsKey(AsyncToolsPackageName) && manifest.Dependencies.ContainsKey(WebGLThreadingPatcherName))
        {
            Debug.Log("Both WebGL Threading Patcher and Async Tools are already installed");
            return;
        }

        if (EditorUtility.DisplayDialog("Web3.Unity",
                "For Web3.Unity to fully work on a WebGL build you need to install Async Utilities & WebGL Threading Patcher, this will make sure async operations can run to completion.\nInstall Async Utilities & WebGL Threading Patcher?",
                "Yes", "No"))
        {
            try
            {
                var parsed = JObject.Parse(json);

                parsed.Merge(JObject.Parse(JsonConvert.SerializeObject(new Manifest(new Dictionary<string, string>()
                {
                    { AsyncToolsPackageName, AsyncToolPackageLink },
                    { WebGLThreadingPatcherName, WebGLThreadingPatcherLink}
                }))));

                File.WriteAllText(ManifestPath, parsed.ToString(Formatting.Indented));
                UnityEditor.PackageManager.Client.Resolve();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error adding {AsyncToolsPackageName} package. {e}");

                throw;
            }
        }
    }

    private class Manifest
    {
        [JsonProperty("dependencies", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, string> Dependencies { get; private set; }

        public Manifest(Dictionary<string, string> dependencies)
        {
            Dependencies = dependencies;
        }
    }
}