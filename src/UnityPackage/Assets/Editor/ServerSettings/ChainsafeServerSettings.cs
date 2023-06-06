using UnityEditor;
using System.Collections;
using UnityEngine;
using SDKConfiguration;
using System;
using System.IO;

public class ChainSafeServerSettings : EditorWindow
{
    public string ProjectID = "Please Enter Your Project ID";
    public string ChainID = "Please Enter Your Chain ID";
    public string Chain = "Please Enter Your Chain i.e Ethereum, Binance, Cronos";
    public string Network = "Please Enter Your Network i.e Mainnet, Testnet";
    public string Token = "Please Enter Your Chain's Native Token i.e Eth, Cro";
    public string RPC = "Please Enter Your RPC";
    public User saveObject;
    Texture2D m_Logo = null;
    GameObject serverCheck = null;
    ProjectConfigScriptableObject projectConfigSO = null;

    // checks if data is already entered
    void Awake()
    {
        if ((ProjectID == ("Please Enter Your Project ID")) && (PlayerPrefs.GetString("ProjectID") != ""))
        {
            ProjectID = PlayerPrefs.GetString("ProjectID");
            PlayerPrefs.Save();
        }

        if ((ChainID == ("Please Enter Your Chain ID")) && (PlayerPrefs.GetString("ChainID") != ""))
        {
            ChainID = PlayerPrefs.GetString("ChainID");
            PlayerPrefs.Save();
        }

        if (Chain == ("Please Enter Your Chain i.e Ethereum, Binance, Cronos") && (PlayerPrefs.GetString("Chain") != ""))
        {
            Chain = PlayerPrefs.GetString("Chain");
            PlayerPrefs.Save();
        }

        if (Network == ("Please Enter Your Network i.e Mainnet, Testnet") && (PlayerPrefs.GetString("Network") != ""))
        {
            Network = PlayerPrefs.GetString("Network");
            PlayerPrefs.Save();
        }

        if (Token == ("Please Enter Your Chain's Native Token i.e Eth, Cro") && (PlayerPrefs.GetString("Token") != ""))
        {
            Token = PlayerPrefs.GetString("Token");
            PlayerPrefs.Save();
        }

        if (RPC == ("Please Enter Your RPC") && (PlayerPrefs.GetString("RPC") != ""))
        {
            RPC = PlayerPrefs.GetString("RPC");
            PlayerPrefs.Save();
        }
    }

    // Initializes window
    [MenuItem("Window/ChainSafeServerSettings")]
    public static void ShowWindow()
    {
        // show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ChainSafeServerSettings));
    }

    // called when menu is opened, loads Chainsafe's Logo
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
        GUILayout.Label("Welcome To The ChainSafe SDK!", EditorStyles.boldLabel);
        GUILayout.Label("Here you can enter all the information needed to get your game started on the blockchain!", EditorStyles.label);
        // inputs
        ProjectID = EditorGUILayout.TextField("Project ID", ProjectID);
        ChainID = EditorGUILayout.TextField("Chain ID", ChainID);
        Chain = EditorGUILayout.TextField("Chain", Chain);
        Network = EditorGUILayout.TextField("Network", Network);
        Token = EditorGUILayout.TextField("Token", Token);
        RPC = EditorGUILayout.TextField("RPC", RPC);
        // buttons

        // register
        if (GUILayout.Button("Need To Register?"))
        {
            Application.OpenURL("https://dashboard.gaming.chainsafe.io/");
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
            // set player prefs for unity open close within the editor
            PlayerPrefs.SetString("ProjectID", ProjectID);
            PlayerPrefs.SetString("ChainID", ChainID);
            PlayerPrefs.SetString("Chain", Chain);
            PlayerPrefs.SetString("Network", Network);
            PlayerPrefs.SetString("Token", Token);
            PlayerPrefs.SetString("RPC", RPC);
            PlayerPrefs.SetString("Registered", "true");
            // set the scriptable object for when the project is built out
            projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
            if (projectConfigSO == null)
            {
                projectConfigSO = CreateInstance<ProjectConfigScriptableObject>();
                AssetDatabase.CreateAsset(projectConfigSO, "Assets/Resources/ProjectConfigData.asset");
            }
            projectConfigSO.ProjectID = ProjectID;
            projectConfigSO.ChainID = ChainID;
            projectConfigSO.Chain = Chain;
            projectConfigSO.Network = Network;
            projectConfigSO.Token = Token;
            projectConfigSO.RPC = RPC;
            EditorUtility.SetDirty(projectConfigSO);
            WriteNetworkFile();
            AssetDatabase.SaveAssets();
            // assign script to prefab and instantiate then destroy after
            serverCheck = (GameObject)Resources.Load("dll", typeof(GameObject));
            GameObject serverCheckScript = (GameObject)Instantiate(serverCheck, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            serverCheckScript.GetComponent<ServerCheck>().CheckProject();
            Debug.Log("Server Check Script: " + serverCheck);
        }
        GUILayout.Label("Reminder: Your ProjectID Must Be Valid To Save & Build With Our SDK. You Can Register For One On Our Website At Dashboard.Gaming.Chainsafe.io", EditorStyles.label);
    }

    public void WriteNetworkFile()
    {
        // declares paths to write our javascript files to
        string path1 = "Assets/WebGLTemplates/Web3GL-2020x/network.js";
        string path2 = "Assets/WebGLTemplates/Web3GL-MetaMask/network.js";

        // writes data to the webgl default network file
        StreamWriter writer1 = new StreamWriter(path1, false);
        writer1.WriteLine("//You can see a list of compatible EVM chains at https://chainlist.org/");
        writer1.WriteLine("window.networks = [");
        writer1.WriteLine("     {");
        writer1.WriteLine("id: " + PlayerPrefs.GetString("ChainID") + ",");
        writer1.WriteLine("label: " + '"' + PlayerPrefs.GetString("Chain") + " " + PlayerPrefs.GetString("Network") + '"' + ",");
        writer1.WriteLine("token: " + '"' + PlayerPrefs.GetString("Token") + '"' + ",");
        writer1.WriteLine("rpcUrl: " + "'" + PlayerPrefs.GetString("RPC") + "'" + ",");
        writer1.WriteLine("     }");
        writer1.WriteLine("]");
        writer1.Close();

        // writes data to the webgl metamask network file
        StreamWriter writer2 = new StreamWriter(path2, false);
        writer2.WriteLine("//You can see a list of compatible EVM chains at https://chainlist.org/");
        writer2.WriteLine("window.web3ChainId = " + PlayerPrefs.GetString("ChainID") + ";");
        writer2.Close();
    }
}