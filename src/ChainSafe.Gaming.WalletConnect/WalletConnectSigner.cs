using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.WalletConnect.Methods;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Common.Model.Errors;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Models.Relay;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectSigner : ISigner, ILifecycleParticipant
    {
        private readonly IWalletConnectCustomProvider walletConnectCustomProvider;
        private readonly WalletConnectConfig config;

        public WalletConnectSigner(IWalletConnectCustomProvider walletConnectCustomProvider, WalletConnectConfig config)
        {
            this.walletConnectCustomProvider = walletConnectCustomProvider;
            this.config = config;
        }

        private string Address { get; set; }

        public async ValueTask WillStartAsync()
        {
            // if testing just don't initialize wallet connect
            if (config.Testing)
            {
                config.TestWalletAddress?.AssertIsPublicAddress(nameof(config.TestWalletAddress));

                Address = config.TestWalletAddress;

                return;
            }

            // get address by connecting
            Address = await walletConnectCustomProvider.Connect();
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public Task<string> GetAddress()
        {
            if (!AddressExtensions.IsPublicAddress(Address))
            {
                throw new Web3Exception(
                    $"Public address recovered from signature is not valid. Public address: {Address}");
            }

            return Task.FromResult(Address);
        }

        public async Task<string> SignMessage(string message)
        {
            var requestData = new EthSignMessage(message, Address);

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

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var requestData = new EthSignTypedData<TStructType>(Address, domain, message);

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