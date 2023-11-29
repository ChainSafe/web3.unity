using System;
using System.Collections.Generic;
using System.Reflection;
using MetaMask.IO;
using UnityEditor;

using UnityEngine;

using MetaMask.Transports.Unity.UI;
using UnityEngine.Experimental.Rendering;

namespace MetaMask.Unity
{
    public class MetaMaskWindow : EditorWindow
    {
        #region Consts
        /// <summary>The path to the header Image.</summary>
        private const string _headerImagePath = "MetaMask/EditorImages/MetaMask_Header_Logo";
        /// <summary>The path to the logo Image.</summary>
        private const string _metamaskLogoImagePath = "MetaMask/EditorImages/Metamask_Stacked_Logo";
        /// <summary>The path to the background Image.</summary>
        private const string _backgroundImagePath = "MetaMask/EditorImages/MetaMask_EditorWindow_BG";
        /// <summary>The path to the background Image.</summary>
        private const string _buttonImagePath = "MetaMask/EditorImages/MetaMask_Button";

        #endregion

        #region Fields

        /// <summary>The current state of the MetaMask Editor UI Window.</summary>
        enum MetaMaskState
        {
            main,
            install,
            connect
        }

        /// <summary>The current state of the MetaMask client.</summary>       
        private MetaMaskState _state = MetaMaskState.main;
        /// <summary>The style for the header of the main window.</summary>
        private GUIStyle _headerStyle;
        /// <summary>The style for the MetaMask logo.</summary>       
        private GUIStyle _metamaskLogoStyle;
        /// <summary>The style used for text that is displayed in the main window.</summary>
        private GUIStyle _higherTextStyle;
        /// <summary>The style for the H2 text.</summary>
        private GUIStyle _h2TextStyle;
        /// <summary>The style for the body text.</summary>
        private GUIStyle _bodyTextStyle;
        /// <summary>The style for the button style.</summary>
        private GUIStyle _buttonStyle;
        /// <summary>The style for the input field.</summary>
        private GUIStyle _inputFieldStyle;
        /// <summary>The style for the input toggle.</summary>
        private GUIStyle _inputToggleStyle;
        /// <summary>The style for the side-by-side view.</summary>
        private GUIStyle _sidebySideStyle;
        /// <summary>The style for the small Header view.</summary>
        private GUIStyle _smallHeaderStyle;

        /// <summary>The last y-coordinate of the pointer.</summary>
        private float _lastYPosition;
        /// <summary>Gets the name of the application.</summary>
        /// <returns>The name of the application.</returns>
        private string _appNameText = "App Name";
        /// <summary>The text to display in the app URL field.</summary>
        private string _appUrlText = "App Url";
        /// <summary>The text to display in the app URL field.</summary>
        private Texture2D _appIconTexture;
        /// <summary>Gets the user agent string for the current application.</summary>
        /// <returns>The user agent string for the current application.</returns>
        private string _appUserAgentText = "User Agent";
        /// <summary>The text to display in the Encryption Password field.</summary>
        private string _encryptionPasswordText = "Encryption Password";
        /// <summary>The text to display in the Session Identifier field.</summary>
        private string _sessionIdentifierText = "Session Identifier";
        /// <summary>Gets or sets a value indicating whether the application is in debug mode.</summary>
        private bool _logsEnabled;

        #endregion

        #region Editor Methods

        [MenuItem("Tools/MetaMask/Setup Window")]
        /// <summary>Shows the window.</summary>
        public static void ShowWindow()
        {
            var window = GetWindow<MetaMaskWindow>("MetaMask Setup");
            
            LoadSettings(FindCurrentConfig(), window);
        }

        /// <summary>The main GUI function.</summary>
        private void OnGUI()
        {
            DrawBackground();
            MaximumWindow();
            InitStyles();
            if (_state == MetaMaskState.main)
                Installer();
            else if (_state == MetaMaskState.install)
                Credentials();
            else if (_state == MetaMaskState.connect)
                DrawConnect();
        }

        #endregion

        #region Drawer Methods

        private void DrawHeader(string title)
        {
            GUILayout.Box(Resources.Load<Texture>(_headerImagePath), _headerStyle);
            GUILayout.Box(Resources.Load<Texture>(_metamaskLogoImagePath), _metamaskLogoStyle);
            GUILayout.Box(title, _higherTextStyle);
        }

