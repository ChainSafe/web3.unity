using UnityEditor;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ChainSafe.GamingSdk.Editor;
using System.IO;
using System.Text;
using ChainSafe.Gaming.UnityPackage;

/// <summary>
/// Allows the developer to report a bug or join discord for help via GUI
/// </summary>
public class ReportBug : EditorWindow
{
    Texture2D logo = null;

    // Initializes window
    [MenuItem("ChainSafe SDK/Report Bug")]
    public static void ShowWindow()
    {
        // show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(ReportBug));
    }

    // Called when menu is opened, loads Chainsafe Logo
    void OnEnable()
    {
        if (!logo)
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/io.chainsafe.web3-unity/Editor/Textures/ChainSafeLogo.png");
        }
    }

    // Displayed content
    void OnGUI()
    {
        // Image
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label(logo, GUILayout.MaxWidth(250f), GUILayout.MaxHeight(250f));
        EditorGUILayout.EndVertical();
        // Text
        GUILayout.Label("Found an issue with the SDK?", EditorStyles.boldLabel);
        GUILayout.Label("Here you can report a bug and someone from the team will attend to it ASAP", EditorStyles.label);
        // Buttons
        // Report bug
        if (GUILayout.Button("Report Bug"))
        {
            Application.OpenURL("https://github.com/ChainSafe/web3.unity/issues/new?assignees=&labels=Type%3A+Bug&projects=&template=bug_report.md&title=");
        }
        // Discord
        GUILayout.Label("You can also join our discord if you're looking for additional help", EditorStyles.label);
        if (GUILayout.Button("Join Discord"))
        {
            Application.OpenURL("http://discord.gg/n2U6x9c");
        }
    }
}