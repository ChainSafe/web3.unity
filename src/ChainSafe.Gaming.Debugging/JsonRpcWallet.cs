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

        public JsonRpcWallet(JsonRpcWalletConfig config, IRpcProvider provider)
        {
            this.config = config;
            this.provider = provider;
            PublicAddress = this.config.AddressOverride;
        }

        public string PublicAddress { get; private set; }

        public async ValueTask WillStartAsync()
        {
            if (string.IsNullOrEmpty(PublicAddress))
            {
                PublicAddress = await QueryAddress();
            }
        }

        public ValueTask WillStopAsync() => new ValueTask(Task.CompletedTask);

        private async Task<string> QueryAddress()
        {
            var accounts = await provider.Perform<string[]>("eth_accounts");
            if (accounts.Length <= config.AccountIndex)
            {
                throw new WalletException($"No account with index #{config.AccountIndex} available");
            }

            return accounts[config.AccountIndex];
        }

        public Task<string> SignMessage(byte[] message)
        {
            return SignMessageImpl(message.ToHex());
        }

        public Task<string> SignMessage(string message)
        {
            return SignMessageImpl(message.ToHexUTF8());
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
                var fromAddress = PublicAddress.ToLower();
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

        private Task<string> SignMessageImpl(string hexMessage)
        {
            return provider.Perform<string>("personal_sign", hexMessage, PublicAddress.ToLower());
        }

        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var typedData = new TypedData<SerializableDomain>
            {
                PrimaryType = nameof(TStructType),
                Domain = domain,
                Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(Domain), typeof(TStructType)),
                Message = MemberValueFactory.CreateFromMessage(message),
            };

            var data = Eip712TypedDataSigner.Current.EncodeTypedData(typedData);

            return provider.Perform<string>("eth_signTypedData_v4", PublicAddress.ToLower(), data);
        }
    }
}