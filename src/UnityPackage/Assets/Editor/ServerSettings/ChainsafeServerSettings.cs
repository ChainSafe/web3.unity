using UnityEditor;
using System.Collections;
using UnityEngine;
using SDKConfiguration;
using System;
using System.IO;

public class ChainSafeServerSettings : EditorWindow
{
    private const string ProjectID = "Please Enter Your Project ID";
    private const string ChainID = "Please Enter Your Chain ID";
    private const string Chain = "Please Enter Your Chain i.e Ethereum, Binance, Cronos";
    private const string Network = "Please Enter Your Network i.e Mainnet, Testnet";
    private const string Symbol = "Please Enter Your Chain's Native Symbol i.e Eth, Cro";
    private const string RPC = "Please Enter Your RPC";
    private string _projectID = ProjectID;
    private string _chainID = ChainID;
    private string _chain = Chain;
    private string _network = Network;
    private string _symbol = Symbol;
    private string _rpc = RPC;
    public User saveObject;
    Texture2D m_Logo = null;
    GameObject serverCheck = null;
    ProjectConfigScriptableObject projectConfigSO = null;

    // checks if data is already entered
    void Awake()
    {
        if ((_projectID == (ProjectID)) && (PlayerPrefs.GetString("ProjectID") != ""))
        {
            _projectID = PlayerPrefs.GetString("ProjectID");
            PlayerPrefs.Save();
        }

        if ((_chainID == (ChainID)) && (PlayerPrefs.GetString("ChainID") != ""))
        {
            _chainID = PlayerPrefs.GetString("ChainID");
            PlayerPrefs.Save();
        }

        if ((_chain == (Chain)) && (PlayerPrefs.GetString("Chain") != ""))
        {
            _chain = PlayerPrefs.GetString("Chain");
            PlayerPrefs.Save();
        }

        if ((_network == (Network)) && (PlayerPrefs.GetString("Network") != ""))
        {
            _network = PlayerPrefs.GetString("Network");
            PlayerPrefs.Save();
        }

        if ((_symbol == (Symbol)) && (PlayerPrefs.GetString("Symbol") != ""))
        {
            _symbol = PlayerPrefs.GetString("Symbol");
            PlayerPrefs.Save();
        }

        if ((_rpc == (RPC)) && (PlayerPrefs.GetString("RPC") != ""))
        {
            _rpc = PlayerPrefs.GetString("RPC");
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
        _projectID = EditorGUILayout.TextField("Project ID", _projectID);
        _chainID = EditorGUILayout.TextField("Chain ID", _chainID);
        _chain = EditorGUILayout.TextField("Chain", _chain);
        _network = EditorGUILayout.TextField("Network", _network);
        _symbol = EditorGUILayout.TextField("Symbol", _symbol);
        _rpc = EditorGUILayout.TextField("RPC", _rpc);
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
            PlayerPrefs.SetString("ProjectID", _projectID);
            PlayerPrefs.SetString("ChainID", _chainID);
            PlayerPrefs.SetString("Chain", _chain);
            PlayerPrefs.SetString("Network", _network);
            PlayerPrefs.SetString("Symbol", _symbol);
            PlayerPrefs.SetString("RPC", _rpc);
            PlayerPrefs.SetString("Registered", "true");
            // set the scriptable object for when the project is built out
            projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
            if (projectConfigSO == null)
            {
                projectConfigSO = CreateInstance<ProjectConfigScriptableObject>();
                AssetDatabase.CreateAsset(projectConfigSO, "Assets/Resources/ProjectConfigData.asset");
            }
            projectConfigSO.ProjectID = _projectID;
            projectConfigSO.ChainID = _chainID;
            projectConfigSO.Chain = _chain;
            projectConfigSO.Network = _network;
            projectConfigSO.Symbol = _symbol;
            projectConfigSO.RPC = _rpc;
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
        writer1.WriteLine("            id: " + PlayerPrefs.GetString("ChainID") + ",");
        writer1.WriteLine("            label: " + '"' + PlayerPrefs.GetString("Chain") + " " + PlayerPrefs.GetString("Network") + '"' + ",");
        writer1.WriteLine("            token: " + '"' + PlayerPrefs.GetString("Symbol") + '"' + ",");
        writer1.WriteLine("            rpcUrl: " + "'" + PlayerPrefs.GetString("RPC") + "'" + ",");
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