using System;
using System.Linq;
using ChainSafe.Gaming;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ChainSafe.GamingSdk.Editor
{
    public partial class Web3SettingsEditor
    {
        private class ChainSettingsPanel
        {
            private readonly Web3SettingsEditor window;
            private readonly Web3ConfigAsset configAsset;
            private readonly ChainConfigEntry chainConfig;

            private int selectedChainIndex;
            private int selectedRpcIndex;
            private StringListSearchProvider searchProvider;
            private ISearchWindowProvider searchWindowProviderImplementation;
            private bool changedRpcOrWs;
            private int selectedWebHookIndex;

            public ChainSettingsPanel(Web3SettingsEditor window, ChainConfigEntry chainConfigEntry)
            {
                this.window = window;
                this.configAsset = window.web3Config;
                this.chainConfig = chainConfigEntry;

                UpdateServerMenuInfo();
            }

            public string ChainId => chainConfig.ChainId;

            public void OnGUI()
            {
                var title = $"{chainConfig.Chain} ({chainConfig.ChainId})";

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Remove"))
                {
                    OnRemoveClick();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();

                // Set string array from chainList to pass into the menu
                var chainOptions = window.chainList.Select(x => x.name).ToArray();
                // Display the dynamically updating Popup
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Select Chain");
                // Show the network drop down menu
                if (GUILayout.Button(chainConfig.Chain, EditorStyles.popup))
                {
                    searchProvider = CreateInstance<StringListSearchProvider>();
                    searchProvider.Initialize(chainOptions, x =>
                    {
                        chainConfig.Chain = x;
                        UpdateServerMenuInfo(true);
                    });
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                        searchProvider);
                }

                EditorGUILayout.EndHorizontal();

                var usingCustomSettings = selectedChainIndex == 0;

                GUI.enabled = usingCustomSettings;

                chainConfig.Network = EditorGUILayout.TextField("Network", chainConfig.Network);
                chainConfig.ChainId = EditorGUILayout.TextField("Chain ID", chainConfig.ChainId);
                chainConfig.Symbol = EditorGUILayout.TextField("Symbol", chainConfig.Symbol);
                chainConfig.BlockExplorerUrl = EditorGUILayout.TextField("Block Explorer", chainConfig.BlockExplorerUrl);

                GUI.enabled = true;

                // Remove "https://" so the user doesn't have to click through 2 levels for the rpc options
                var rpcOptions = window.chainList[selectedChainIndex].rpc.Where(x => x.StartsWith("https"))
                    .Select(x => x.Replace("/", "\u2215")).ToArray();
                if (rpcOptions.Length > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Select RPC");
                    var selectedRpc = window.chainList[selectedChainIndex].rpc[selectedRpcIndex];
                    // Show the rpc drop down menu
                    if (GUILayout.Button(selectedRpc, EditorStyles.popup))
                    {
                        searchProvider = CreateInstance<StringListSearchProvider>();
                        searchProvider.Initialize(rpcOptions, x =>
                        {
                            var str = x.Replace("\u2215", "/");
                            selectedRpcIndex = window.chainList[selectedChainIndex].rpc.IndexOf(str);
                            // Add "https://" back
                            chainConfig.Rpc = str;
                            changedRpcOrWs = true;
                            UpdateServerMenuInfo();
                        });
                        SearchWindow.Open(
                            new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                            searchProvider);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.HelpBox("If you set your custom RPC URI it will override the selection above.", MessageType.Info);
                }

                // Allows for a custom rpc
                chainConfig.Rpc = EditorGUILayout.TextField("Custom RPC", chainConfig.Rpc);


                // Remove "https://" so the user doesn't have to click through 2 levels for the rpc options
                var webHookOptions = window.chainList[selectedChainIndex].rpc.Where(x => x.StartsWith("w"))
                    .Select(x => x.Replace("/", "\u2215")).ToArray();
                if (webHookOptions.Length > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Select WebHook");
                    selectedWebHookIndex =
                        Mathf.Clamp(selectedWebHookIndex, 0, window.chainList[selectedChainIndex].rpc.Count - 1);
                    var webhookIndex = window.chainList[selectedChainIndex].rpc.IndexOf(chainConfig.Ws);
                    var selectedWebHook =
                        webhookIndex == -1 ? window.chainList[selectedChainIndex].rpc[selectedWebHookIndex] : chainConfig.Ws;
                    if (GUILayout.Button(selectedWebHook, EditorStyles.popup))
                    {
                        searchProvider = CreateInstance<StringListSearchProvider>();
                        searchProvider.Initialize(webHookOptions,
                            x =>
                            {
                                var str = x.Replace("\u2215", "/");

                                selectedWebHookIndex = window.chainList[selectedChainIndex].rpc.IndexOf(str);
                                chainConfig.Ws = str;
                                changedRpcOrWs = true;
                                UpdateServerMenuInfo();
                            });
                        SearchWindow.Open(
                            new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                            searchProvider);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.HelpBox("If you set your custom WebHook URI it will override the selection above.", MessageType.Info);
                }

                chainConfig.Ws = EditorGUILayout.TextField("Custom WebHook", chainConfig.Ws);

                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck() || changedRpcOrWs)
                {
                    Debug.Log("Change detected.");
                    changedRpcOrWs = false;
                    EditorUtility.SetDirty(configAsset);
                }
            }

            private void OnRemoveClick()
            {
                if (EditorUtility.DisplayDialog(
                        "Remove Chain Config",
                        "Do you want to remove the chain config?\nThis action can't be undone.",
                        "Remove",
                        "Cancel"))
                {
                    window.RemoveChainConfigEntry(ChainId);
                }
            }

            private void UpdateServerMenuInfo(bool chainSwitched = false)
            {
                // Get the selected chain index
                selectedChainIndex = window.chainList.FindIndex(x => x.name == chainConfig.Chain);
                // Check if the selectedChainIndex is valid
                if (selectedChainIndex >= 0 && selectedChainIndex < window.chainList.Count)
                {
                    // Set chain values
                    var chainPrototype = window.chainList[selectedChainIndex];

                    var overwriteValues = !chainPrototype.allowCustomValues;

                    if (overwriteValues)
                    {
                        chainConfig.Network = chainPrototype.chain;
                        chainConfig.ChainId = chainPrototype.chainId.ToString();
                        chainConfig.Symbol = chainPrototype.nativeCurrency.symbol;
                        if (chainPrototype.explorers != null)
                        {
                            chainConfig.BlockExplorerUrl = chainPrototype.explorers[0].url;
                        }
                    }
                    // Ensure that the selectedRpcIndex is within bounds
                    selectedRpcIndex = Mathf.Clamp(selectedRpcIndex, 0, chainPrototype.rpc.Count - 1);
                    // Set the rpc
                    if (chainSwitched || string.IsNullOrEmpty(chainConfig.Rpc))
                        chainConfig.Rpc = chainPrototype.rpc[selectedRpcIndex];

                    if (chainSwitched)
                    {
                        chainConfig.Ws = chainPrototype.rpc.FirstOrDefault(x => x.StartsWith("wss"));
                        selectedWebHookIndex = chainPrototype.rpc.IndexOf(chainConfig.Ws);
                        changedRpcOrWs = true;
                    }
                    else
                    {
                        selectedWebHookIndex = chainPrototype.rpc.IndexOf(chainConfig.Ws) == -1
                            ? chainPrototype.rpc
                                .IndexOf(chainPrototype.rpc.FirstOrDefault(x => x.StartsWith("wss")))
                            : chainPrototype.rpc.IndexOf(chainConfig.Ws);
                    }
                }
                else
                {
                    // Handle the case where the selected chain is not found
                    Debug.LogError("Couldn't find the chain, switching to default chain.");
                    selectedChainIndex = window.chainList.FindIndex(x => x.name == ChainConfigEntry.Default.Chain);
                }
            }
        }
    }
}