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

        
        // Initializes window
        [MenuItem("ChainSafe SDK/Project Settings", false, -200)]
        public static void ShowWindow()
        {
            var window = (Web3SettingsEditor)GetWindow(typeof(Web3SettingsEditor));
            window.titleContent = new GUIContent("Web3 Settings");
            window.minSize = new Vector2(450, 300);
            
        }
        

        private static GUIStyle centeredLabelStyle;
        private static GUIStyle wrappedGreyMiniLabel;

        // Chain values

        private Web3ConfigAsset web3Config;
        private List<ChainSettingsPanel> chainSettingPanels;
        private List<ChainInfo.Root> chainList;
        private FetchingStatus fetchingStatus = FetchingStatus.NotFetching;

        private Texture2D logo;
        private Vector2 chainsScrollPosition;
        

        private void Awake()
        {
            web3Config = ProjectConfigUtilities.CreateOrLoad();
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
                GUILayout.FlexibleSpace();
                DrawChainsTabContent();
            }

            GUILayout.Space(15);

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
                web3Config.ChainConfigs.Add(newChainConfig);
                chainSettingPanels.Add(new ChainSettingsPanel(this, newChainConfig));
                EditorUtility.SetDirty(web3Config);
            }

            GUILayout.EndScrollView();
        }

        private void DrawFooter()
        {
            GUILayout.Label("ChainSafe Gaming", EditorStyles.centeredGreyMiniLabel);
        }

        private void RemoveChainConfigEntry(string chainId)
        {
            var index = web3Config.ChainConfigs.FindIndex(entry => entry.ChainId == chainId);

            if (index < 0)
            {
                Debug.LogError($"Can't remove. Chain config with id {chainId} was not found.");
                return;
            }

            web3Config.ChainConfigs.RemoveAt(index);
            EditorUtility.SetDirty(web3Config);
            chainSettingPanels.RemoveAt(chainSettingPanels.FindIndex(panel => panel.ChainId == chainId));
        }

        /// <summary>
        /// Validates the project ID via ChainSafe's backend & writes values to the network js file, static so it can be called externally
        /// </summary>
        /// <param name="projectID"></param>
        private static async void ValidateProjectID(string projectID)
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
            if (web3Config.ChainConfigs.Count != 0)
            {
                chainSettingPanels = web3Config.ChainConfigs
                    .Select((chainConfig) => new ChainSettingsPanel(this, chainConfig))
                    .ToList();
            }
            else
            {
                var newChainConfig = ChainConfigEntry.Default;
                web3Config.ChainConfigs.Add(newChainConfig);
                EditorUtility.SetDirty(web3Config);

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

        private enum FetchingStatus
        {
            NotFetching,
            Fetching,
            Fetched
        }
    }
}