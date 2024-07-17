using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage.Connection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

// Editor for adding and removing providers in an interactive way.
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

        var providers = Resources.LoadAll<ConnectionProviderConfig>(string.Empty);

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
                var loadedProvider = provider.ProviderRow;

                if (loadedProvider == null)
                {
                    Debug.LogWarning($"Error loading {provider.Name} Provider.");
                    
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
