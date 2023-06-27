using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.Web3AuthWallet
{
    public delegate string ConnectMessageBuildDelegate(DateTime expirationTime);

    public class Web3AuthWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly Web3AuthWalletConfig configuration;
        private readonly IRpcProvider provider;

        // private string? address;

        // private string? address;
        public Web3AuthWallet(
            IRpcProvider provider,
            Web3AuthWalletConfig configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public ValueTask Connect()
        {
            if (Connected)
            {
                throw new Web3Exception("Signer already connected.");
            }

            Connected = true;
            return default;
        }

        public Task<string> GetAddress()
        {
            if (!Connected)
            {
                throw new Web3Exception(
                    $"Can't retrieve public address. {nameof(Web3AuthWallet)} is not connected yet.");
            }

            string address = new EthECKey(configuration?.PrivateKey).GetPublicAddress();
            return Task.FromResult(address);
        }

        public Task<string> SignMessage(string message) =>
            Task.FromResult(new EthereumMessageSigner().HashAndSign(
                message,
                configuration?.PrivateKey));

        public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
        }

        public ValueTask WillStartAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask WillStopAsync()
        {
            throw new NotImplementedException();
        }
    }
}