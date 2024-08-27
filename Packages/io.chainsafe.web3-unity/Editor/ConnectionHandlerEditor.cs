using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChainSafe.Gaming.UnityPackage.Connection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

// Editor for adding and removing providers in an interactive way.
[CustomEditor(typeof(ConnectionHandler))]
public class ConnectionHandlerEditor : Editor
{
    readonly List<Type> _providerTypes = new List<Type>();
    
    public struct Provider
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
        
        [JsonProperty("path")]
        public string Path { get; private set; }
    }
    
    private bool _foldout;

    private void OnEnable()
    {
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
        {
            _providerTypes.AddRange(assembly.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ConnectionProvider))));
        });
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _foldout = EditorGUILayout.Foldout(_foldout, "Connection Providers");
        
        if (_foldout)
        {
            EditorGUILayout.BeginVertical("Box");
            
            // Get provider display name.
            var providersProperty = serializedObject.FindProperty("providers");

            ConnectionProvider[] allProviders = Resources.LoadAll<ConnectionProvider>(string.Empty);
            
            // Get available providers.
            List<ConnectionProvider> availableProviders = new List<ConnectionProvider>();
                
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
            
            foreach (Type providerType in _providerTypes)
            {
                EditorGUILayout.BeginVertical("Box");
                
                string providerDisplayName = providerType.Name;

                if (providerDisplayName.Contains(nameof(ConnectionProvider)))
                {
                    providerDisplayName = providerDisplayName.Replace(nameof(ConnectionProvider), string.Empty);
                }
                
                EditorGUILayout.LabelField(providerDisplayName, EditorStyles.boldLabel);

                ConnectionProvider provider = allProviders.FirstOrDefault(p => p.GetType() == providerType);

                if (provider != null)
                {
                    bool isAvailable = availableProviders.Contains(provider);

                    EditorGUILayout.BeginHorizontal();
                    
                    EditorGUI.BeginChangeCheck();
                    
                    isAvailable = EditorGUILayout.Toggle(isAvailable, GUILayout.MaxWidth(20));
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (isAvailable)
                        {
                            providersProperty.InsertArrayElementAtIndex(providersProperty.arraySize);
                            
                            providersProperty.GetArrayElementAtIndex(providersProperty.arraySize - 1).objectReferenceValue = provider;
                        }

                        else
                        {
                            int index = availableProviders.IndexOf(provider);
                            
                            providersProperty.DeleteArrayElementAtIndex(index);
                        }
                        
                        serializedObject.ApplyModifiedProperties();
                    }
                    
                    EditorGUI.BeginDisabledGroup(true);
                    
                    EditorGUILayout.ObjectField(provider, typeof(ConnectionProvider), false);
                    
                    EditorGUI.EndDisabledGroup();
                    
                    EditorGUILayout.EndHorizontal();
                }

                else
                {
                    if (GUILayout.Button("Add Provider", GUILayout.MaxWidth(100)))
                    {
                        ConnectionProvider newProvider = (ConnectionProvider) CreateInstance(providerType);
                        
                        AssetDatabase.CreateAsset(newProvider, Path.Combine("Assets", nameof(Resources), $"{providerType.Name}.asset"));
                        
                        providersProperty.InsertArrayElementAtIndex(providersProperty.arraySize);
                        
                        providersProperty.GetArrayElementAtIndex(providersProperty.arraySize - 1).objectReferenceValue = newProvider;
                        
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}