        /// <summary>Draws the connect screen.</summary>
        private void DrawConnect()
        {
            GUILayout.Box(Resources.Load<Texture>(_headerImagePath), _headerStyle);
            GUILayout.Box(Resources.Load<Texture>(_metamaskLogoImagePath), _metamaskLogoStyle);
            GUILayout.Box("SDK Configured!", _higherTextStyle);
            GUILayout.Box(
                "Thank you for configuring the MetaMask SDK. You can now use the MetaMask SDK to connect to the MetaMask Wallet.",
                _bodyTextStyle);
            GUILayout.BeginArea(new Rect((EditorGUIUtility.currentViewWidth / 2) - 70, _lastYPosition + 120, 165, 80));
            if (GUILayout.Button("Spawn Instance", new GUIStyle(_buttonStyle)
            {
                fixedWidth = 135
            }))
            {
                var obj = FindObjectOfType<MetaMaskUnity>();
                if (obj != null)
                {
                    Debug.LogError("An instance of MetaMaskUnity already exists in the currently open scene");
                }
                else
                {
                    var newObj = new GameObject();
                    var mmu = newObj.AddComponent<MetaMaskUnity>();
                    
                    // set default transport
                    var fieldInfo = mmu.GetType().GetField("_transport", BindingFlags.Instance | BindingFlags.NonPublic);
                    fieldInfo?.SetValue(mmu, MetaMaskUnityUITransport.DefaultInstance);
                }
            }

            GUILayout.EndArea();
            StoreYPosition();
            GUILayout.BeginArea(new Rect((EditorGUIUtility.currentViewWidth / 2) - 65, _lastYPosition + 180, 165, 100));
            if (GUILayout.Button("Main Menu", _buttonStyle))
            {
                _state = MetaMaskState.main;
            }

            GUILayout.EndArea();
            StoreYPosition();
        }

        /// <summary>The installer window.</summary>
        private void Installer()
        {
            GUILayout.Box(Resources.Load<Texture>(_headerImagePath), _headerStyle);
            GUILayout.Box(Resources.Load<Texture>(_metamaskLogoImagePath), _metamaskLogoStyle);
            GUILayout.Box("Welcome Back!", _higherTextStyle);
            GUILayout.Box(
                "Welcome to the MetaMask SDK Installer Window, Below you will find our documentation as well as a section to modify the SDK configuration!",
                _bodyTextStyle);
            GUILayout.BeginArea(new Rect((EditorGUIUtility.currentViewWidth / 2) - 65, _lastYPosition + 120, 165, 80));
            if (GUILayout.Button("Documentation", _buttonStyle))
            {
                Application.OpenURL("https://docs.metamask.io/guide/");
            }

            GUILayout.EndArea();
            StoreYPosition();
            GUILayout.BeginArea(new Rect((EditorGUIUtility.currentViewWidth / 2) - 65, _lastYPosition + 180, 165, 100));
            if (GUILayout.Button("Credentials", _buttonStyle))
            {
                _state = MetaMaskState.install;
            }

            GUILayout.EndArea();
            StoreYPosition();
        }

