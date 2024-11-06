using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChainSafe.Gaming.UnityPackage.Connection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

// Editor for adding and removing providers in an interactive way.
[CustomEditor(typeof(ConnectionHandler))]
public class ConnectionHandlerEditor : Editor
{
    private readonly List<Type> _providerTypes = new List<Type>();

    private Dictionary<Type, Editor> _editors = new Dictionary<Type, Editor>();

    private Dictionary<Type, bool> _editorFoldouts = new Dictionary<Type, bool>();

    private List<ConnectionProvider> _allProviders = new List<ConnectionProvider>();

    private readonly List<ConnectionProvider> _availableProviders = new List<ConnectionProvider>();

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
        Assembly[] unityAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies);
        
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
        {
            if (unityAssemblies.All(a => assembly.GetName().Name != a.name))
            {
                return;
            }
            
            _providerTypes.AddRange(assembly.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ConnectionProvider))));
        });

        _editors = _providerTypes.ToDictionary(t => t, t => default(Editor));

        _editorFoldouts = _providerTypes.ToDictionary(t => t, t => false);

        _allProviders = Resources.LoadAll<ConnectionProvider>(string.Empty).ToList();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _foldout = EditorGUILayout.Foldout(_foldout, "Connection Providers");

        if (_foldout)
        {
            EditorGUILayout.BeginVertical();

            // Get provider display name.
            var providersProperty = serializedObject.FindProperty("providers");

            _allProviders = _allProviders.Where(p => p != null).ToList();

            // Get available providers.
            _availableProviders.Clear();

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

                _availableProviders.Add(providerProperty.objectReferenceValue as ConnectionProvider);
            }

            foreach (Type providerType in _providerTypes)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                string providerDisplayName = providerType.Name;

                if (providerDisplayName.Contains(nameof(ConnectionProvider)))
                {
                    providerDisplayName = providerDisplayName.Replace(nameof(ConnectionProvider), string.Empty);
                }

                ConnectionProvider provider = _allProviders.FirstOrDefault(p => p.GetType() == providerType);

                if (provider != null)
                {
                    EditorGUI.indentLevel++;

                    _editorFoldouts[providerType] = EditorGUILayout.Foldout(_editorFoldouts[providerType], providerDisplayName);

                    EditorGUI.indentLevel--;

                    bool isAvailable = _availableProviders.Contains(provider);

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
                            int index = _availableProviders.IndexOf(provider);

                            providersProperty.DeleteArrayElementAtIndex(index);
                        }

                        serializedObject.ApplyModifiedProperties();
                    }

                    EditorGUI.BeginDisabledGroup(true);

                    EditorGUILayout.ObjectField(provider, typeof(ConnectionProvider), false);

                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.EndHorizontal();

                    if (_editorFoldouts[providerType])
                    {
                        Editor editor = _editors[providerType];

                        if (!editor)
                        {
                            CreateCachedEditor(provider, null, ref editor);

                            _editors[providerType] = editor;
                        }

                        EditorGUILayout.BeginVertical(GUI.skin.box);

                        editor.OnInspectorGUI();

                        EditorGUILayout.EndVertical();
                    }
                }

                else
                {
                    EditorGUILayout.LabelField(providerDisplayName);

                    if (GUILayout.Button("Add Provider", GUILayout.MaxWidth(100)))
                    {
                        ConnectionProvider newProvider = (ConnectionProvider)CreateInstance(providerType);

                        AssetDatabase.CreateAsset(newProvider, Path.Combine("Assets", nameof(Resources), $"{providerType.Name}.asset"));

                        //Update the list of providers.
                        _allProviders.Add(newProvider);

                        _editorFoldouts[providerType] = true;
                        
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
