using System.Collections.Generic;
using System.IO;
using ChainSafe.Gaming;
using Newtonsoft.Json;
using Scenes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConnectionHandler))]
public class ConnectionHandlerEditor : Editor
{
    public struct Provider
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
        
        [JsonProperty("path")]
        public string Path { get; private set; }
    }
    
    private bool _foldout;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string json = File.ReadAllText("Packages/io.chainsafe.web3-unity/Runtime/providers.json");
        
        var providers = JsonConvert.DeserializeObject<Provider[]>(json);

        _foldout = EditorGUILayout.Foldout(_foldout, "Connection Providers");
        
        if (_foldout)
        {
            List<ConnectionProvider> availableProviders = new List<ConnectionProvider>();
            
            var providersProperty = serializedObject.FindProperty("providers");

            int arraySize = providersProperty.arraySize;
            
            for (int i = 0; i < arraySize; i++)
            {
                var providerProperty = providersProperty.GetArrayElementAtIndex(i);
                    
                if (providerProperty.objectReferenceValue == null)
                {
                    providersProperty.DeleteArrayElementAtIndex(i);
                        
                    serializedObject.ApplyModifiedProperties();
                        
                    return;
                }
                
                availableProviders.Add(providerProperty.objectReferenceValue as ConnectionProvider);
            }
            
            foreach (var provider in providers)
            {
                var loadedProvider = AssetDatabase.LoadAssetAtPath<ConnectionProvider>(provider.Path);

                if (loadedProvider == null)
                {
                    Debug.LogWarning($"Error loading {provider.Name} Provider at {provider.Path}");
                    
                    continue;
                }
                
                EditorGUI.BeginChangeCheck();

                bool isAvailable = availableProviders.Contains(loadedProvider);
                
                isAvailable = GUILayout.Toggle(isAvailable, provider.Name);

                if (EditorGUI.EndChangeCheck())
                {
                    if (isAvailable)
                    {
                        providersProperty.InsertArrayElementAtIndex(arraySize);

                        providersProperty.GetArrayElementAtIndex(arraySize).objectReferenceValue = loadedProvider;
                    }

                    else
                    {
                        providersProperty.DeleteArrayElementAtIndex(availableProviders.IndexOf(loadedProvider));
                    }

                    serializedObject.ApplyModifiedProperties();
                    
                    return;
                }
            }
        }
    }
}
