using System.IO;
using System.Linq;
using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.GamingSdk.Editor
{
    /// <summary>
    /// Make connection providers read-write by moving it from packages into Assets/Resources.
    /// </summary>
    [InitializeOnLoad]
    public class ReadWriteProvider
    {
        static ReadWriteProvider()
        {
            TryMovingConnectionProviders();
        }
        
        private static void TryMovingConnectionProviders()
        {
            var providers = Resources.LoadAll<ConnectionProvider>(string.Empty)
                .Where(p => AssetDatabase.GetAssetPath(p).Contains("io.chainsafe.web3-unity"));

            foreach (var provider in providers)
            {
                string source = AssetDatabase.GetAssetPath(provider);
                
                string directory = Path.Combine(Application.dataPath, nameof(Resources));

                string destination = Path.Combine(directory, Path.GetFileName(source));
                
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                File.Move(source, destination);
            }
        }
    }
}