using System;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Marketplace
{
    public class MarketplaceItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "chain_id")]
        public int ChainId { get; set; }

        [JsonProperty(PropertyName = "project_id")]
        public string ProjectId { get; set; }

        [JsonProperty(PropertyName = "marketplace_id")]
        public string MarketPlaceId { get; set; }

        [JsonProperty(PropertyName = "token")]
        public MarketplaceItemToken Token { get; set; }

        [JsonProperty(PropertyName = "marketplace_contract_address")]
        public string MarketPlaceContractAddress { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public string Seller { get; set; }

        [JsonProperty(PropertyName = "buyer")]
        public string Buyer { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "listed_at")]
        public long ListedAtRaw { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string StatusRaw { get; set; }

        public DateTime ListedAt => DateTime.UnixEpoch + TimeSpan.FromMilliseconds(ListedAtRaw);

        public MarketplaceItemStatus Status
        {
            get
            {
                switch (StatusRaw)
                {
                    case "sold": return MarketplaceItemStatus.Sold;
                    case "listed": return MarketplaceItemStatus.Listed;
                    case "canceled": return MarketplaceItemStatus.Canceled;
                    default: throw new ArgumentOutOfRangeException(nameof(StatusRaw));
                }
            }
        }
    }
}