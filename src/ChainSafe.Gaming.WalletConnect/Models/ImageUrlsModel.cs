using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// Wallet Connect Model used in <see cref="WalletModel"/> for Wallet's image url if any.
    /// </summary>
    public class ImageUrlsModel
    {
        /// <summary>
        /// Small size wallet icon image url.
        /// </summary>
        [JsonProperty("sm")]
        public string SmallUrl { get; private set; }

        /// <summary>
        /// Medium size wallet icon image url.
        /// </summary>
        [JsonProperty("md")]
        public string MediumUrl { get; private set; }

        /// <summary>
        /// Large size wallet icon image url.
        /// </summary>
        [JsonProperty("lg")]
        public string LargeUrl { get; private set; }
    }
}