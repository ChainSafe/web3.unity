using System.Collections.Generic;

using MetaMask.Unity;

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

using UnityEngine;

namespace MetaMask
{

    public class MetaMaskInstallerWindow : EditorWindow
    {

        private static MetaMaskInstallerWindow instance;

        private const string showOnStartupKey = "metamask.installer.showOnStartup";
        private const string BasePath = "Assets/MetaMask/Installer/";
        private const string PackagesPath = BasePath + "Packages/";
        private const string MainPackage = PackagesPath + "main.unitypackage";
        private const string DemoPackage = PackagesPath + "demo.unitypackage";

        private static readonly Dictionary<string, PackageManagerDependency> PackageManagerDependencies = new Dictionary<string, PackageManagerDependency>() {
            { "com.unity.nuget.newtonsoft-json", new PackageManagerDependency( "com.unity.nuget.newtonsoft-json", "Newtonsoft.Json", "https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@latest", false) }
        };

        private static readonly Dictionary<string, PackageDependency> PackageDependencies = new Dictionary<string, PackageDependency>() {
            { "JAR Resolver", new PackageDependency( PackagesPath+ "external-dependency-manager-1.2.175.unitypackage", "JAR Resolver", "https://github.com/googlesamples/unity-jar-resolver",() =>  AssetDatabase.IsValidFolder("Assets/ExternalDependencyManager/"))},
        };

        private static Dictionary<string, bool> DependenciesStatus = new Dictionary<string, bool>();

        private ListRequest listRequest;
        private AddRequest addRequest;
        private Vector2 scrollPosition;

        [MenuItem("Tools/MetaMask/Install")]
        public static void Initialize()
        {
            instance = GetWindow<MetaMaskInstallerWindow>();
            instance.titleContent = new GUIContent("MetaMask Unity Installer");
            instance.minSize = new Vector2(440, 292);
            instance.UpdateDependencies();
            instance.Show();
        }

        [InitializeOnLoadMethod]
        private static void ShowOnStartup()
        {
            var show = EditorPrefs.GetBool(showOnStartupKey, true);
            if (show)
            {
                Initialize();
                EditorPrefs.SetBool(showOnStartupKey, false);
            }
        }

        private void OnEnable()
        {
            EditorApplication.update += EditorUpdate;
            DependenciesStatus = JsonUtility.FromJson<Dictionary<string, bool>>(EditorPrefs.GetString("metamask.dependencies.status", JsonUtility.ToJson(DependenciesStatus)));
        }

        private void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
            EditorPrefs.SetString("metamask.dependencies.status", JsonUtility.ToJson(DependenciesStatus));
        }

        private void EditorUpdate()
        {
            if (instance == null)
            {
                return;
            }
            if (this.listRequest != null && this.listRequest.IsCompleted)
            {
                if (this.listRequest.Status == StatusCode.Success)
                {
                    foreach (var package in this.listRequest.Result)
                    {
                        if (PackageManagerDependencies.ContainsKey(package.name))
                        {
                            PackageManagerDependencies[package.name].Installed = true;
                        }
                    }
                    this.listRequest = null;
                }
                else if (this.listRequest.Status >= StatusCode.Failure)
                {
                    if (this.listRequest.Error != null)
                    {
                        Debug.LogError(this.listRequest.Error.message);
                        this.listRequest = null;
                    }
                }
            }
            
            if(PackageDependencies.Count > 0)
            {
                foreach (var package in PackageDependencies)
                {
                    if (package.Value.CheckInstalled())
                    {
                        DependenciesStatus[package.Key] = true;
                    }
                }
            }
            
            if (this.addRequest != null && this.addRequest.IsCompleted)
            {
                if (this.addRequest.Status == StatusCode.Success)
                {
                    Debug.Log("The dependency has been installed successfully");
                    this.addRequest = null;
                }
                else if (this.addRequest.Status >= StatusCode.Failure)
                {
                    if (this.addRequest.Error != null)
                    {
                        Debug.LogError(this.addRequest.Error.message);
                    }
                    this.addRequest = null;
                }
            }
        }

