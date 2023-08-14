using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer.EIP712;
using Web3Unity.Scripts.Library.Ethers.Providers;
using TransactionRequest = Web3Unity.Scripts.Library.Ethers.Transactions.TransactionRequest;
using TransactionResponse = Web3Unity.Scripts.Library.Ethers.Transactions.TransactionResponse;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public class JsonRpcWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly JsonRpcWalletConfiguration configuration;
        private readonly IRpcProvider provider;
        private string address;

        public JsonRpcWallet(JsonRpcWalletConfiguration configuration, IRpcProvider provider)
        {
            this.configuration = configuration;
            this.provider = provider;
            address = this.configuration.AddressOverride;
        }

        public async ValueTask WillStartAsync()
        {
            if (string.IsNullOrEmpty(address))
            {
                address = await QueryAddress();
            }
        }

        public ValueTask WillStopAsync() => ValueTask.CompletedTask;

        public Task<string> GetAddress() => Task.FromResult(address);

        private async Task<string> QueryAddress()
        {
            var accounts = await provider.Perform<string[]>("eth_accounts");
            if (accounts.Length <= configuration.AccountIndex)
            {
                throw new Web3Exception($"No account with index #{configuration.AccountIndex} available");
            }

            return accounts[configuration.AccountIndex];
        }

        public async Task<string> SignMessage(byte[] message)
        {
            return await SignMessageImpl(message.ToHex());
        }

        public async Task<string> SignMessage(string message)
        {
            return await SignMessageImpl(message.ToHexUTF8());
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var hash = await SendUncheckedTransaction(transaction);

            try
            {
                var tx = await provider.GetTransaction(hash);
                return provider.WrapTransaction(tx, hash);
            }
            catch (Exception e)
            {
                throw new Exception($"failed to get transaction {hash}", e);
            }
        }

        private async Task<string> SendUncheckedTransaction(TransactionRequest transaction)
        {
            if (transaction.From == null)
            {
                var fromAddress = (await GetAddress()).ToLower();
                transaction.From = fromAddress;
            }

            if (transaction.GasLimit == null)
            {
                var feeData = await provider.GetFeeData();
                transaction.MaxFeePerGas = new HexBigInteger(feeData.MaxFeePerGas);
            }

            var rpcTxParams = transaction.ToRPCParam();
            return await provider.Perform<string>("eth_sendTransaction", rpcTxParams);
        }

        private async Task<string> SignMessageImpl(string hexMessage)
        {
            var adr = await GetAddress();
            return await provider.Perform<string>("personal_sign", hexMessage, adr.ToLower());
        }

        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, Dictionary<string, MemberDescription[]> types, TStructType message)
        {
            var data = Eip712TypedDataSigner.Current.EncodeTypedData(
                    new TypedData<SerializableDomain>
                    {
                        Domain = domain,
                        Types = types,
                        Message = MemberValueFactory.CreateFromMessage(message),
                    });
            return SignMessage(data);
        }
    }
}