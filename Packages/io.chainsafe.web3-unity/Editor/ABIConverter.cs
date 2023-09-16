using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class ABIWindow : EditorWindow
{
    // The ABI string
    string ABI;

    // Our Text Area ScrollView
    Vector2 TextArea;

    // Where is the window menu located
    [MenuItem("Window/ChainSafe ABI Converter")]

    // Show our window
    public static void ShowWindow()
    {
        GetWindow<ABIWindow>("ChainSafe ABI Converter");
    }


    private void OnGUI()
    {
        // Ensure our labels are using Rich text for added customization
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;

        // Window Header
        GUILayout.Space(10);
        GUILayout.Label("<b><size=15>ChainSafe ABI Converter</size></b>", style);
        GUILayout.Label("Copy/Paste is supported.", style);
        GUILayout.Space(10);

        // Window Body (Our TextArea)
        TextArea = EditorGUILayout.BeginScrollView(TextArea, GUILayout.Height(200));
        GUILayout.Space(10);
        ABI = EditorGUILayout.TextArea(ABI);
        GUILayout.Space(10);
        GUILayout.EndScrollView();

        // The logic for formatting our ABI
        if (GUILayout.Button("Convert", GUILayout.Height(30)))
        {
            string RemoveNewLines = ABI.Replace("\n", "");
            string RemoveWhiteSpaces = Regex.Replace(RemoveNewLines, @"\s", "");
            string ImprovedABI = RemoveWhiteSpaces.Replace("\"", "\\\"");

            // Copy the results to the users clipboard
            GUIUtility.systemCopyBuffer = "\"" + ImprovedABI + "\";";

            Debug.Log("ABI Conversion Complete. Results copied to clipboard!");
        }

        // Window Footer (Instructions)
        GUILayout.Space(10);
        GUILayout.Label("This tool will automatically convert your ABI for use with the web3.unity SDK. \n", EditorStyles.largeLabel);
        GUILayout.Label("<b>=============</b>", style);
        GUILayout.Label("<b>  INSTRUCTIONS</b>", style);
        GUILayout.Label("<b>=============</b> \n", style);
        GUILayout.Label(" 1. Copy your ABI from https://remix.ethereum.org/ \n 2. Paste it in the field above \n 3. Click on \"Convert\" \n 4. The new ABI should automatically be copied to your clipboard! \n 5. Add the ABI to your contract manager or custom code.", EditorStyles.largeLabel);

        GUILayout.Space(10);

        // Start of our footer buttons
        GUILayout.BeginHorizontal();

        // Our documentation button
        if (GUILayout.Button("Documentation", GUILayout.Width(110), GUILayout.Height(25)))
        {
            Application.OpenURL("https://docs.gaming.chainsafe.io/current/getting-started");
        }

        // Our discord button
        if (GUILayout.Button("Join Discord", GUILayout.Width(110), GUILayout.Height(25)))
        {
            Application.OpenURL("https://discord.gg/MM67VQwaKX");
        }

        GUILayout.EndHorizontal();


    }

}
