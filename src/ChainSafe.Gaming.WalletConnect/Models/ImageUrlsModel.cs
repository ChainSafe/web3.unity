using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    public class ImageUrlsModel
    {
        [JsonProperty("sm")]
        public string SmallUrl { get; private set; }

        [JsonProperty("md")]
        public string MediumUrl { get; private set; }

        [JsonProperty("lg")]
        public string LargeUrl { get; private set; }
    }
}