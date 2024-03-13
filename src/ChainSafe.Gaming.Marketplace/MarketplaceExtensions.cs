// <copyright file="MarketplaceExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using ChainSafe.Gaming.Web3;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Marketplace extensions.
    /// </summary>
    public static class MarketplaceExtensions
    {
        /// <summary>
        /// Checks if the metadata propery exists.
        /// </summary>
        /// <typeparam name="T">Generic type to be agnostic.</typeparam>
        /// <param name="token">Token name.</param>
        /// <param name="name">Name of the property.</param>
        /// <returns>Bool true or false.</returns>
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

        /// <summary>
        /// Gets metadata properties.
        /// </summary>
        /// <typeparam name="T">Generic type to be agnostic.</typeparam>
        /// <param name="token">Token.</param>
        /// <param name="name">Name.</param>
        /// <returns>Metadata property value.</returns>
        /// <exception cref="Web3Exception">Metadata properly can't be deserialized.</exception>
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