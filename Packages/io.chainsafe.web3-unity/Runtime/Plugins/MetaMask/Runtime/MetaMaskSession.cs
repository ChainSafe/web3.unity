using System;

using MetaMask.Cryptography;
using MetaMask.Models;

using Newtonsoft.Json;

namespace MetaMask
{

    /// <summary>
    /// Manages the MetaMask session information and provides methods for encryption and decryption based on the wallet's and the client's public/private keys.
    /// </summary>
    public class MetaMaskSession
    {

        #region Fields

        protected IEciesProvider ecies;
        protected MetaMaskSessionData data;

        protected string publicKey;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the session's persistent data.
        /// </summary>
        public MetaMaskSessionData Data => this.data;

        public IEciesProvider EciesProvider => this.ecies;

        /// <summary>
        /// Gets the client's public key derived from the private key that has been generated the first time.
        /// </summary>
        public string PublicKey => this.publicKey;

        public DateTime LastActive => this.data.LastActive;

        private readonly string _privateKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="MetaMaskSession"/>.
        /// </summary>
        /// <param name="ecies">The ECIES provider</param>
        /// <param name="data">The session's persistent data</param>
        public MetaMaskSession(IEciesProvider ecies, MetaMaskSessionData data)
        {
            this.ecies = ecies;
            this.data = data;

            // Generate a new private key if there is none
            _privateKey = this.ecies.GeneratePrivateKey();

            // Derive the public key from the private key
            this.publicKey = this.ecies.GetPublicKey(_privateKey);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Prepares a message and decides whether to apply encryption or not based on the <paramref name="encrypt"/>.
        /// </summary>
        /// <param name="data">The data to prepare as <see cref="MetaMaskMessage"/></param>
        /// <param name="encrypt">Whether to encrypt the data or not</param>
        /// <param name="walletPublicKey">The wallet's public key used for encryption</param>
        /// <returns>Returns a new <see cref="MetaMaskMessage"/> that can be encrypted according to <paramref name="encrypt"/></returns>
        public MetaMaskMessage PrepareMessage(object data, bool encrypt, string walletPublicKey)
        {
            var message = new MetaMaskMessage();
            message.Id = this.data.ChannelId;
            if (encrypt && this.ecies != null)
            {
                message.Message = EncryptMessage(data, walletPublicKey);
            }
            else
            {
                message.Message = data;
            }
            return message;
        }

        /// <summary>
        /// Encrypts a message by serializing it to JSON and then using ECIES encryption on top of it.
        /// </summary>
        /// <param name="message">The message to encrypt</param>
        /// <param name="walletPublicKey">The wallet's public key used for encryption</param>
        /// <returns>Returns the encrypted message in string format</returns>
        public string EncryptMessage(object message, string walletPublicKey)
        {
            var json = JsonConvert.SerializeObject(message);
            return this.ecies.Encrypt(json, walletPublicKey);
        }

        /// <summary>
        /// Decrypts a message by using ECIES encryption and the client's private key.
        /// </summary>
        /// <param name="message">The encrypted message to decrypt</param>
        /// <returns>Returns the decrypted message</returns>
        public string DecryptMessage(string message)
        {
            return this.ecies.Decrypt(message, _privateKey);
        }

        #endregion
    }

    /// <summary>
    /// The session's persistent data.
    /// </summary>
    public class MetaMaskSessionData : IAppConfig
    {

        /// <summary>
        /// Gets or sets the client's Application Name.
        /// </summary>
        [JsonProperty("app_name")]
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the client's Application URL.
        /// </summary>
        [JsonProperty("app_url")]
        public string AppUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the client's Application URL.
        /// </summary>
        [JsonProperty("app_icon")]
        public string AppIcon { get; set; }

        /// <summary>
        /// Gets or sets the Channel ID used for communication between MetaMask and the client.
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
        
        /// <summary>
        /// The date and time this session was last active
        /// </summary>
        [JsonProperty("last_active")]
        public DateTime LastActive { get; internal set; } = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of <see cref="MetaMaskSessionData"/>.
        /// </summary>
        public MetaMaskSessionData() { }

        /// <summary>
        /// Initializes a new instance of <see cref="MetaMaskSessionData"/>.
        /// </summary>
        /// <param name="appName">The client's application name</param>
        /// <param name="appUrl">The client's application URL</param>
        public MetaMaskSessionData(IAppConfig appConfig)
        {
            var appName = appConfig.AppName;
            var appUrl = appConfig.AppUrl;
            
            if (string.IsNullOrWhiteSpace(appName))
                throw new ArgumentException(
                    "MetaMask app name cannot be null, please update app name in Window > MetaMask > Setup Window under Credentials");
            
            if (string.IsNullOrWhiteSpace(appUrl))
                throw new ArgumentException(
                    "MetaMask app url cannot be null, please update app name in Window > MetaMask > Setup Window under Credentials");

            AppName = appName;
            AppUrl = appUrl;
            AppIcon = appConfig.AppIcon;
        }
    }
}
