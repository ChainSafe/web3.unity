using UnityEditor;
using System.IO;
using UnityEngine;

[InitializeOnLoad]
public class CscRspChecker
{
    private static FileSystemWatcher watcher;
    private static string defineSymbol = "MARKETPLACE_AVAILABLE";
    private static string cscRspPath = Path.Combine(Application.dataPath, "../../../Packages/io.chainsafe.web3-unity.marketplace/csc.rsp");
    private static string packagePath = Path.Combine(Application.dataPath, "../../../Packages/io.chainsafe.web3-unity.marketplace");

    static CscRspChecker()
    {
        // Check if the package directory exists
        if (!Directory.Exists(packagePath))
        {
            Debug.Log("Package directory does not exist. Exiting.");
            return;
        }

        // Setup FileSystemWatcher to monitor the csc.rsp file
        SetupFileSystemWatcher();

        // Check if the csc.rsp file exists and handle its content
        CheckAndCreateCscRsp();
    }

    private static void CheckAndCreateCscRsp()
    {
        if (!Directory.Exists(packagePath))
        {
            Debug.Log("Package directory does not exist. Exiting.");
            return;
        }
        
        if (File.Exists(cscRspPath))
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(cscRspPath);
            var containsDefine = false;

            // Check if any line contains the required define
            foreach (var line in lines)
            {
                if (line.Contains($"-define:{defineSymbol}"))
                {
                    containsDefine = true;
                    break;
                }
            }

            // If define is not found, append it to the file
            if (!containsDefine)
            {
                File.AppendAllText(cscRspPath, $"\n-define:{defineSymbol}");
                Debug.Log($"{defineSymbol} define added to csc.rsp file.");
            }
        }
        else
        {
            // If the file does not exist, create it and add define
            File.WriteAllText(cscRspPath, $"-define:{defineSymbol}");
            Debug.Log("csc.rsp file created with MARKETPLACE_AVAILABLE define.");
        }
    }

    private static void SetupFileSystemWatcher()
    {
        watcher = new FileSystemWatcher(Path.GetDirectoryName(cscRspPath))
        {
            Filter = Path.GetFileName(cscRspPath),
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };
        watcher.Deleted += OnCscRspDeleted;
        watcher.EnableRaisingEvents = true;
        Debug.Log("FileSystemWatcher setup to monitor csc.rsp file.");
    }

    private static void OnCscRspDeleted(object sender, FileSystemEventArgs e)
    {
        Debug.Log("csc.rsp file deleted. Removing define.");
        watcher.EnableRaisingEvents = false;
        RemoveDefine();
    }

    private static void RemoveDefine()
    {
        // Remove define from PlayerSettings
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (defines.Contains(defineSymbol))
        {
            defines = defines.Replace(defineSymbol, "").Replace(";;", ";").Trim(';');
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            Debug.Log($"{defineSymbol} define removed from scripting define symbols.");

            // Refresh the editor to apply changes
            AssetDatabase.Refresh();
        }
    }
}