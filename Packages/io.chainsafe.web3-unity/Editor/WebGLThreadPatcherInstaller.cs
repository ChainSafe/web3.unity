using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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

    [MenuItem("ChainSafe SDK/Install WebGLThreadingPatcher")]
    public static void TryInstallThreadPatcher()
    {
        Manifest manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(ManifestPath));

        // check if ThreadPatcher is already installed.
        if (manifest.Dependencies.ContainsKey(AsyncToolsPackageName))
        {
            return;
        }

        if (EditorUtility.DisplayDialog("Web3.Unity", "For Web3.Unity to fully work on a WebGL build you need to install Async Utilities, this will make sure async operations can run to completion.\nInstall Async Utilities?", "Yes", "No"))
        {
            // Add the package as a dependency.
            manifest.Dependencies.Add(AsyncToolsPackageName, AsyncToolPackageLink);

            File.WriteAllText(ManifestPath, JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }
    }

    private class Manifest
    {
        [JsonProperty("dependencies")]
        public Dictionary<string, string> Dependencies { get; private set; }

        [JsonProperty("enableLockFile")]
        public bool EnableLockFile { get; private set; } = true;
        
        [JsonProperty("resolutionStrategy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ResolutionStrategy { get; private set; }
        
        [JsonProperty("testables", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Testables { get; private set; }
        
        [JsonProperty("scopedRegistries", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ScopedRegistry[] ScopedRegistries { get; private set; }
    }

    public struct ScopedRegistry
    {
        [JsonProperty("name")] public string Name { get; private set; }

        [JsonProperty("url")] public string Url { get; private set; }

        [JsonProperty("scopes")] public string[] Scopes { get; private set; }
    }
}