        private void OnGUI()
        {
            if (instance == null)
            {
                Initialize();
            }
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);
            GUILayout.Label("The below dependencies need to be installed in order to install and use the MetaMask package.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.HelpBox("You can install them using their corresponding install button, or you can just manually install them by downloading them from any other place, or if you have them installed in your project already, check the box alongside that package to mark it as installed so you can proceed with installation of the main package.", MessageType.Info);
            bool allInstalled = true;
            foreach (var item in PackageManagerDependencies)
            {
                if (!DependenciesStatus.ContainsKey(item.Key))
                {
                    DependenciesStatus[item.Key] = false;
                }
                var dependency = item.Value;
                allInstalled &= DependenciesStatus[item.Key] || dependency.Installed;
                GUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.BeginHorizontal();

                DependenciesStatus[item.Key] = EditorGUILayout.ToggleLeft(dependency.DisplayName, DependenciesStatus[item.Key] || dependency.Installed, EditorStyles.boldLabel);

                GUILayout.FlexibleSpace();

                EditorGUI.BeginDisabledGroup(dependency.Installed || this.addRequest != null);

                if (GUILayout.Button(dependency.Installed ? "Installed" : "Install"))
                {
                    if (EditorUtility.DisplayDialog("Install Package", "This will install the '" + dependency.DisplayName + "' package in your project through package manager.", "Install", "Cancel"))
                    {
                        MetaMaskUnityAnalytics.LogEvent($"Install Dependency {dependency.DisplayName}");
                        if (dependency.IsExternal)
                        {
                            this.addRequest = Client.Add(dependency.ExternalUrl);
                        }
                        else
                        {
                            this.addRequest = Client.Add(dependency.PackageName);
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Learn more"))
                {
                    MetaMaskUnityAnalytics.LogEvent($"Learn More Dependency {dependency.DisplayName}");
                    Application.OpenURL(dependency.LearnMoreUrl);
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            
            foreach (var item in PackageDependencies)
            {
                if (!DependenciesStatus.ContainsKey(item.Key))
                {
                    DependenciesStatus[item.Key] = false;
                }
                var dependency = item.Value;
                dependency.Installed = dependency.CheckInstalled();
                allInstalled &= DependenciesStatus[item.Key] || dependency.Installed;
                GUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.BeginHorizontal();

                DependenciesStatus[item.Key] = EditorGUILayout.ToggleLeft(dependency.DisplayName, DependenciesStatus[item.Key] || dependency.Installed, EditorStyles.boldLabel);

                //GUILayout.Label(dependency.DisplayName, EditorStyles.boldLabel);

                GUILayout.FlexibleSpace();

                EditorGUI.BeginDisabledGroup(dependency.Installed || this.addRequest != null);

                if (GUILayout.Button(dependency.Installed ? "Installed" : "Install"))
                {
                    if (EditorUtility.DisplayDialog("Install Package", "This will import the '" + dependency.DisplayName + "' package in your project directly.", "Install", "Cancel"))
                    {
                        AssetDatabase.ImportPackage(dependency.PackagePath, true);
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Learn more"))
                {
                    Application.OpenURL(dependency.LearnMoreUrl);
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            // Demo install
            bool isMetaMaskInstalled = System.Type.GetType("MetaMask.Unity.MetaMaskUnity, MetaMaskUnity.Runtime") != null;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Demo (Optional)", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUI.BeginDisabledGroup(!isMetaMaskInstalled);
            if (GUILayout.Button("Install"))
            {
                InstallDemoPackage();
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            string installLabel = "Install MetaMask";
            bool metaMaskInstallReady = this.listRequest == null && allInstalled;
            if (!metaMaskInstallReady)
            {
                installLabel = "Install Anyway";
            }
            else if (isMetaMaskInstalled)
            {
                installLabel = "Reinstall";
            }
            if (isMetaMaskInstalled)
            {
                installLabel = "Already Installed";
            }
            if (this.listRequest != null)
            {
                installLabel = "Checking...";
            }
            EditorGUI.BeginDisabledGroup(this.listRequest != null);
            if (GUILayout.Button(installLabel, Styles.PrimaryInstallButton, GUILayout.Height(40)))
            {
                InstallMainPackage();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }

        public void InstallDemoPackage()
        {
            AssetDatabase.ImportPackage(DemoPackage, true);
            MetaMaskUnityAnalytics.LogEvent("Install Demo");
        }

        public void InstallMainPackage()
        {
            AssetDatabase.ImportPackage(MainPackage, true);
            MetaMaskUnityAnalytics.LogEvent("Install Main");
        }

        public void UpdateDependencies()
        {
            this.listRequest = Client.List(true);
        }

        private class Dependency
        {

            public readonly string DisplayName;
            public readonly string LearnMoreUrl;

            public bool Installed = false;

            public Dependency(string displayName, string learnMoreUrl)
            {
                this.DisplayName = displayName;
                this.LearnMoreUrl = learnMoreUrl;
            }

        }

        private class PackageManagerDependency : Dependency
        {

            public readonly string PackageName;
            public readonly bool IsExternal;
            public readonly string ExternalUrl;

            public PackageManagerDependency(string packageName, string displayName, string learnMoreUrl, bool external, string externalUrl = null) : base(displayName, learnMoreUrl)
            {
                this.PackageName = packageName;
                this.IsExternal = external;
                this.ExternalUrl = externalUrl;
            }

        }
        
        private class PackageDependency : Dependency
        {

            public readonly string PackagePath;

            public readonly System.Func<bool> CheckInstalled;

            public PackageDependency(string packagePath, string displayName, string learnMoreUrl, System.Func<bool> checkInstalled) : base(displayName, learnMoreUrl)
            {
                this.PackagePath = packagePath;
                this.CheckInstalled = checkInstalled;
            }

        }

        private class Styles
        {

            public static GUIStyle PrimaryInstallButton;

            static Styles()
            {
                PrimaryInstallButton = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 14
                };
            }

        }

    }

}