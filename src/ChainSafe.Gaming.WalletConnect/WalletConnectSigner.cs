using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Implementation of <see cref="ISigner"/> for Wallet Connect.
    /// </summary>
    public class WalletConnectSigner : ISigner, ILifecycleParticipant
    {
        private readonly IWalletConnectCustomProvider walletConnectCustomProvider;
        private readonly WalletConnectConfig config;

        private string address;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletConnectSigner"/> class.
        /// </summary>
        /// <param name="walletConnectCustomProvider">Wallet Connect Provider that connects wallet and makes jsom RPC requests via Wallet Connect.</param>
        /// <param name="config">Wallet Connect config for passing configuration values.</param>
        public WalletConnectSigner(IWalletConnectCustomProvider walletConnectCustomProvider, WalletConnectConfig config)
        {
            this.walletConnectCustomProvider = walletConnectCustomProvider;
            this.config = config;
        }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStartAsync"/>.
        /// Lifetime event method, called during initialization.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public async ValueTask WillStartAsync()
        {
            // if testing just don't initialize wallet connect
            if (config.Testing)
            {
                config.TestWalletAddress?.AssertIsPublicAddress(nameof(config.TestWalletAddress));

                address = config.TestWalletAddress;

                return;
            }

            // get address by connecting
            address = await walletConnectCustomProvider.Connect();
        }

        /// <summary>
        /// Implementation of <see cref="ILifecycleParticipant.WillStopAsync"/>.
        /// Lifetime event method, called during <see cref="Web3.TerminateAsync"/>.
        /// </summary>
        /// <returns>async awaitable task.</returns>
        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.GetAddress"/>.
        /// Get public address of connected client.
        /// </summary>
        /// <returns>Wallet address of connected client.</returns>
        /// <exception cref="Web3Exception">Throws exception if getting address fails.</exception>
        public Task<string> GetAddress()
        {
            if (!AddressExtensions.IsPublicAddress(address))
            {
                throw new Web3Exception(
                    $"Public address recovered from signature is not valid. Public address: {address}");
            }

            return Task.FromResult(address);
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignMessage"/>.
        /// Sign message using Wallet Connect.
        /// This prompts user to sign a message on a connected wallet.
        /// </summary>
        /// <param name="message">Message to sign.</param>
        /// <returns>Hash response of a successfully signed message.</returns>
        /// <exception cref="Web3Exception">Throws Exception if signing message fails.</exception>
        public async Task<string> SignMessage(string message)
        {
            var requestData = new EthSignMessage(message, address);

            string hash =
                await walletConnectCustomProvider.Request(requestData);

            var isValid = ValidateResponse(hash);
            if (!isValid)
            {
                throw new Web3Exception("Incorrect response format from signing.");
            }

            // TODO: log event on success
            return hash;

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignTypedData{TStructType}"/>.
        /// Sign Typed Data using wallet connect.
        /// </summary>
        /// <param name="domain">A serializable domain separator.</param>
        /// <param name="message">Data to be signed.</param>
        /// <typeparam name="TStructType">Data type of data to be signed.</typeparam>
        /// <returns>Hash response of a successfully signed typed data.</returns>
        /// <exception cref="Web3Exception">Throws Exception if signing typed data fails.</exception>
        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var requestData = new EthSignTypedData<TStructType>(address, domain, message);

            string hash =
                await walletConnectCustomProvider.Request(requestData);

            var isValid = ValidateResponse(hash);
            if (!isValid)
            {
                throw new Web3Exception("Incorrect response format extracted from clipboard.");
            }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }

            return hash;
        }
    }
}