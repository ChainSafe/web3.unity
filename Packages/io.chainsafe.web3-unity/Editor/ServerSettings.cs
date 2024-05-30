using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using ChainInfo = ChainSafe.Gaming.UnityPackage.Model;

/// <summary>
///     Allows the developer to alter chain configuration via GUI
/// </summary>
public class ChainSafeServerSettings : EditorWindow
{
    #region Fields

    // Default values
    private const string ProjectIdPrompt = "Please enter your project ID";
    private const string ChainIdDefault = "11155111";
    private const string ChainDefault = "Ethereum";
    private const string NetworkDefault = "Sepolia";
    private const string SymbolDefault = "Seth";
    private const string RpcDefault = "https://rpc.sepolia.org";

    // Chain values
    private string projectID;
    private string chainID;
    private string chain;
    private string network;
    private string symbol;
    private string rpc;
    private string newRpc;
    public string previousProjectId;

    private Texture2D logo;

    // Search window
    private StringListSearchProvider searchProvider;
    private ISearchWindowProvider _searchWindowProviderImplementation;
    public UnityEvent onDropDownChange;
    private int previousNetworkDropdownIndex;
    private List<ChainInfo.Root> chainList;
    private int selectedChainIndex;
    private int selectedRpcIndex;
    private FetchingStatus fetchingStatus = FetchingStatus.NotFetching;

    private enum FetchingStatus
    {
        NotFetching,
        Fetching,
        Fetched
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Checks if data is already entered, sets default values if not
    /// </summary>
    private void Awake()
    {
        // Get saved settings or revert to default
        var projectConfig = ProjectConfigUtilities.Load();
        projectID = string.IsNullOrEmpty(projectConfig?.ProjectId) ? ProjectIdPrompt : projectConfig.ProjectId;
        chainID = string.IsNullOrEmpty(projectConfig?.ChainId) ? ChainIdDefault : projectConfig.ChainId;
        chain = string.IsNullOrEmpty(projectConfig?.Chain) ? ChainDefault : projectConfig.Chain;
        network = string.IsNullOrEmpty(projectConfig?.Network) ? NetworkDefault : projectConfig.Network;
        symbol = string.IsNullOrEmpty(projectConfig?.Symbol) ? SymbolDefault : projectConfig.Symbol;
        rpc = string.IsNullOrEmpty(projectConfig?.Rpc) ? RpcDefault : projectConfig.Rpc;
        // Search menu
        onDropDownChange = new UnityEvent();
        onDropDownChange.AddListener(UpdateServerMenuInfo);
        // Fetch supported chains
    }

    /// <summary>
    ///     Updates the values in the server settings area when an item is selected
    /// </summary>
    public void UpdateServerMenuInfo()
    {
        // Get the selected chain index
        selectedChainIndex = Array.FindIndex(chainList.ToArray(), x => x.name == chain);

        // Check if the selectedChainIndex is valid
        if (selectedChainIndex >= 0 && selectedChainIndex < chainList.Count)
        {
            // Set chain values
            network = chainList[selectedChainIndex].chain;
            chainID = chainList[selectedChainIndex].chainId.ToString();
            symbol = chainList[selectedChainIndex].nativeCurrency.symbol;
            // Ensure that the selectedRpcIndex is within bounds
            selectedRpcIndex = Mathf.Clamp(selectedRpcIndex, 0, chainList[selectedChainIndex].rpc.Count - 1);
            // Set the rpc
            rpc = chainList[selectedChainIndex].rpc[selectedRpcIndex];
        }
        else
        {
            // Handle the case where the selected chain is not found
            Debug.LogError("Selected chain not found in the chainList.");
        }
    }

    /// <summary>
    ///     Fetches the supported EVM chains list from Chainlist's github json
    /// </summary>
    private async void FetchSupportedChains()
    {
        using var webRequest = UnityWebRequest.Get("https://chainid.network/chains.json");
        await EditorUtilities.SendAndWait(webRequest);
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error Getting Supported Chains: " + webRequest.error);
            return;
        }

        var json = webRequest.downloadHandler.text;
        chainList = JsonConvert.DeserializeObject<List<ChainInfo.Root>>(json);
        chainList = chainList.OrderBy(x => x.name).ToList();
        fetchingStatus = FetchingStatus.Fetched;
    }

    // Initializes window
    [MenuItem("ChainSafe SDK/Server Settings", false, 1)]
    public static void ShowWindow()
    {
        // Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(ChainSafeServerSettings));
    }

    /// <summary>
    ///     Called when menu is opened, loads Chainsafe Logo
    /// </summary>
    private void OnEnable()
    {
        if (!logo)
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Packages/io.chainsafe.web3-unity/Editor/Textures/ChainSafeLogo.png");
    }

