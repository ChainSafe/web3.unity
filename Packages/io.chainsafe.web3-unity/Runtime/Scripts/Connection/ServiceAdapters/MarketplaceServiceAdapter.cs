using System;
using System.IO;
using ChainSafe.Gaming.Marketplace;
using ChainSafe.Gaming.Marketplace.Extensions;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Enables usage of Marketplace when attached.
    /// </summary>
    public class MarketplaceServiceAdapter : MonoBehaviour, ILightWeightServiceAdapter
    {
        [SerializeField] private MarketplaceConfigUnity marketplaceConfig;
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                if (string.IsNullOrEmpty(marketplaceConfig.MarketplaceContractAddress))
                    throw new InvalidDataException("Please provide an appropriate marketplace address");

                if (string.IsNullOrEmpty(marketplaceConfig.MarketplaceId))
                    throw new InvalidDataException("Please provide an appropriate marketplace id");

                services.UseMarketplace(marketplaceConfig);
            });
        }
    }

    [Serializable]
    public class MarketplaceConfigUnity : IMarketplaceConfig
    {
        [field: SerializeField] public string EndpointOverride { get; set; }
        [field: SerializeField] public string MarketplaceContractAbiOverride { get; set; }
        [field: SerializeField] public string ProjectIdOverride { get; set; }
        [field: SerializeField] public string MarketplaceId { get; set; }
        [field: SerializeField] public string MarketplaceContractAddress { get; set; }
    }


}
