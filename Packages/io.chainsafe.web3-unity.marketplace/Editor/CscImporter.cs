using UnityEditor;
using System.IO;
using UnityEngine;

[InitializeOnLoad]
public class CscRspChecker
{
    private static string defineSymbol = "MARKETPLACE_AVAILABLE";
    private static string cscRspPath = Path.Combine(Application.dataPath, "csc.rsp");
    private static bool initialized;

    static CscRspChecker()
    {
       
        CheckAndCreateCscRsp();
    }

    private static void CheckAndCreateCscRsp()
    {
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
            Debug.Log($"csc.rsp file created with {defineSymbol} define.");
        }
        initialized = true;
    }
}