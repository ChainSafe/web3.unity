using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Concrete implementation of <see cref="ISigner"/> via HyperPlay desktop client.
    /// </summary>
    public class HyperPlaySigner : ISigner, ILifecycleParticipant
    {
        private readonly IWalletProvider walletProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperPlaySigner"/> class.
        /// </summary>
        /// <param name="walletProvider">HyperPlay connection provider to connect and make RPC requests.</param>
        public HyperPlaySigner(IWalletProvider walletProvider)
        {
            this.walletProvider = walletProvider;
        }

        public string PublicAddress { get; private set; }

        public async ValueTask WillStartAsync()
        {
            PublicAddress = await walletProvider.Connect();
        }

        /// <summary>
        /// Sign message via HyperPlay desktop client.
        /// </summary>
        /// <param name="message">Message to sign.</param>
        /// <returns>Signed message hash.</returns>
        public Task<string> SignMessage(string message)
        {
            return walletProvider.Perform<string>("personal_sign", message, PublicAddress);
        }

        /// <summary>
        /// Sign typed data via HyperPlay desktop client.
        /// </summary>
        /// <param name="domain">A serializable domain separator.</param>
        /// <param name="message">Data to be signed.</param>
        /// <typeparam name="TStructType">Data type of data to be signed.</typeparam>
        /// <returns>Hash response of a successfully signed typed data.</returns>
        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            return walletProvider.Perform<string>("eth_signTypedData_v3", PublicAddress, typedData);
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }
    }
}