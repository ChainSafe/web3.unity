using UnityEditor;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ChainSafe.GamingSdk.Editor;
using System.IO;
using System.Text;

public class ChainSafeServerSettings : EditorWindow
{
    private const string ProjectID = "Please enter your project ID";
    private const string ChainID = "Please enter your chain ID";
    private const string Chain = "Please enter your chain i.e Ethereum, Binance, Cronos";
    private const string Network = "Please enter your network i.e Mainnet, Testnet";
    private const string Symbol = "Please enter your chain's native symbol i.e Eth, Cro";
    private const string Rpc = "Please enter your RPC endpoint";

    private string projectID = ProjectID;
    private string chainID = ChainID;
    private string chain = Chain;
    private string network = Network;
    private string symbol = Symbol;
    private string rpc = Rpc;

    Texture2D logo = null;

    // checks if data is already entered
    void Awake()
    {
        if ((projectID == ProjectID) && (PlayerPrefs.GetString("ProjectID") != ""))
        {
            projectID = PlayerPrefs.GetString("ProjectID");
        }

        if ((chainID == ChainID) && (PlayerPrefs.GetString("ChainID") != ""))
        {
            chainID = PlayerPrefs.GetString("ChainID");
        }

        if (chain == Chain && (PlayerPrefs.GetString("Chain") != ""))
        {
            chain = PlayerPrefs.GetString("Chain");
        }

        if (network == Network && (PlayerPrefs.GetString("Network") != ""))
        {
            network = PlayerPrefs.GetString("Network");
        }

        if (symbol == Symbol && (PlayerPrefs.GetString("Symbol") != ""))
        {
            symbol = PlayerPrefs.GetString("Symbol");
        }

        if (rpc == Rpc && (PlayerPrefs.GetString("RPC") != ""))
        {
            rpc = PlayerPrefs.GetString("RPC");
        }
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
            projectConfig.ProjectID = projectID;
            projectConfig.ChainID = chainID;
            projectConfig.Chain = chain;
            projectConfig.Network = network;
            projectConfig.Symbol = symbol;
            projectConfig.RPC = rpc;
            ProjectConfigUtilities.Save(projectConfig);

            // TODO: this should happen *before* the asset is saved.
            ValidateProjectID(projectID);
        }
        GUILayout.Label("Reminder: Your ProjectID Must Be Valid To Save & Build With Our SDK. You Can Register For One On Our Website At Dashboard.Gaming.Chainsafe.io", EditorStyles.label);

        if (GUILayout.Button("Syncronize WebGL templates"))
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

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log("Error Checking Project ID!");
            Debug.LogError("ProjectID Not Valid! Please Go To Dashboard.Gaming.Chainsafe.io To Get A New ProjectID");
            return false;
        }

        var response = JsonConvert.DeserializeObject<ValidateProjectIDResponse>(www.downloadHandler.text);
        if (response.Response.ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase))
        {
            Debug.Log("ProjectID Valid, You May Now Build With The SDK!");
            return true;
        }
        else
        {
            Debug.LogError("ProjectID Not Valid! Please Go To Dashboard.Gaming.Chainsafe.IO To Get A New Project ID");
            return false;
        }
    }

    public static void WriteNetworkFile()
    {
        Debug.Log("Updating network.js . . .");

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
            sb.AppendLine("    id: " + PlayerPrefs.GetString("ChainID") + ",");
            sb.AppendLine("    label: " + '"' + PlayerPrefs.GetString("Chain") + " " + PlayerPrefs.GetString("Network") + '"' + ",");
            sb.AppendLine("    token: " + '"' + PlayerPrefs.GetString("Symbol") + '"' + ",");
            sb.AppendLine("    rpcUrl: " + "'" + PlayerPrefs.GetString("RPC") + "'" + ",");
            sb.AppendLine("  }");
            sb.AppendLine("]");
            var textAsset = new TextAsset(sb.ToString());
            AssetDatabase.CreateAsset(textAsset, path1);
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
            sb.AppendLine("window.web3ChainId = " + PlayerPrefs.GetString("ChainID") + ";");
            var textAsset = new TextAsset(sb.ToString());
            AssetDatabase.CreateAsset(textAsset, path2);
        }
        else
        {
            Debug.LogWarning($"{Path.GetDirectoryName(path2)} is missing, network.js file will not be updated for this template");
        }
    }

    private class ValidateProjectIDResponse
    {
        [JsonProperty("response")]
        public bool Response { get; set; }
    }
}