using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChainSafe.Gaming.Marketplace
{
    public static class MarketplaceExtensions
    {
        public static bool HasMetadataProperty<T>(this MarketplaceItemToken token, string name)
        {
            if (!token.Metadata.ContainsKey(name))
            {
                return false;
            }

            try
            {
                token.GetMetadataProperty<T>(name);
            }
            catch (Web3Exception)
            {
                return false;
            }

            return true;
        }

        public static T GetMetadataProperty<T>(this MarketplaceItemToken token, string name)
        {
            var rawValue = token.Metadata[name];

            if (rawValue is JObject jObjValue)
            {
                return jObjValue.ToObject<T>();
            }

            if (rawValue is JArray jArrayValue)
            {
                return jArrayValue.ToObject<T>();
            }

            if (rawValue is string)
            {
                return (T)rawValue;
            }

            throw new Web3Exception(
                $"Can't deserialize metadata property with name \"{name}\" to type {typeof(T).Name}");
        }
    }
}