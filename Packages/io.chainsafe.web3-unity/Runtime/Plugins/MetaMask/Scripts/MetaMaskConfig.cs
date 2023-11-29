using UnityEngine;

namespace MetaMask.Unity
{

    [CreateAssetMenu(menuName = "MetaMask/Config")]
    public class MetaMaskConfig : ScriptableObject, IAppConfig
    {

        #region Constants

        /// <summary>The path to the resource file containing the configuration.</summary>       
        protected const string ResourcePath = "MetaMask/Config";
        /// <summary>The default resource path.</summary>
        /// <remarks>This is the default resource path used by the <see cref="ResourceExtension"/>.</remarks>
        protected const string DefaultResourcePath = ResourcePath + "/Default";

        #endregion

        #region Fields

        /// <summary>Gets the default instance of the <see cref="MetaMaskConfig"/> class.</summary>
        /// <returns>The default instance of the <see cref="MetaMaskConfig"/> class.</returns>
        protected static MetaMaskConfig defaultInstance;
        
        [Header("Debug Logging")]
        [Tooltip("Whether to turn off the debug logs.")]
        [SerializeField]
        protected bool log = true;
        /// <summary>The name of the application.</summary>
        [Header("App Details")]
        [SerializeField]
        protected string appName = "example";

        /// <summary>The URL of the app.</summary>
        /// <remarks>This is used to determine whether the app is running in the foreground.</remarks>
        [SerializeField]
        protected string appUrl = "example.com";
        
        /// <summary>The URL of the app icon.</summary>
        /// <remarks>This is used to determine where the app icon should be fetched from.</remarks>
        [SerializeField]
        protected string appIcon = "";

        /// <summary>The user agent to use when making requests.</summary>
        /// <remarks>This is used to identify the application when making requests.</remarks>
        [SerializeField]
        protected string userAgent = "UnityUGUITransport/1.0.0";

        [Header("Persistent Data")]
        [Tooltip("Whether to encrypt the persistent data.")]
        [SerializeField]
        protected bool encrypt = true;

        /// <summary>The password used to encrypt the persistent data.</summary>
        [SerializeField]
        protected string encryptionPassword = MetaMaskDataManager.RandomString(12);

        /// <summary>The session identifier.</summary>
        /// <remarks>This is used to store the session data in the local storage.</remarks>
        [SerializeField]
        protected string sessionIdentifier = "metamask.session.data";

        /// <summary>The URL of the socket server.</summary>
        /// <remarks>This is an advanced property.</remarks>
        [Header("Advanced")]
        [SerializeField]
        protected string socketUrl = MetaMaskWallet.SocketUrl;

        #endregion

        #region Properties

        /// <summary>Gets the default instance of the MetaMaskConfig class.</summary>
        /// <returns>The default instance of the MetaMaskConfig class.</returns>
        public static MetaMaskConfig DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = Resources.Load<MetaMaskConfig>(DefaultResourcePath);
                }
                return defaultInstance;
            }
        }

        /// <summary>Gets whether to log details to the console.</summary>
        /// <returns>Whether to log details to the console.</returns>
        public virtual bool Log => this.log;

        /// <summary>Gets the name of the application.</summary>
        /// <returns>The name of the application.</returns>
        public virtual string AppName => this.appName;

        /// <summary>Gets the URL of the app.</summary>
        /// <returns>The URL of the app.</returns>
        public virtual string AppUrl => this.appUrl;
        
        /// <summary>Gets the URL of the app.</summary>
        /// <returns>The URL of the app.</returns>
        public virtual string AppIcon => this.appIcon;

        /// <summary>The user agent to use when making requests.</summary>
        /// <remarks>This is used to identify the application when making requests.</remarks>
        public virtual string UserAgent => this.userAgent;

        /// <summary>Gets whether to encrypt the data.</summary>
        /// <returns>Whether to encrypt the data.</returns>
        public virtual bool Encrypt => this.encrypt;

        /// <summary>Gets the password used to encrypt the data.</summary>
        /// <returns>The password used to encrypt the data.</returns>
        public virtual string EncryptionPassword => this.encryptionPassword;

        /// <summary>Gets the session identifier.</summary>
        /// <returns>The session identifier.</returns>
        public virtual string SessionIdentifier => this.sessionIdentifier;

        /// <summary>Gets the URL of the socket.</summary>
        /// <returns>The URL of the socket.</returns>
        public virtual string SocketUrl => this.socketUrl;

        #endregion

    }

}