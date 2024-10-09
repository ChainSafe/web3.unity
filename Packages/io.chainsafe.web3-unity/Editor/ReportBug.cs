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
    private static GUIStyle centeredLabelStyle;
    private Texture2D logo;

    // Initializes window
    [MenuItem("ChainSafe SDK/Report Bug", priority = 100)]
    public static void ShowWindow()
    {
        // show existing window instance. If one doesn't exist, make one.
        var window = GetWindow(typeof(ReportBug));
        window.minSize = new Vector2(450, 300);
    }

    // Called when menu is opened, loads Chainsafe Logo
    void OnEnable()
    {
        if (!logo)
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Packages/io.chainsafe.web3-unity/Editor/Textures/ChainSafeLogo2.png");
        }
    }

    // Displayed content
    void OnGUI()
    {
        InitStyles();

        // Image
        DrawHeader();
        // Text
        GUILayout.Label("Found an issue with the SDK?", EditorStyles.boldLabel);
        GUILayout.Label("Here you can report a bug and someone from the team will attend to it ASAP", EditorStyles.label);
        // Buttons
        // Report bug
        if (GUILayout.Button("Report Bug"))
        {
            Application.OpenURL("https://github.com/ChainSafe/web3.unity/issues/new?assignees=&labels=Type%3A+Bug&projects=&template=bug_report.md&title=");
        }
        GUILayout.Space(10);
        // Discord
        GUILayout.Label("You can also join our discord if you're looking for additional help", EditorStyles.label);
        if (GUILayout.Button("Join Discord"))
        {
            Application.OpenURL("http://discord.gg/n2U6x9c");
        }
    }

    private void InitStyles()
    {
        centeredLabelStyle ??= new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter
        };
    }

    private void DrawHeader()
    {
        using (new GUILayout.VerticalScope(GUILayout.Height(200)))
        {
            GUILayout.FlexibleSpace();

            // logo layout
            using (new EditorGUILayout.HorizontalScope())
            {
                // GUILayout.FlexibleSpace();
                GUILayout.Label(logo, centeredLabelStyle, GUILayout.MaxHeight(160));
                // GUILayout.FlexibleSpace();
            }

            GUILayout.FlexibleSpace();
        }
    }
}