using UnityEditor;
using System.IO;
using UnityEngine;

[InitializeOnLoad]
public class CscRspChecker
{
    private static bool checkDone = false;
    
    static CscRspChecker()
    {
        if (checkDone)
        {
            Debug.Log("Check is done we good");
            return;
        }
        checkDone = true;
        // Define the path to the csc.rsp file
        var cscRspPath = Path.Combine(Application.dataPath, "csc.rsp");

        // Check if the csc.rsp file exists
        if (File.Exists(cscRspPath))
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(cscRspPath);
            var containsDefine = false;

            // Check if any line contains the required define
            foreach (var line in lines)
                if (line.Contains("-define:MARKETPLACE_AVAILABLE"))
                {
                    containsDefine = true;
                    break;
                }

            // If the define is not found, append it to the file
            if (!containsDefine)
            {
                File.AppendAllText(cscRspPath, "\n-define:MARKETPLACE_AVAILABLE");
                Debug.Log("MARKETPLACE_AVAILABLE define added to csc.rsp file.");
            }
        }
        else
        {
            // If the file does not exist, create it and add the define
            File.WriteAllText(cscRspPath, "-define:MARKETPLACE_AVAILABLE");
            Debug.Log("csc.rsp file created with MARKETPLACE_AVAILABLE define.");
        }
        // Setup FileSystemWatcher to monitor the csc.rsp file
        SetupFileSystemWatcher();
    }
    
    private static void SetupFileSystemWatcher()
    {
        watcher = new FileSystemWatcher(Path.GetDirectoryName(cscRspPath));
        watcher.Filter = Path.GetFileName(cscRspPath);
        watcher.Deleted += OnCscRspDeleted;
        watcher.EnableRaisingEvents = true;
    }
    
    private static void OnCscRspDeleted(object sender, FileSystemEventArgs e)
    {
        Debug.Log("csc.rsp file deleted. Removing define.");
        RemoveDefine();
    }
    
    private static void RemoveDefine()
    {
        // Remove the define from PlayerSettings
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (defines.Contains(defineSymbol))
        {
            defines = defines.Replace(defineSymbol, "").Replace(";;", ";");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            Debug.Log($"{defineSymbol} define removed from scripting define symbols.");
        }
    }

   
}