#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Web3AuthSDK.Editor
{
    public class Web3AuthDebug : EditorWindow
    {
        Button debugButton;
        TextField debugLink;

        public static Action<Uri> onURLRecieved;

        [SerializeField] public int index;

        [MenuItem("Window/Web3Auth/Deep Linking Debug")]
        public static void ShowExample()
        {
            Web3AuthDebug wnd = GetWindow<Web3AuthDebug>();
            wnd.titleContent = new GUIContent("Deep Linking Editor Debug");
        }

        public void OnEnable()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/Web3AuthSDK/Debug/Web3AuthDebug.uxml");
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/Web3AuthSDK/Debug/Web3AuthDebug.uss");
            root.styleSheets.Add(styleSheet);

            debugButton = root.Q<Button>("debugButton");
            debugButton.clicked += OnDebugButtonClick;

            debugLink = root.Q<TextField>("debugLink");
        }

        private void OnDebugButtonClick()
        {
            Debug.Log(debugLink.text);
            onURLRecieved(new Uri(debugLink.text));
        }
    }
}
#endif