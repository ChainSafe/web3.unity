using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using ChainInfo = ChainSafe.Gaming.UnityPackage.Model;

namespace ChainSafe.GamingSdk.Editor
{
    public partial class Web3SettingsEditor : EditorWindow
    {
        // Default values
        private const string EnableAnalyticsScriptingDefineSymbol = "ENABLE_ANALYTICS";

        // Initializes window
        [MenuItem("ChainSafe SDK/Project Settings", false, 1)]
        public static void ShowWindow()
        {
            // Show existing window instance. If it doesn't exist, make one.
            var window = GetWindow(typeof(Web3SettingsEditor));
            window.titleContent = new GUIContent("Web3 Settings");
            window.minSize = new Vector2(450, 300);
        }

        public static void WriteNetworkFile()
        {
            Debug.Log("Updating network.js...");

            var projectConfig = ProjectConfigUtilities.CreateOrLoad();

            if (!projectConfig.ChainConfigs.Any())
            {
                Debug.LogError("Can not generate network.js files for WebGL. Please add at least one Chain Config to continue.");
                return;
            }

            // declares paths to write our javascript files to
            var path1 = "Assets/WebGLTemplates/Web3GL-2020x/network.js";
            var path2 = "Assets/WebGLTemplates/Web3GL-MetaMask/network.js";

            if (AssetDatabase.IsValidFolder(Path.GetDirectoryName(path1)))
            {
                // write data to the webgl default network file
                var sb = new StringBuilder();
                sb.AppendLine("//You can see a list of compatible EVM chains at https://chainlist.org/");
                sb.AppendLine("window.networks = [");
                for (var i = 0; i < projectConfig.ChainConfigs.Count; i++)
                {
                    var chainConfig = projectConfig.ChainConfigs[i];
                    var isLast = i == projectConfig.ChainConfigs.Count - 1;
                    sb.AppendLine("  {");
                    sb.AppendLine("    id: " + chainConfig.ChainId + ",");
                    sb.AppendLine("    label: " + '"' + chainConfig.Chain + " " + chainConfig.Network + '"' + ",");
                    sb.AppendLine("    token: " + '"' + chainConfig.Symbol + '"' + ",");
                    sb.AppendLine("    rpcUrl: " + "'" + chainConfig.Rpc + "'" + ",");
                    sb.AppendLine(!isLast ? "  }," : "  }");
                }
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
                sb.AppendLine("window.web3ChainId = " + projectConfig.ChainConfigs.First().ChainId + ";");
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
        
        private static GUIStyle centeredLabelStyle;
        private static GUIStyle wrappedGreyMiniLabel;

        // Chain values
        public string previousProjectId;

        private ProjectConfigAsset projectConfig;
        private List<ChainSettingsPanel> chainSettingPanels;
        private List<ChainInfo.Root> chainList;
        private FetchingStatus fetchingStatus = FetchingStatus.NotFetching;

        private Texture2D logo;
        private Vector2 chainsScrollPosition;

        private Tabs ActiveTab
        {
            get => (Tabs)EditorPrefs.GetInt("Web3SdkSettingsEditor.Tab", (int)Tabs.Project);
            set => EditorPrefs.SetInt("Web3SdkSettingsEditor.Tab", (int)value);
        }

        private void Awake()
        {
            projectConfig = ProjectConfigUtilities.CreateOrLoad();
            previousProjectId = projectConfig.ProjectId;
        }

        private void OnEnable()
        {
            if (!logo)
                logo = AssetDatabase.LoadAssetAtPath<Texture2D>(
                    "Packages/io.chainsafe.web3-unity/Editor/Textures/ChainSafeLogo2.png");

            TryFetchSupportedChains(); 
        }

        private void OnGUI()
        {
            InitStyles();
            
            using (new EditorGUILayout.VerticalScope())
            {
                DrawHeader();
                DrawBody();
                GUILayout.FlexibleSpace();
                DrawFooter();
            }
        }

        private void InitStyles()
        {
            centeredLabelStyle ??= new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter
            };

            wrappedGreyMiniLabel ??= new GUIStyle(EditorStyles.miniLabel)
            {
                wordWrap = true,
                // alignment = TextAnchor.MiddleCenter,
            };
        }

        private void DrawHeader()
        {
            using (new GUILayout.VerticalScope(GUILayout.Height(240)))
            {
                GUILayout.FlexibleSpace();
                
                // logo layout
                using (new EditorGUILayout.HorizontalScope())
                {
                    // GUILayout.FlexibleSpace();
                    GUILayout.Label(logo, centeredLabelStyle, GUILayout.MaxHeight(160));
                    // GUILayout.FlexibleSpace();
                }
                
                GUILayout.Space(15);
                
                GUILayout.Label("Welcome to web3.unity, the ChainSafe Gaming SDK!", centeredLabelStyle);
                
                GUILayout.FlexibleSpace();
            }
        }

        private void DrawBody()
        {
            // tabs layout
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                ActiveTab = (Tabs)GUILayout.Toolbar((int)ActiveTab, new[] { "Project Settings", "Chain Settings" });
                GUILayout.FlexibleSpace();
            }
            
            // EditorGUILayout.Separator();
            GUILayout.Space(15);

            // DrawHorizontalLine();

            switch (ActiveTab)
            {
                case Tabs.Project:
                    DrawProjectTabContent();
                    break;
                case Tabs.Chains:
                    DrawChainsTabContent();
                    break;
            }
        }

