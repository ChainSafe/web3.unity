using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.MetaMask
{
    /// <summary>
    /// Implementation of <see cref="ISigner"/> for Metamask.
    /// </summary>
    public class MetaMaskSigner : ISigner, ILifecycleParticipant
    {
        private readonly IWalletProvider walletProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskSigner"/> class.
        /// </summary>
        /// <param name="walletProvider">Metamask provider that connects to Metamask and makes JsonRPC requests.</param>
        public MetaMaskSigner(IWalletProvider walletProvider)
        {
            this.walletProvider = walletProvider;
        }

        public string PublicAddress { get; private set; }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStartAsync"/>.
        /// Lifetime event method, called during initialization.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public async ValueTask WillStartAsync()
        {
            PublicAddress = await walletProvider.Connect();
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignMessage"/>.
        /// Sign message using MetaMask.
        /// This prompts user to sign a message on MetaMask.
        /// </summary>
        /// <param name="message">Message to sign.</param>
        /// <returns>Hash response of a successfully signed message.</returns>
        public Task<string> SignMessage(string message)
        {
            return walletProvider.Perform<string>("personal_sign", message, PublicAddress);
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignTypedData{TStructType}"/>.
        /// Sign Typed Data using MetaMask.
        /// </summary>
        /// <param name="domain">A serializable domain separator.</param>
        /// <param name="message">Data to be signed.</param>
        /// <typeparam name="TStructType">Data type of data to be signed.</typeparam>
        /// <returns>Hash response of a successfully signed typed data.</returns>
        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            // MetaMask doesn't work with regular eth_signTypedData method, has to be eth_signTypedData_v4.
            return walletProvider.Perform<string>("eth_signTypedData_v4", typedData, PublicAddress);
        }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStopAsync"/>.
        /// Lifetime event method, called during "Web3.TerminateAsync".
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public ValueTask WillStopAsync() => new(Task.CompletedTask);
    }
}