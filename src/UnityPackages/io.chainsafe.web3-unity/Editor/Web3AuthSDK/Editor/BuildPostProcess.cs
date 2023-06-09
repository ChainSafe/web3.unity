using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections;
using System;

public class BuildPostProcess
{

    // Runs all the post process build steps. Called from Unity during build
    [PostProcessBuildAttribute(0)] // Configures this this post process to run first
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
#if UNITY_IOS
        
        Uri uri = null;
        
        try
        {
            uri = new Uri(System.IO.File.ReadAllText("Assets/Resources/webauth"));
        }
        catch
        { 

            throw new Exception("Deep Link uri is invalid or does not exist. Please generate from \"Window > Web3Auth > Generate Deep Link\" Menu");
        }


        var infoPlist = new UnityEditor.iOS.Xcode.PlistDocument();
        var infoPlistPath = pathToBuiltProject + "/Info.plist";
        infoPlist.ReadFromFile(infoPlistPath);

        // Register ios URL scheme for external apps to launch this app.
        var urlTypeDict = infoPlist.root.CreateArray("CFBundleURLTypes").AddDict();
        urlTypeDict.SetString("CFBundleURLName", uri.Host);

        var urlSchemes = urlTypeDict.CreateArray("CFBundleURLSchemes");
        urlSchemes.AddString(uri.Scheme);

        infoPlist.WriteToFile(infoPlistPath);
#endif
    }
}