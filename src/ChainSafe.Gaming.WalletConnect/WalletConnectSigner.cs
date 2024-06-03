using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// WalletConnect implementation of <see cref="ISigner"/>.
    /// </summary>
    public class WalletConnectSigner : ISigner, ILifecycleParticipant, ILogoutHandler
    {
        private readonly IWalletProvider provider;

        public WalletConnectSigner(IWalletProvider provider)
        {
            this.provider = provider;
        }

        public string PublicAddress { get; private set; }

        public async ValueTask WillStartAsync()
        {
            PublicAddress = await provider.Connect();
        }

        ValueTask ILifecycleParticipant.WillStopAsync() => new(Task.CompletedTask);

        public Task OnLogout() => provider.Disconnect();

        public async Task<string> SignMessage(string message)
        {
            var hash = await provider.Perform<string>("personal_sign", message, PublicAddress);

            if (!ValidateSignResponse(hash))
            {
                throw new Web3Exception("Incorrect response format from signing.");
            }

            WCLogger.Log("Successfully signed message.");
            return hash;
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            var hash = await provider.Perform<string>("eth_signTypedData", PublicAddress, typedData);

            if (!ValidateSignResponse(hash))
            {
                throw new Web3Exception("Incorrect response format from signing.");
            }

            WCLogger.Log("Successfully signed typed data.");
            return hash;
        }

        private static bool ValidateSignResponse(string response)
        {
            // TODO: validate with regex
            return response.StartsWith("0x") && response.Length == 132;
        }
    }
}