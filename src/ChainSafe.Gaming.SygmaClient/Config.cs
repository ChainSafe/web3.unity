using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.SygmaClient
{
    public class Config
    {
        private readonly IHttpClient httpClient;
        private readonly uint sourceChainIdId;

        private Environment environment;

        private Dictionary<uint, EvmConfig> domainDictionary;

        public Config(IHttpClient http, uint sourceChainId)
        {
            sourceChainIdId = sourceChainId;
            httpClient = http;
        }

        public EvmConfig SourceDomainConfig => domainDictionary[sourceChainIdId];

        public EvmConfig DestinationDomainConfig(uint destinationChainId) => domainDictionary[destinationChainId];

        public async void Fetch(Environment env)
        {
            if (sourceChainIdId == 0)
            {
                return;
            }

            RawConfig environmentConfig = null;

            environment = env;
            if (environment == Environment.Local)
            {
                environmentConfig = LocalConfig.Fetch();
                domainDictionary = environmentConfig.Domains.ToDictionary(domain => domain.ChainId, value => value);
                return;
            }

            var configUrl = environment switch
            {
                Environment.Devnet => ConfigUrl.Devnet,
                Environment.Testnet => ConfigUrl.Testnet,
                _ => ConfigUrl.Mainnet
            };

            environmentConfig = (await httpClient.Get<RawConfig>(configUrl)).AssertSuccess();
            domainDictionary = environmentConfig.Domains.ToDictionary(domain => domain.ChainId, value => value);
        }
    }
}