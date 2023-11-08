using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class CreateDeepLink : EditorWindow
{
    Button generateButton;
    TextField uri;


    [MenuItem("Window/Web3Auth/Generate Deep Link")]
    public static void ShowExample()
    {
        CreateDeepLink wnd = GetWindow<CreateDeepLink>();
        wnd.titleContent = new GUIContent("Generate Deep Link");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/Web3AuthSDK/Editor/CreateDeepLink.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/Web3AuthSDK/Editor/CreateDeepLink.uss");
        root.styleSheets.Add(styleSheet);

        generateButton = root.Q<Button>("generateButton");
        generateButton.clicked += OnGenerateButtonClick;

        uri = root.Q<TextField>("uri");
    }

    private void OnGenerateButtonClick()
    {
        Debug.Log(uri.text);
        if (!System.IO.Directory.Exists("Assets/Resources/"))
            System.IO.Directory.CreateDirectory("Assets/Resources/");
        System.IO.File.WriteAllText("Assets/Resources/webauth", uri.text);

        EditorUtility.DisplayDialog("Deep link generated", "Uri \"" + uri.text + "\" is successfully generated", "Ok");
    }
}