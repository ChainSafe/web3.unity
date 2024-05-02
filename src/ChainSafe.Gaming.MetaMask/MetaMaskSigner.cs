using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.MetaMask
{
    /// <summary>
    /// Implementation of <see cref="ISigner"/> for Metamask.
    /// </summary>
    public class MetaMaskSigner : ISigner, ILifecycleParticipant
    {
        private readonly IMetaMaskProvider metaMaskProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskSigner"/> class.
        /// </summary>
        /// <param name="metaMaskProvider">Metamask provider that connects to Metamask and makes JsonRPC requests.</param>
        public MetaMaskSigner(IMetaMaskProvider metaMaskProvider)
        {
            this.metaMaskProvider = metaMaskProvider;
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.GetAddress"/>.
        /// Get public address of connected client.
        /// </summary>
        /// <value>Wallet address of connected client.</value>
        public string PublicAddress { get; private set; }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStartAsync"/>.
        /// Lifetime event method, called during initialization.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public async ValueTask WillStartAsync()
        {
            PublicAddress = await metaMaskProvider.Connect();
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
            return metaMaskProvider.Request<string>("personal_sign", message, PublicAddress);
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
            return metaMaskProvider.Request<string>("eth_signTypedData_v4", typedData, PublicAddress);
        }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStopAsync"/>.
        /// Lifetime event method, called during "Web3.TerminateAsync".
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public ValueTask WillStopAsync() => new(Task.CompletedTask);
    }
}