        private void DrawProjectTabContent()
        {
            EditorGUI.BeginChangeCheck();
            
            projectConfig.ProjectId = EditorGUILayout.TextField("Project ID", projectConfig.ProjectId);
            if (string.IsNullOrWhiteSpace(projectConfig.ProjectId))
            {
                EditorGUILayout.HelpBox(
                    "Please enter your Project ID to start using the ChainSafe Gaming SDK.",
                    MessageType.Warning);
                if (GUILayout.Button("Get a Project ID"))
                {
                    Application.OpenURL("https://dashboard.gaming.chainsafe.io/");
                }
            }
            EditorGUILayout.Space();
            projectConfig.EnableAnalytics =
                EditorGUILayout.Toggle(
                    new GUIContent("Allow Analytics:",
                        "Consent to collecting data for analytics purposes. This will help improve our product."),
                    projectConfig.EnableAnalytics);
            GUILayout.Label(
                "We will collect data for analytics to help improve your experience with our SDK. This data allows us to optimize performance, introduce new features, and ensure seamless integration. You can opt out at any time, but we encourage keeping analytics enabled for the best results!", 
                wrappedGreyMiniLabel);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(projectConfig);
                
                if (projectConfig.ProjectId != previousProjectId)
                {
                    ValidateProjectID(projectConfig.ProjectId);
                    previousProjectId = projectConfig.ProjectId;
                }
                
                if (projectConfig.EnableAnalytics)
                    ScriptingDefineSymbols.TryAddDefineSymbol(EnableAnalyticsScriptingDefineSymbol);
                else
                    ScriptingDefineSymbols.TryRemoveDefineSymbol(EnableAnalyticsScriptingDefineSymbol);
            }
        }

        private void DrawChainsTabContent()
        {
            if (fetchingStatus != FetchingStatus.Fetched)
            {
                EditorGUILayout.LabelField(new GUIContent("Fetching supported wallets.."), centeredLabelStyle);
                return;
            }
            
            chainsScrollPosition = GUILayout.BeginScrollView(chainsScrollPosition);

            for (int i = 0; i < chainSettingPanels.Count; i++)
            {
                var panel = chainSettingPanels[i];
                panel.OnGUI();
                EditorGUILayout.Space(25);
            }

            if (GUILayout.Button("+"))
            {
                var newChainConfig = ChainConfigEntry.Empty;
                projectConfig.ChainConfigs.Add(newChainConfig);
                chainSettingPanels.Add(new ChainSettingsPanel(this, newChainConfig));
                EditorUtility.SetDirty(projectConfig);
            }
            
            GUILayout.EndScrollView();
        }

