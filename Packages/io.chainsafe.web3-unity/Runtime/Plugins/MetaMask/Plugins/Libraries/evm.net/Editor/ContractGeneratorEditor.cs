#if UNITY_EDITOR
using System.IO;
using evm.net.Generator;
using evm.net.Models.ABI;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class ContractGeneratorEditor : EditorWindow
{
    private static ContractGeneratorEditor instance;

    private Vector2 AbiScrollPos;
    private string AbiJson = "[\n\t{ }\n]";
    private string rootNamespace = "Contracts";

    [MenuItem("Tools/MetaMask/Contract ABI Converter")]
    public static void Initialize()
    {
        instance = GetWindow<ContractGeneratorEditor>();
        instance.titleContent = new GUIContent("Contract ABI -> Code Generator");
        instance.minSize = new Vector2(440, 292);
        instance.Show();
    }
    
    private void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    private void EditorUpdate()
    {
        
    }

    private void OnGUI()
    {
        if (instance == null)
        {
            Initialize();
        }

        EditorGUILayout.LabelField("Contract ABI Json", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Copy and paste your Contract ABI JSON to the text box below, then click Convert. You may optionally set options to customize the generated C# code.", MessageType.Info);

        AbiScrollPos = EditorGUILayout.BeginScrollView(AbiScrollPos);
        AbiJson = EditorGUILayout.TextArea(AbiJson, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
        
        GUILayout.Space(15f);
        
        EditorGUILayout.LabelField("Contract Code Settings", EditorStyles.boldLabel);

        rootNamespace = EditorGUILayout.TextField("Root Namespace", rootNamespace);

        GUILayout.Space(15f);

        
        if (GUILayout.Button("Convert"))
        {
            string contractName;
            string bytecode = null;
            ContractABI abi;
            try
            {
                var contractArtifact = JsonConvert.DeserializeObject<ContractArtifact>(AbiJson);
                contractName = contractArtifact.ContractName;
                bytecode = contractArtifact.Bytecode;
                abi = contractArtifact.ABI;
            }
            catch (JsonSerializationException)
            {
                // Try just ContractABI
                abi = JsonConvert.DeserializeObject<ContractABI>(AbiJson);
                contractName = rootNamespace.Split(".")[0];
            }
            
            // Choose save directory
            string path = EditorUtility.SaveFolderPanel("Choose folder to save scripts", "Assets", contractName);

            ContractInterfaceGenerator generator = new ContractInterfaceGenerator(rootNamespace, contractName, bytecode, abi);
            var items = generator.GenerateAll();

            foreach (var item in items.Keys)
            {
                var filepath = path + "/" + item + ".cs";
                var text = items[item];
                
                File.WriteAllText(filepath, text);
            }

            AssetDatabase.Refresh();
        }
    }
    
    
}

#endif