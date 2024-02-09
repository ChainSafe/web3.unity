using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// WalletConnect wallet model used for identifying and redirecting wallets.
    /// </summary>
    public class WalletModel
    {
        /// <summary>
        /// Id of the wallet.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Name of the wallet.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Description of the wallet.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// Homepage of the wallet.
        /// </summary>
        [JsonProperty("homepage")]
        public string Homepage { get; private set; }

        /// <summary>
        /// <see cref="WalletLinkModel"/> for mobile platforms.
        /// </summary>
        [JsonProperty("mobile")]
        public WalletLinkModel Mobile { get; private set; }

        /// <summary>
        /// <see cref="WalletLinkModel"/> for desktop platforms.
        /// </summary>
        [JsonProperty("desktop")]
        public WalletLinkModel Desktop { get; private set; }

        /// <summary>
        /// wallet icons urls.
        /// </summary>
        [JsonProperty("image_url")]
        public ImageUrlsModel Images { get; private set; }
    }
}