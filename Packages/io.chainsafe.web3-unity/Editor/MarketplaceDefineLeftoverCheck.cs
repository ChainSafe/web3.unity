#if MARKETPLACE_AVAILABLE
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public static class MarketplaceDefineLeftoverCheck
{
    // To store package list request
    private static ListRequest listRequest;

    // The package name you want to check
    private static string packageName = "io.chainsafe.web3-unity.marketplace";

    static MarketplaceDefineLeftoverCheck()
    {
        Events.registeredPackages += OnPackagesChanged;
    }

    private static void OnPackagesChanged(PackageRegistrationEventArgs obj)
    {
        if (obj.removed.Any(x => x.name == packageName))
        {
            RemoveCrcRspDefine();
        }
    }


    private static void CheckForPackage()
    {
        listRequest = Client.List(false); // Start the package list request
        while (listRequest.IsCompleted == false) ;
        Debug.Log("OK I AM HERE");
        var packageFound = false;
        foreach (var package in listRequest.Result)
        {
            Debug.Log(package.name);
            if (package.name == packageName)
            {
                packageFound = true;
                break;
            }
        }

        if (!packageFound)
            RemoveCrcRspDefine();
    }

    private static void RemoveCrcRspDefine()
    {
        Debug.Log("HHHH");
        var cscRspPath = Path.Combine(Application.dataPath, "csc.rsp");
        if (!File.Exists(cscRspPath))
            return;
        // Read all lines from the file
        var lines = File.ReadAllLines(cscRspPath);

        if (lines.Length == 1 && lines[0].Contains("-define:MARKETPLACE_AVAILABLE"))
        {
            AssetDatabase.DeleteAsset("Assets/csc.rsp");
            return;
        }


        // Initialize a list to hold modified lines
        var modifiedLines = new List<string>();

        // Check each line and replace the text
        foreach (var line in lines)
        {
            var modifiedLine = line.Replace("-define:MARKETPLACE_AVAILABLE", "");
            modifiedLines.Add(modifiedLine);
        }

        // Write the modified lines back to the file
        File.WriteAllLines(cscRspPath, modifiedLines.ToArray());
    }
}
#endif
