using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Evm.Providers;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    /// Concrete implementation of <see cref="IWalletProvider"/>.
    /// </summary>
    public abstract class WalletProvider : IWalletProvider
    {
        private readonly ChainRegistryProvider chainRegistryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletProvider"/> class.
        /// </summary>
        /// <param name="chainRegistryProvider">Injected <see cref="chainRegistryProvider"/>.</param>
        protected WalletProvider(ChainRegistryProvider chainRegistryProvider)
        {
            this.chainRegistryProvider = chainRegistryProvider;
        }

        public Network LastKnownNetwork { get; private set; }

        async Task<Network> IRpcProvider.RefreshNetwork()
        {
            string chainIdHex = await Perform<string>("eth_chainId");

            ulong chainId = new HexBigInteger(chainIdHex).ToUlong();

            if (chainId <= 0)
            {
                throw new Web3Exception("Couldn't detect network.");
            }

            var chain = await chainRegistryProvider.GetChain(chainId);

            return chain != null
                ? new Network { Name = chain.Name, ChainId = chainId }
                : new Network { Name = "Unknown", ChainId = chainId };
        }

        public abstract Task<string> Connect();

        public abstract Task Disconnect();

        public abstract Task<T> Perform<T>(string method, params object[] parameters);
    }
}