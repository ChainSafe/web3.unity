using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer.EIP712;

namespace ChainSafe.Gaming.Wallets
{
    public class JsonRpcWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly JsonRpcWalletConfig config;
        private readonly IRpcProvider provider;
        private string address;

        public JsonRpcWallet(JsonRpcWalletConfig config, IRpcProvider provider)
        {
            this.config = config;
            this.provider = provider;
            address = this.config.AddressOverride;
        }

        public async ValueTask WillStartAsync()
        {
            if (string.IsNullOrEmpty(address))
            {
                address = await QueryAddress();
            }
        }

        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);

        public Task<string> GetAddress() => Task.FromResult(address);

        private async Task<string> QueryAddress()
        {
            var accounts = await provider.Perform<string[]>("eth_accounts");
            if (accounts.Length <= config.AccountIndex)
            {
                throw new Web3Exception($"No account with index #{config.AccountIndex} available");
            }

            return accounts[config.AccountIndex];
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

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var typedData = new TypedData<SerializableDomain>
            {
                PrimaryType = nameof(TStructType),
                Domain = domain,
                Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(Domain), typeof(TStructType)),
                Message = MemberValueFactory.CreateFromMessage(message),
            };

            var data = Eip712TypedDataSigner.Current.EncodeTypedData(typedData);

            var adr = await GetAddress();
            return await provider.Perform<string>("eth_signTypedData_v4", adr.ToLower(), data);
        }
    }
}