    /// <summary>
    ///     Displayed content
    /// </summary>
    private void OnGUI()
    {
        // Image
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label(logo, GUILayout.MaxWidth(250f), GUILayout.MaxHeight(250f));
        EditorGUILayout.EndVertical();
        EditorGUI.BeginChangeCheck();
        // Text
        GUILayout.Label("Welcome To The ChainSafe SDK!", EditorStyles.boldLabel);
        GUILayout.Label("Here you can enter all the information needed to get your game started on the blockchain!",
            EditorStyles.label);
        // Inputs
        projectID = EditorGUILayout.TextField("Project ID", projectID);
        // Search menu
        // Null check to stop the recursive loop before the web request has completed
        if (chainList == null)
        {
            if (fetchingStatus == FetchingStatus.NotFetching || fetchingStatus == FetchingStatus.Fetched)
            {
                fetchingStatus = FetchingStatus.Fetching;
                FetchSupportedChains();
            }

            return;
        }

        // Set string array from chainList to pass into the menu
        var chainOptions = chainList.Select(x => x.name).ToArray();
        // Display the dynamically updating Popup
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Select Chain");
        // Show the network drop down menu
        if (GUILayout.Button(chain, EditorStyles.popup))
        {
            searchProvider = CreateInstance<StringListSearchProvider>();
            searchProvider.Initialize(chainOptions, x => { chain = x; });
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                searchProvider);
        }

        EditorGUILayout.EndHorizontal();
        network = EditorGUILayout.TextField("Network: ", network);
        chainID = EditorGUILayout.TextField("Chain ID: ", chainID);
        symbol = EditorGUILayout.TextField("Symbol: ", symbol);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Select RPC");
        // Remove "https://" so the user doesn't have to click through 2 levels for the rpc options
        var rpcOptions = chainList[selectedChainIndex].rpc.Where(rpc => rpc.StartsWith("https"))
            .Select(rpc => rpc.Substring(8)).ToArray();
        var selectedRpc = rpcOptions[selectedRpcIndex];
        // Show the rpc drop down menu
        if (GUILayout.Button(selectedRpc, EditorStyles.popup))
        {
            searchProvider = CreateInstance<StringListSearchProvider>();
            searchProvider.Initialize(rpcOptions, x =>
            {
                selectedRpcIndex = Array.IndexOf(rpcOptions, x);
                // Add "https://" back
                rpc = "https://" + x;
            });
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                searchProvider);
        }

        EditorGUILayout.EndHorizontal();
        // Allows for a custom rpc
        rpc = EditorGUILayout.TextField("RPC: ", rpc);
        GUILayout.Label("If you're using a custom RPC it will override the selection above", EditorStyles.boldLabel);

        // Buttons
        // Register
        if (GUILayout.Button("Need To Register?")) Application.OpenURL("https://dashboard.gaming.chainsafe.io/");
        // Docs
        if (GUILayout.Button("Check Out Our Docs!")) Application.OpenURL("https://docs.gaming.chainsafe.io/");
        // Save button
        if (EditorGUI.EndChangeCheck())
        {
            var projectConfig = ProjectConfigUtilities.CreateOrLoad();
            projectConfig.ProjectId = projectID;
            projectConfig.ChainId = chainID;
            projectConfig.Chain = chain;
            projectConfig.Network = network;
            projectConfig.Symbol = symbol;
            projectConfig.Rpc = rpc;
            ProjectConfigUtilities.Save(projectConfig);
            if(projectID != previousProjectId)
                ValidateProjectID(projectID);
            previousProjectId = projectConfig.ProjectId;
        }

        GUILayout.Label(
            "Reminder: Your ProjectID Must Be Valid To Save & Build With Our SDK. You Can Register For One On Our Website At Dashboard.Gaming.Chainsafe.io",
            EditorStyles.label);
    }

    /// <summary>
    ///     Validates the project ID via ChainSafe's backend & writes values to the network js file, static so it can be called
    ///     externally
    /// </summary>
    /// <param name="projectID"></param>
    private static async void ValidateProjectID(string projectID)
    {
        try
        {
            if (await ValidateProjectIDAsync(projectID))
            {
#if UNITY_WEBGL
                WriteNetworkFile();
#endif
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to validate project ID");
            Debug.LogException(e);
        }
    }

    /// <summary>
    ///     Validates the project ID via ChainSafe's backend
    /// </summary>
    private static async Task<bool> ValidateProjectIDAsync(string projectID)
    {
        var form = new WWWForm();
        form.AddField("projectId", projectID);
        Debug.Log("Checking Project ID!");
        using var www = UnityWebRequest.Post("https://api.gaming.chainsafe.io/project/checkId", form);
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

    /// <summary>
    ///     Writes values to the network js file
    /// </summary>
    public static void WriteNetworkFile()
    {
        Debug.Log("Updating network.js...");

        var projectConfig = ProjectConfigUtilities.CreateOrLoad();

        // declares paths to write our javascript files to
        var path1 = "Assets/WebGLTemplates/Web3GL-2020x/network.js";
        var path2 = "Assets/WebGLTemplates/Web3GL-MetaMask/network.js";

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
            Debug.LogWarning(
                $"{Path.GetDirectoryName(path1)} is missing, network.js file will not be updated for this template");
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
            Debug.LogWarning(
                $"{Path.GetDirectoryName(path2)} is missing, network.js file will not be updated for this template");
        }

        AssetDatabase.Refresh();

        Debug.Log("Done");
    }

    private class ValidateProjectIDResponse
    {
        [JsonProperty("response")] public bool Response { get; set; }
    }

    #endregion
}