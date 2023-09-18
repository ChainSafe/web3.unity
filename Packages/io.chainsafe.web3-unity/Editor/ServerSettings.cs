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

public class ChainSafeServerSettings : EditorWindow
{
    private const string ProjectIdPrompt = "Please enter your project ID";
    private const string ChainIdPrompt = "Please enter your chain ID";
    private const string ChainPrompt = "Please enter your chain i.e Ethereum, Binance, Cronos";
    private const string NetworkPrompt = "Please enter your network i.e Mainnet, Testnet";
    private const string SymbolPrompt = "Please enter your chain's native symbol i.e Eth, Cro";
    private const string RpcPrompt = "Please enter your RPC endpoint";

    private string projectID;
    private string chainID;
    private string chain;
    private string network;
    private string symbol;
    private string rpc;

    Texture2D logo = null;

    // checks if data is already entered
    void Awake()
    {
        var projectConfig = ProjectConfigUtilities.Load();

        projectID = string.IsNullOrEmpty(projectConfig?.ProjectId) ? ProjectIdPrompt : projectConfig.ProjectId;
        chainID = string.IsNullOrEmpty(projectConfig?.ChainId) ? ChainIdPrompt : projectConfig.ChainId;
        chain = string.IsNullOrEmpty(projectConfig?.Chain) ? ChainPrompt : projectConfig.Chain;
        network = string.IsNullOrEmpty(projectConfig?.Network) ? NetworkPrompt : projectConfig.Network;
        symbol = string.IsNullOrEmpty(projectConfig?.Symbol) ? SymbolPrompt : projectConfig.Symbol;
        rpc = string.IsNullOrEmpty(projectConfig?.Rpc) ? RpcPrompt : projectConfig.Rpc;
    }

    // Initializes window
    [MenuItem("Window/ChainSafeServerSettings")]
    public static void ShowWindow()
    {
        // show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(ChainSafeServerSettings));
    }

    // called when menu is opened, loads Chainsafe Logo
    void OnEnable()
    {
        if (!logo)
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/io.chainsafe.web3-unity/Editor/Textures/ChainSafeLogo.png");
        }
    }

    // displayed content
    void OnGUI()
    {
        // image
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label(logo, GUILayout.MaxWidth(250f), GUILayout.MaxHeight(250f));
        EditorGUILayout.EndVertical();
        // text
        GUILayout.Label("Welcome To The ChainSafe SDK!", EditorStyles.boldLabel);
        GUILayout.Label("Here you can enter all the information needed to get your game started on the blockchain!", EditorStyles.label);
        // inputs
        projectID = EditorGUILayout.TextField("Project ID", projectID);
        chainID = EditorGUILayout.TextField("Chain ID", chainID);
        chain = EditorGUILayout.TextField("Chain", chain);
        network = EditorGUILayout.TextField("Network", network);
        symbol = EditorGUILayout.TextField("Symbol", symbol);
        rpc = EditorGUILayout.TextField("RPC", rpc);
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
            var projectConfig = ProjectConfigUtilities.CreateOrLoad();
            projectConfig.ProjectId = projectID;
            projectConfig.ChainId = chainID;
            projectConfig.Chain = chain;
            projectConfig.Network = network;
            projectConfig.Symbol = symbol;
            projectConfig.Rpc = rpc;
            ProjectConfigUtilities.Save(projectConfig);

            // TODO: this should happen *before* the asset is saved.
            ValidateProjectID(projectID);
        }
        GUILayout.Label("Reminder: Your ProjectID Must Be Valid To Save & Build With Our SDK. You Can Register For One On Our Website At Dashboard.Gaming.Chainsafe.io", EditorStyles.label);

        if (GUILayout.Button("Synchronize WebGL templates"))
        {
            WebGLTemplateSync.Syncronize();
        }
    }

    static async void ValidateProjectID(string projectID)
    {
        try
        {
            if (await ValidateProjectIDAsync(projectID))
            {
                WriteNetworkFile();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to validate project ID");
            Debug.LogException(e);
        }
    }

    internal static async Task<bool> ValidateProjectIDAsync(string projectID)
    {
        var form = new WWWForm();
        form.AddField("projectId", projectID);
        Debug.Log("Checking Project ID!");
        using UnityWebRequest www = UnityWebRequest.Post("https://api.gaming.chainsafe.io/project/checkId", form);
        await EditorUtilities.SendAndWait(www);
        const string dbgProjectIDMessage =
            "Project ID is not valid! Please go to https://dashboard.daming.chainsafe.io to get a new Project ID";

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log("Error Checking Project ID!");
            Debug.LogError(dbgProjectIDMessage);
            return false;
        }

        var response = JsonConvert.DeserializeObject<ValidateProjectIDResponse>(www.downloadHandler.text);
        if (response.Response.ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase))
        {
            Debug.Log("ProjectID is valid, you may now build with The SDK!");
            return true;
        }

        Debug.LogError(dbgProjectIDMessage);
        return false;
    }

    public static void WriteNetworkFile()
    {
        Debug.Log("Updating network.js...");

        var projectConfig = ProjectConfigUtilities.CreateOrLoad();

        // declares paths to write our javascript files to
        string path1 = "Assets/WebGLTemplates/Web3GL-2020x/network.js";
        string path2 = "Assets/WebGLTemplates/Web3GL-MetaMask/network.js";

        if (AssetDatabase.IsValidFolder(Path.GetDirectoryName(path1)))
        {
            // write data to the webgl default network file
            var sb = new StringBuilder();
            sb.AppendLine("//You can see a list of compatible EVM chains at https://chainlist.org/");
            sb.AppendLine("window.networks = [");
            sb.AppendLine("  {");
            sb.AppendLine("    id: " + projectConfig.ChainId + ",");
            sb.AppendLine("    label: " + '"' + projectConfig.Chain + " " + projectConfig.Network + '"' + ",");
            sb.AppendLine("    token: " + '"' + projectConfig.Symbol + '"' + ",");
            sb.AppendLine("    rpcUrl: " + "'" + projectConfig.Rpc + "'" + ",");
            sb.AppendLine("  }");
            sb.AppendLine("]");
            File.WriteAllText(path1, sb.ToString());
        }
        else
        {
            Debug.LogWarning($"{Path.GetDirectoryName(path1)} is missing, network.js file will not be updated for this template");
        }

        if (AssetDatabase.IsValidFolder(Path.GetDirectoryName(path2)))
        {
            // writes data to the webgl metamask network file
            var sb = new StringBuilder();
            sb.AppendLine("//You can see a list of compatible EVM chains at https://chainlist.org/");
            sb.AppendLine("window.web3ChainId = " + projectConfig.ChainId + ";");
            File.WriteAllText(path2, sb.ToString());
        }
        else
        {
            Debug.LogWarning($"{Path.GetDirectoryName(path2)} is missing, network.js file will not be updated for this template");
        }

        AssetDatabase.Refresh();

        Debug.Log("Done");
    }

    private class ValidateProjectIDResponse
    {
        [JsonProperty("response")]
        public bool Response { get; set; }
    }
}