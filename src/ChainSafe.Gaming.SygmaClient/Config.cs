using ChainSafe.Gaming.SygmaClient.Types;

namespace ChainSafe.Gaming.SygmaClient
{
    public class Config
    {
        public Config(uint chainId, Environment environment)
        {
            ChainId = chainId;
            /*
            if (environment === Environment.LOCAL) {
                this.environment = localConfig;
                return;
            }
            */
            var configUrl = string.Empty;
            switch (environment)
            {
                case Environment.Devnet:
                    configUrl = ConfigUrl.Devnet;
                    break;
                case Environment.Testnet:
                    configUrl = ConfigUrl.Testnet;
                    break;
                default:
                    configUrl = ConfigUrl.Mainnet;
                    break;
            }
        }

        private uint ChainId { get; }
    }
}