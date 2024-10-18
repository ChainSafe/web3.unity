using Newtonsoft.Json;

namespace ChainSafe.Gaming.Reown.Models
{
    /// <summary>
    /// Reown wallet model used for identifying and redirecting wallets.
    /// </summary>
    public class WalletModel
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("homepage")] public string Homepage { get; set; }

        [JsonProperty("image_id")] public string ImageId { get; set; }

        [JsonProperty("order")] public int Order { get; set; }

        [JsonProperty("mobile_link")] public string MobileLink { get; set; }

        [JsonProperty("desktop_link")] public string DesktopLink { get; set; }

        [JsonProperty("webapp_link")] public string WebappLink { get; set; }

        [JsonProperty("app_store")] public string AppStore { get; set; }

        [JsonProperty("play_store")] public string PlayStore { get; set; }
    }
}