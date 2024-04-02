using System.Collections.Generic;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.SygmaClient
{
    public class Config
    {
        private readonly ILogWriter logWriter;
        private readonly IHttpClient httpClient;

        private uint chainId;

        private Environment environment;

        public Config(IHttpClient http, uint chain, ILogWriter logWriter)
        {
            chainId = chain;
            httpClient = http;
            this.logWriter = logWriter;
        }

        public RawConfig EnvironmentConfig { get; set; }

        public async void Fetch(Environment env)
        {
            if (chainId == 0)
            {
                return;
            }

            environment = env;
            if (environment == Environment.Local)
            {
                EnvironmentConfig = LocalConfig.Fetch();
                return;
            }

            var configUrl = environment switch
            {
                Environment.Devnet => ConfigUrl.Devnet,
                Environment.Testnet => ConfigUrl.Testnet,
                _ => ConfigUrl.Mainnet
            };

            var result = await httpClient.GetRaw(configUrl);
            logWriter.Log(result.Response);

            EnvironmentConfig = (await httpClient.Get<RawConfig>(configUrl)).AssertSuccess();
        }

        public EvmConfig SourceDomainConfig()
        {
            return EnvironmentConfig.Domains.Find(cfg => cfg.ChainId == chainId);
        }
    }
}