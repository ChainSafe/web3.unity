using System;
using ChainSafe.Gaming.Mud.Unity;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.Gaming.Mud.UnityEditor
{
    [CustomEditor(typeof(MudConfigAsset))]
    public class MudConfigAssetInspector : Editor
    {
        private MudConfigAsset asset;
        private GUIStyle warningStyle;

        public override void OnInspectorGUI()
        {
            asset = (MudConfigAsset)target;
            warningStyle ??= new GUIStyle(EditorStyles.largeLabel)
            {
                wordWrap = true
            };

            serializedObject.Update();
            var storageProperty = serializedObject.FindProperty(nameof(MudConfigAsset.StorageType));

            EditorGUILayout.PropertyField(storageProperty, new GUIContent("From Block Number"));
            EditorGUILayout.Space();
            GUILayout.Label("Storage Settings", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            switch (asset.StorageType)
            {
                case MudStorageType.LocalStorage:
                    DrawLocalStorageGui();
                    break;
                case MudStorageType.OffchainIndexer:
                    DrawOffchainIndexerStorageGui();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLocalStorageGui()
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(MudConfigAsset.InMemoryFromBlockNumber)),
                new GUIContent("Scan From Block Number"));
        }

        private void DrawOffchainIndexerStorageGui()
        {
            EditorGUILayout.LabelField("Offchain Indexer Storage is not implemented yet. Please use Local Storage for now.", warningStyle);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Switch to Local Storage"))
            {
                asset.StorageType = MudStorageType.LocalStorage;
                EditorUtility.SetDirty(asset);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}