#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

[InitializeOnLoad]
public static class ScopedRegistryAndDependencyInstaller
{
    private static readonly string RegistryName = "package.openupm.com";
    private static readonly string RegistryUrl = "https://package.openupm.com";
    private static readonly string[] RequiredScopes =
    {
        "com.reown.appkit.unity",
        "com.nethereum.unity",
        "com.reown.core",
        "com.reown.core.common",
        "com.reown.core.crypto",
        "com.reown.core.network",
        "com.reown.core.storage",
        "com.reown.sign",
        "com.reown.sign.nethereum",
        "com.reown.sign.nethereum.unity",
        "com.reown.sign.unity",
        "com.reown.unity.dependencies"
    };

    // The Git dependency to add
    private const string ChainsafeDependencyKey = "io.chainsafe.web3-unity";
    private const string ChainsafeDependencyUrl = "https://github.com/ChainSafe/web3.unity.git?path=/Packages/io.chainsafe.web3-unity#dev";

    static ScopedRegistryAndDependencyInstaller()
    {
        // Check if we've already installed the registry and dependencies
        if (EditorPrefs.GetBool("Installed scoped registries", false))
            return;

        try
        {
            string manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            string manifestJson = File.ReadAllText(manifestPath, Encoding.UTF8);
            JObject manifest = JObject.Parse(manifestJson);

            // Ensure scopedRegistries node exists
            if (manifest["scopedRegistries"] == null)
            {
                manifest["scopedRegistries"] = new JArray();
            }

            var scopedRegistries = (JArray)manifest["scopedRegistries"];

            // Find if our registry already exists
            var existingRegistry = scopedRegistries
                .OfType<JObject>()
                .FirstOrDefault(r =>
                    r["name"] != null &&
                    r["name"].Value<string>().Equals(RegistryName, System.StringComparison.OrdinalIgnoreCase));

            if (existingRegistry == null)
            {
                // Create a new registry entry
                existingRegistry = new JObject
                {
                    ["name"] = RegistryName,
                    ["url"] = RegistryUrl,
                    ["scopes"] = new JArray(RequiredScopes)
                };
                scopedRegistries.Add(existingRegistry);
            }
            else
            {
                // Registry exists, ensure scopes are present
                JArray scopesArray = (JArray)existingRegistry["scopes"];
                var currentScopes = scopesArray.Select(s => s.Value<string>()).ToList();

                foreach (var scope in RequiredScopes)
                {
                    if (!currentScopes.Contains(scope))
                    {
                        scopesArray.Add(scope);
                    }
                }
            }

            // Add the Chainsafe Git dependency
            if (manifest["dependencies"] == null)
            {
                manifest["dependencies"] = new JObject();
            }

            JObject dependencies = (JObject)manifest["dependencies"];

            // If not present or differs, add/update it
            if (dependencies[ChainsafeDependencyKey] == null || dependencies[ChainsafeDependencyKey].Value<string>() != ChainsafeDependencyUrl)
            {
                dependencies[ChainsafeDependencyKey] = ChainsafeDependencyUrl;
            }

            // Write changes back
            File.WriteAllText(manifestPath, manifest.ToString(), Encoding.UTF8);

            // Set EditorPref so we don't run again
            EditorPrefs.SetBool("Installed scoped registries", true);

            // Refresh to ensure Unity sees the new dependencies
            AssetDatabase.Refresh();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to install scoped registries or Chainsafe dependency: {ex.Message}\n{ex.StackTrace}");
        }
    }
}
#endif