        private Dictionary<string, Vector2> scrollPositions = new();
        private string MakeTextField(string label, string value)
        {
            EditorGUILayout.LabelField(label, this._smallHeaderStyle);
            if (!scrollPositions.ContainsKey(label))
                scrollPositions.Add(label, new Vector2());

            var scrollPosition = scrollPositions[label];
            scrollPositions[label] = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));
            var newValue = GUILayout.TextField(value, _inputFieldStyle, GUILayout.ExpandWidth(true));
            GUILayout.EndScrollView();
            return newValue;
        }

        /// <summary>Displays the credentials screen which allows the configuration of the MetaMask SDK.</summary>
        private void Credentials()
        {
            GUILayout.Box(Resources.Load<Texture>(_headerImagePath), _headerStyle);
            GUILayout.Box("App Configuration", _higherTextStyle);
            GUILayout.Box("Please enter your application configuration data below!", _bodyTextStyle);
            _appNameText = MakeTextField("App Name", _appNameText);
            _appUrlText = MakeTextField("App Url", _appUrlText);
            
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("App Icon", this._smallHeaderStyle);
            _appIconTexture = EditorGUILayout.ObjectField("", _appIconTexture, typeof(Texture2D), true, GUILayout.Width(90), GUILayout.Height(70)) as Texture2D;
            EditorGUILayout.EndVertical();
            
            //_appIconUrlText = MakeTextField("App Icon Url", _appIconUrlText);
            _appUserAgentText = MakeTextField("User Agent", _appUserAgentText);
            var oc = GUI.contentColor;
            GUI.contentColor = _inputToggleStyle.normal.textColor;
            _logsEnabled = GUILayout.Toggle(_logsEnabled, "Logs Enabled", _inputToggleStyle);
            GUI.contentColor = oc;
            GUILayout.BeginHorizontal(_sidebySideStyle);
            if (GUILayout.Button("Back", _buttonStyle))
            {
                _state = MetaMaskState.main;
                Repaint();
            }
            
            GUILayout.Space(25);

            if (GUILayout.Button("Apply", _buttonStyle))
            {
                _state = MetaMaskState.connect;
                Repaint();
                ApplySettings();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            StoreYPosition();
        }

        #endregion

        #region Private Methods

        private static MetaMaskConfig FindCurrentConfig()
        {
            var metamaskUnity = FindObjectOfType<MetaMaskUnity>();
            if (metamaskUnity == null)
            {
                metamaskUnity = MetaMaskUnity.Instance;
            }

            // We cannot use ?. because it may bypass Unity.Object custom null conditional check
            return metamaskUnity != null ? metamaskUnity.Config : MetaMaskConfig.DefaultInstance;
        }

        /// <summary>Applies the current settings to the SDK.</summary>
        private void ApplySettings()
        {
            var metaMaskConfig = FindCurrentConfig();
            SerializedObject soMetaMaskConfig = new SerializedObject(metaMaskConfig);
            SerializedProperty spLoggingEnabled = soMetaMaskConfig.FindProperty("log");
            SerializedProperty spUserAgent = soMetaMaskConfig.FindProperty("userAgent");
            SerializedProperty spAppName = soMetaMaskConfig.FindProperty("appName");
            SerializedProperty spAppUrl = soMetaMaskConfig.FindProperty("appUrl");
            SerializedProperty spAppIconUrl = soMetaMaskConfig.FindProperty("appIcon");
            SerializedProperty spEncryptionPassword = soMetaMaskConfig.FindProperty("encryptionPassword");
            SerializedProperty spSessionIdentifier = soMetaMaskConfig.FindProperty("sessionIdentifier");
            spAppName.stringValue = this._appNameText;
            spAppUrl.stringValue = this._appUrlText;
            spAppIconUrl.stringValue = TextureToBase64(this._appIconTexture);
            spEncryptionPassword.stringValue = this._appUserAgentText;
            spSessionIdentifier.stringValue = this._sessionIdentifierText;
            spLoggingEnabled.boolValue = this._logsEnabled;
            spUserAgent.stringValue = this._appUserAgentText;
            soMetaMaskConfig.ApplyModifiedProperties();
        }

        /// <summary>Loads the settings from the SDK's settings .</summary>
        /// <param name="metaMaskConfig">The configuration to load the settings into.</param>
        /// <param name="metaMaskUIConfig">The configuration to load the settings into.</param>
        /// <param name="window">The window to load the settings into.</param>
        private static void LoadSettings(MetaMaskConfig metaMaskConfig, MetaMaskWindow window)
        {
            SerializedObject soMetaMaskConfig = new SerializedObject(metaMaskConfig);
            SerializedProperty spLoggingEnabled = soMetaMaskConfig.FindProperty("log");
            SerializedProperty spUserAgent = soMetaMaskConfig.FindProperty("userAgent");
            SerializedProperty spAppName = soMetaMaskConfig.FindProperty("appName");
            SerializedProperty spAppUrl = soMetaMaskConfig.FindProperty("appUrl");
            SerializedProperty spAppIcon = soMetaMaskConfig.FindProperty("appIcon");
            SerializedProperty spEncryptionPassword = soMetaMaskConfig.FindProperty("encryptionPassword");
            SerializedProperty spSessionIdentifier = soMetaMaskConfig.FindProperty("sessionIdentifier");
            window._appNameText = spAppName.stringValue;
            window._appUrlText = spAppUrl.stringValue;
            window._appIconTexture = Base64ToTexture(spAppIcon.stringValue);
            window._appUserAgentText = spUserAgent.stringValue;
            window._encryptionPasswordText = spEncryptionPassword.stringValue;
            window._sessionIdentifierText = spSessionIdentifier.stringValue;
            window._logsEnabled = spLoggingEnabled.boolValue;
        }

        /// <summary>Stores the y-position of the last drawn GUI element.</summary>
        private void StoreYPosition()
        {
            if (Event.current.type == EventType.Repaint)
            {
                _lastYPosition = GUILayoutUtility.GetLastRect().y;
            }
        }

        /// <summary>Initializes the styles.</summary>
        private void InitStyles()
        {
            if (_headerStyle == null)
            {
                _headerStyle = new GUIStyle();
                RemovePadding(_headerStyle);
                _headerStyle.fixedHeight = 90;
                _headerStyle.stretchWidth = true;
            }

            if (_metamaskLogoStyle == null)
            {
                _metamaskLogoStyle = new GUIStyle();
                RemovePadding(_metamaskLogoStyle);
                _metamaskLogoStyle.fixedHeight = 220;
                _metamaskLogoStyle.alignment = TextAnchor.MiddleCenter;
                _metamaskLogoStyle.fixedWidth = 240;
                _metamaskLogoStyle.margin = new RectOffset(59, 0, 0, 0);
            }

            if (_higherTextStyle == null)
            {
                _higherTextStyle = new GUIStyle();
                _higherTextStyle.wordWrap = true;
                _higherTextStyle.alignment = TextAnchor.MiddleCenter;
                _higherTextStyle.fontSize = 28;
                _higherTextStyle.fontStyle = FontStyle.Bold;
                _higherTextStyle.normal.textColor = Color.black;
                _higherTextStyle.margin = new RectOffset(0, 0, 0, 20);
            }

            if (_h2TextStyle == null)
            {
                _h2TextStyle = new GUIStyle();
                _h2TextStyle.wordWrap = true;
                _h2TextStyle.alignment = TextAnchor.LowerLeft;
                _h2TextStyle.fontSize = 20;
                _h2TextStyle.fontStyle = FontStyle.Bold;
                _h2TextStyle.normal.textColor = Color.black;
                _h2TextStyle.margin = new RectOffset(20, 0, 5, 10);
            }

            if (_bodyTextStyle == null)
            {
                _bodyTextStyle = new GUIStyle();
                _bodyTextStyle.wordWrap = true;
                _bodyTextStyle.alignment = TextAnchor.MiddleCenter;
                _bodyTextStyle.fontSize = 18;
                _bodyTextStyle.fontStyle = FontStyle.Bold;
                _bodyTextStyle.normal.textColor = Color.grey;
                _bodyTextStyle.margin = new RectOffset(20, 20, 0, 20);
            }

            if (_buttonStyle == null)
            {
                _buttonStyle = new GUIStyle();
                _buttonStyle.normal.background = Resources.Load<Texture2D>(_buttonImagePath);
                _buttonStyle.fontSize = 14;
                _buttonStyle.fontStyle = FontStyle.Bold;
                _buttonStyle.border = new RectOffset(0, 0, 0, 0);
                _buttonStyle.normal.textColor = Color.white;
                _buttonStyle.alignment = TextAnchor.MiddleCenter;
                _buttonStyle.fixedWidth = 125;
                _buttonStyle.fixedHeight = 45;
            }

            if (_inputFieldStyle == null)
            {
                _inputFieldStyle = new GUIStyle(GUI.skin.textField);
                _inputFieldStyle.normal.background = MakeTexture(2, 2, Color.black);
                _inputFieldStyle.fontSize = 16;
                _inputFieldStyle.normal.textColor = Color.white;
                _inputFieldStyle.fontStyle = FontStyle.Bold;
                _inputFieldStyle.margin = new RectOffset(20, 20, 0, 0);
                _inputFieldStyle.padding = new RectOffset(5, 5, 10, 10);
            }

            if (_inputToggleStyle == null)
            {
                _inputToggleStyle = new GUIStyle(GUI.skin.toggle);
                _inputToggleStyle.fontSize = 20;
                _inputToggleStyle.normal.textColor = Color.black;
                _inputToggleStyle.fontStyle = FontStyle.Bold;
                _inputToggleStyle.margin = new RectOffset(20, 20, 0, 0);
                _inputToggleStyle.padding = new RectOffset(35, 5, 10, 10);
            }

            if (_smallHeaderStyle == null)
            {
                _smallHeaderStyle = new GUIStyle("miniLabel");
                _smallHeaderStyle.margin = new RectOffset(20, 0, 0, 0);
                _smallHeaderStyle.padding = new RectOffset(20, 0, 0, 0);
                _smallHeaderStyle.normal.textColor = Color.black;
                _smallHeaderStyle.fontStyle = FontStyle.Bold;
            }

            if (_sidebySideStyle == null)
            {
                _sidebySideStyle = new GUIStyle();
                _sidebySideStyle.margin = new RectOffset(42, 0, 0, 0);
                _sidebySideStyle.padding = new RectOffset(0, 0, 5, 0);
            }
        }

        /// <summary>Removes the padding and border from a GUIStyle.</summary>
        /// <param name="style">The style to remove the padding and border from.</param>
        /// <returns>The style with the padding and border removed.</returns>
        private GUIStyle RemovePadding(GUIStyle style)
        {
            style.padding = new RectOffset(0, 0, 0, 0);
            style.border = new RectOffset(0, 0, 0, 0);
            return style;
        }

        /// <summary>Sets the window to its maximum size.</summary>
        private void MaximumWindow()
        {
            /*if (_state == MetaMaskState.install)
            {
                this.maxSize = new Vector2(365, 825);
                this.minSize = new Vector2(365, 825);
            }
            else
            {*/
                this.maxSize = new Vector2(365, 615);
                this.minSize = new Vector2(365, 615);
            //}
        }

        /// <summary>Draws the background image.</summary>
        private void DrawBackground()
        {
            Texture texture = Resources.Load<Texture>(_backgroundImagePath);
            GUI.DrawTexture(new Rect(0, 0, this.position.width, this.position.height), texture);
        }

        /// <summary>Creates a texture with the specified color.</summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="color">The color of the texture.</param>
        /// <returns>The texture.</returns>
        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D backgroundTexture = new Texture2D(width, height);
            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();
            return backgroundTexture;
        }

        private string TextureToBase64(Texture2D texture2D)
        {
            // resize the texture
            if (texture2D.width > 64 || texture2D.height > 64)
            {
                var source = texture2D;
                var newWidth = 64;
                var newHeight = 64;
                
                RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
                rt.filterMode = source.filterMode;
                RenderTexture.active = rt;
                Graphics.Blit(source, rt);
                Texture2D nTex = new Texture2D(newWidth, newHeight);
                nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
                nTex.Apply();
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(rt);

                texture2D = nTex;
            }
            
            // Always copy the pixels to a in-memory texture
            // Since this texture may not support reading or encoding
            Texture2D newTexture = new Texture2D(texture2D.width, texture2D.height, TextureFormat.RGBAFloat, false);
            newTexture.SetPixels(0,0, texture2D.width, texture2D.height, texture2D.GetPixels(0, 0, texture2D.width, texture2D.height));
            newTexture.Apply();

            byte[] imageData = newTexture.EncodeToPNG();
            return Convert.ToBase64String(imageData);
        }

        private static Texture2D Base64ToTexture(string encodedData)
        {
            try
            {
                byte[] imageData = Convert.FromBase64String(encodedData);

                int width, height;
                GetImageSize(imageData, out width, out height);

                Texture2D texture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
                texture.hideFlags = HideFlags.HideAndDontSave;
                //texture.filterMode = FilterMode.Point;
                texture.LoadImage(imageData, true);

                return texture;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
        
        private static void GetImageSize(byte[] imageData, out int width, out int height)
        {
            width = ReadInt(imageData, 3 + 15);
            height = ReadInt(imageData, 3 + 19);
        }
		
        private static int ReadInt(byte[] imageData, int offset)
        {
            return (imageData[offset] << 8) | imageData[offset + 1];
        }

        #endregion
    }
}