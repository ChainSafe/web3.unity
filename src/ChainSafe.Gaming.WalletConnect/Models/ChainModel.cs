namespace ChainSafe.Gaming.WalletConnect.Models
{
    public class ChainModel
    {
        public const string EvmNamespace = "eip155";

        public ChainModel(string chainNamespace, string chainId, string name)
        {
            ChainNamespace = chainNamespace;

            ChainId = chainId;

            Name = name;
        }

        public string ChainNamespace { get; private set; }

        public string ChainId { get; private set; }

        public string Name { get; private set; }

        public string FullChainId => $"{ChainNamespace}:{ChainId}";
    }
}