using UnityEditor;
using System.Collections;
using UnityEngine;
using SDKConfiguration;
using System;

public class ChainSafeServerSettings : EditorWindow
{
    public string ProjectID = "Please Enter Your Project ID";
    public string ChainID = "Please Enter Your Chain ID";
    public string Chain = "Please Enter Your Chain i.e Ethereum, Binance, Cronos";
    public string Network = "Please Enter Your Network i.e Mainnet, Testnet";
    public string RPC = "Please Enter Your RPC";
    public User saveObject;
    Texture2D m_Logo = null;
    GameObject serverCheck = null;

    // checks if data is already entered
    void Awake()
    {
        if ((ProjectID == ("Please Enter Your Project ID")) && (PlayerPrefs.GetString("ProjectID") != ""))
        {
        ProjectID = PlayerPrefs.GetString("ProjectID");
        }

        if ((ChainID == ("Please Enter Your Chain ID")) && (PlayerPrefs.GetString("ChainID") != ""))
        {
        ChainID = PlayerPrefs.GetString("ChainID");
        PlayerPrefs.SetInt("ChainIDInt", int.Parse(PlayerPrefs.GetString("ChainID")));
        }

        if (Chain == ("Please Enter Your Chain i.e Ethereum, Binance, Cronos") && (PlayerPrefs.GetString("Chain") != ""))
        {
        Chain = PlayerPrefs.GetString("Chain");
        }

        if (Network == ("Please Enter Your Network i.e Mainnet, Testnet") && (PlayerPrefs.GetString("Network") != ""))
        {
        Network = PlayerPrefs.GetString("Network");
        }

        if (RPC == ("Please Enter Your RPC") && (PlayerPrefs.GetString("RPC") != ""))
        {
        RPC = PlayerPrefs.GetString("RPC");
        }
    }

    // Initializes window
    [MenuItem("Window/ChainSafeServerSettings")]
    public static void ShowWindow()
    {
        // show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ChainSafeServerSettings));
    }

    // called when menu is opened, loads Chainsafe Logo
    void OnEnable()
    {
        m_Logo = (Texture2D)Resources.Load("chainsafemenulogo", typeof(Texture2D));
    }
    
    // displayed content
    void OnGUI()
    {
        // image
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label(m_Logo, GUILayout.MaxWidth(250f), GUILayout.MaxHeight(250f));
        EditorGUILayout.EndVertical();
        // text
        GUILayout.Label ("Welcome To The ChainSafe SDK!", EditorStyles.boldLabel);
        GUILayout.Label ("Here you can enter all the information needed to get your game started on the blockchain!", EditorStyles.label);
        // inputs
        ProjectID = EditorGUILayout.TextField ("Project ID", ProjectID);
        ChainID = EditorGUILayout.TextField ("Chain ID", ChainID);
        Chain = EditorGUILayout.TextField ("Chain", Chain);
        Network = EditorGUILayout.TextField ("Network", Network);
        RPC = EditorGUILayout.TextField ("RPC", RPC);
        // buttons

        // register
        if (GUILayout.Button("Need To Register?"))
        {
            Application.OpenURL("https://docs.gaming.chainsafe.io/");
        }
        // docs
        if (GUILayout.Button("Check Out Our Docs!"))
        {
            Application.OpenURL("https://docs.gaming.chainsafe.io/");
        }
        // save button
        if (GUILayout.Button("Save Settings"))
        {
            Debug.Log("Saving Settings!");
            // set player prefs for unity open close
            PlayerPrefs.SetString("ProjectID", ProjectID);
            PlayerPrefs.SetString("ChainID", ChainID);
            PlayerPrefs.SetString("Chain", Chain);
            PlayerPrefs.SetInt("ChainIDInt", int.Parse(PlayerPrefs.GetString("ChainID")));
            PlayerPrefs.SetString("Network", Network);
            PlayerPrefs.SetString("RPC", RPC);
            PlayerPrefs.SetString("Registered", "true");
            // assign script to prefab and instantiate then destroy after
            serverCheck = (GameObject)Resources.Load("dll", typeof(GameObject));
            GameObject serverCheckScript = (GameObject)Instantiate(serverCheck, new Vector3(0,0,0), new Quaternion(0,0,0,0));
            serverCheckScript.GetComponent<ServerCheck>().CheckProject();
        }
        GUILayout.Label ("Reminder: Your ProjectID Must Be Valid To Save & Build With Our SDK. You Can Register For One On Our Website At Dashboard.Gaming.Chainsafe.io", EditorStyles.label);
    }
}