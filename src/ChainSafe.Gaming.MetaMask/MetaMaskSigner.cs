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
        /// <param name="logWriter">Log Writer used for logging messages and errors.</param>
        public MetaMaskSigner(IMetaMaskProvider metaMaskProvider, ILogWriter logWriter)
        {
            this.metaMaskProvider = metaMaskProvider;
        }

        /// <summary>
        /// Signer's public key/address.
        /// </summary>
        private string Address { get; set; }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStartAsync"/>.
        /// Lifetime event method, called during initialization.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public async ValueTask WillStartAsync()
        {
            Address = await metaMaskProvider.Connect();
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.GetAddress"/>.
        /// Get public address of connected client.
        /// </summary>
        /// <returns>Wallet address of connected client.</returns>
        public Task<string> GetAddress()
        {
            return Task.FromResult(Address);
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignMessage"/>.
        /// Sign message using MetaMask.
        /// This prompts user to sign a message on MetaMask.
        /// </summary>
        /// <param name="message">Message to sign.</param>
        /// <returns>Hash response of a successfully signed message.</returns>
        public async Task<string> SignMessage(string message)
        {
            return await metaMaskProvider.Request<string>("personal_sign", message, Address);
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignTypedData{TStructType}"/>.
        /// Sign Typed Data using MetaMask.
        /// </summary>
        /// <param name="domain">A serializable domain separator.</param>
        /// <param name="message">Data to be signed.</param>
        /// <typeparam name="TStructType">Data type of data to be signed.</typeparam>
        /// <returns>Hash response of a successfully signed typed data.</returns>
        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            // MetaMask doesn't work with regular eth_signTypedData method, has to be eth_signTypedData_v4.
            return await metaMaskProvider.Request<string>("eth_signTypedData_v4", typedData, Address);
        }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStopAsync"/>.
        /// Lifetime event method, called during "Web3.TerminateAsync".
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);
    }
}