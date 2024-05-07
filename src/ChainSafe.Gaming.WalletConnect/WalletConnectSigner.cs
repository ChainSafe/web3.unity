using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// WalletConnect implementation of <see cref="ISigner"/>.
    /// </summary>
    public class WalletConnectSigner : ISigner, ILifecycleParticipant, ILogoutHandler
    {
        private readonly IWalletConnectProvider provider;

        public WalletConnectSigner(IWalletConnectProvider provider)
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
            var requestData = new EthSignMessage(message, PublicAddress);
            var hash = await provider.Request(requestData);

            if (!ValidateSignResponse(hash))
            {
                throw new Web3Exception("Incorrect response format from signing.");
            }

            WCLogger.Log("Successfully signed message.");
            return hash;
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var requestData = new EthSignTypedData<TStructType>(PublicAddress, domain, message);
            var hash = await provider.Request(requestData);

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