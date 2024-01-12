using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectSignerNew : ISigner, ILifecycleParticipant
    {
        private readonly IWalletConnectProviderNew provider;

        private string address;

        public WalletConnectSignerNew(IWalletConnectProviderNew provider)
        {
            this.provider = provider;
        }

        public async ValueTask WillStartAsync()
        {
            address = await provider.Connect();
        }

        // todo Rework Web3Accessor to manage Web3's state and to ensure Web3.Terminate() is always called.
        async ValueTask ILifecycleParticipant.WillStopAsync()
        {
            await provider.Disconnect();
        }

        public Task<string> GetAddress() => Task.FromResult(address);

        public async Task<string> SignMessage(string message)
        {
            var requestData = new EthSignMessage(message, address);
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
            var requestData = new EthSignTypedData<TStructType>(address, domain, message);
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