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

        // check if ThreadPatcher is already installed.
        if (manifest.Dependencies.ContainsKey(AsyncToolsPackageName))
        {
            return;
        }

        if (EditorUtility.DisplayDialog("Web3.Unity", "For Web3.Unity to fully work on a WebGL build you need to install Async Utilities, this will make sure async operations can run to completion.\nInstall Async Utilities?", "Yes", "No"))
        {
            var parsed = JObject.Parse(json);

            parsed.Merge(JObject.Parse(JsonConvert.SerializeObject(new Manifest(new Dictionary<string, string>()
            {
                { AsyncToolsPackageName, AsyncToolPackageLink }
            }))));

            File.WriteAllText(ManifestPath, parsed.ToString(Formatting.Indented));
        }
    }

    private class Manifest
    {
        [JsonProperty("dependencies")]
        public Dictionary<string, string> Dependencies { get; private set; }

        public Manifest(Dictionary<string, string> dependencies)
        {
            Dependencies = dependencies;
        }
    }
}