        private void DrawFooter()
        {
            GUILayout.Label("ChainSafe Gaming", EditorStyles.centeredGreyMiniLabel);
        }

        private void RemoveChainConfigEntry(string chainId)
        {
            var index = projectConfig.ChainConfigs.FindIndex(entry => entry.ChainId == chainId);

            if (index < 0)
            {
                Debug.LogError($"Can't remove. Chain config with id {chainId} was not found.");
                return;
            }

            projectConfig.ChainConfigs.RemoveAt(index);
            EditorUtility.SetDirty(projectConfig);
            chainSettingPanels.RemoveAt(chainSettingPanels.FindIndex(panel => panel.ChainId == chainId));
        }
        
        /// <summary>
        /// Validates the project ID via ChainSafe's backend & writes values to the network js file, static so it can be called externally
        /// </summary>
        /// <param name="projectID"></param>
        private static async void ValidateProjectID(string projectID)
        {
            bool projectIdValid;
            try
            {
                projectIdValid = await ValidateProjectIDAsync(projectID);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to validate project ID");
                Debug.LogException(e);
                return;
            }
            
            if (projectIdValid)
            {
#if UNITY_WEBGL
                WriteNetworkFile();
#endif
            }
            
            static async Task<bool> ValidateProjectIDAsync(string projectID)
            {
                var form = new WWWForm();
                form.AddField("projectId", projectID);
                Debug.Log("Checking Project ID..");
                using var www = UnityWebRequest.Post("https://api.gaming.chainsafe.io/project/checkId", form);
                await EditorUtilities.SendAndWait(www);
                const string dbgProjectIDMessage =
                    "Project ID is not valid! Please go to https://dashboard.daming.chainsafe.io to get a new Project ID";

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    Debug.Log("Error checking Project ID!");
                    Debug.LogError(dbgProjectIDMessage);
                    return false;
                }

                var response = JsonConvert.DeserializeObject<ValidateProjectIDResponse>(www.downloadHandler.text);
                if (response.Response.ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase))
                {
                    Debug.Log("Project ID is valid. You can now proceed with building using the SDK!");
                    return true;
                }

                Debug.LogError(dbgProjectIDMessage);
                return false;
            }
        }

        /// <summary>
        /// Fetches the supported EVM chains list from Chainlist's github json
        /// </summary>
        private async void TryFetchSupportedChains()
        {
            if (fetchingStatus == FetchingStatus.Fetching) return;
            fetchingStatus = FetchingStatus.Fetching;
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
            chainList.Insert(0, new ChainInfo.Root
            {
                name = "Custom",
                allowCustomValues = true,
                rpc = new List<string> { string.Empty }
            });

            OnChainListFetched();
            fetchingStatus = FetchingStatus.Fetched;
        }

        private void OnChainListFetched()
        {
            if (projectConfig.ChainConfigs.Count != 0)
            {
                chainSettingPanels = projectConfig.ChainConfigs
                    .Select((chainConfig) => new ChainSettingsPanel(this, chainConfig))
                    .ToList();
            }
            else
            {
                var newChainConfig = ChainConfigEntry.Default;
                projectConfig.ChainConfigs.Add(newChainConfig);
                EditorUtility.SetDirty(projectConfig);
                
                chainSettingPanels = new List<ChainSettingsPanel>
                {
                    new(this, newChainConfig)
                };
            }
        }

        private class ValidateProjectIDResponse
        {
            [JsonProperty("response")] public bool Response { get; set; }
        }
        
        private enum Tabs
        {
            Project = 0,
            Chains = 1
        }

        private enum FetchingStatus
        {
            NotFetching,
            Fetching,
            Fetched
        }
    }
}