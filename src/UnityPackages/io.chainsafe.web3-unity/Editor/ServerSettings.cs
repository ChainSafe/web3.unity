using UnityEditor;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ChainSafe.GamingSdk.Editor;

public class ChainSafeServerSettings : EditorWindow
{
    private string ProjectID = "Please Enter Your Project ID";
    private string ChainID = "Please Enter Your Chain ID";
    private string Chain = "Please Enter Your Chain i.e Ethereum, Binance, Cronos";
    private string Network = "Please Enter Your Network i.e Mainnet, Testnet";
    private string RPC = "Please Enter Your RPC";

    Texture2D logo = null;

    // checks if data is already entered
    void Awake()
    {
        if ((ProjectID == "Please Enter Your Project ID") && (PlayerPrefs.GetString("ProjectID") != ""))
        {
            ProjectID = PlayerPrefs.GetString("ProjectID");
        }

        if ((ChainID == "Please Enter Your Chain ID") && (PlayerPrefs.GetString("ChainID") != ""))
        {
            ChainID = PlayerPrefs.GetString("ChainID");
        }

        if (Chain == "Please Enter Your Chain i.e Ethereum, Binance, Cronos" && (PlayerPrefs.GetString("Chain") != ""))
        {
            Chain = PlayerPrefs.GetString("Chain");
        }

        if (Network == "Please Enter Your Network i.e Mainnet, Testnet" && (PlayerPrefs.GetString("Network") != ""))
        {
            Network = PlayerPrefs.GetString("Network");
        }

        if (RPC == "Please Enter Your RPC" && (PlayerPrefs.GetString("RPC") != ""))
        {
            RPC = PlayerPrefs.GetString("RPC");
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
        ProjectID = EditorGUILayout.TextField("Project ID", ProjectID);
        ChainID = EditorGUILayout.TextField("Chain ID", ChainID);
        Chain = EditorGUILayout.TextField("Chain", Chain);
        Network = EditorGUILayout.TextField("Network", Network);
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
            var projectConfig = ProjectConfigUtilities.CreateOrLoad();
            projectConfig.ProjectID = ProjectID;
            projectConfig.ChainID = ChainID;
            projectConfig.Chain = Chain;
            projectConfig.Network = Network;
            projectConfig.RPC = RPC;
            ProjectConfigUtilities.Save(projectConfig);

            // TODO: this should happen *before* the asset is saved.
            ValidateProjectID(ProjectID);
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
            await ValidateProjectIDAsync(projectID);
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

    private class ValidateProjectIDResponse
    {
        [JsonProperty("response")]
        public bool Response { get; set; }
    }
}