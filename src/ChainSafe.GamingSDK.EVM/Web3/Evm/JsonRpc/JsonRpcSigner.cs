using System;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using TransactionRequest = Web3Unity.Scripts.Library.Ethers.Transactions.TransactionRequest;
using TransactionResponse = Web3Unity.Scripts.Library.Ethers.Transactions.TransactionResponse;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public class JsonRpcSigner : BaseSigner
    {
        private readonly JsonRpcSignerConfiguration configuration;

        private string address;
        private bool connected;

        public JsonRpcSigner(JsonRpcSignerConfiguration configuration, IEvmProvider provider)
            : base(provider)
        {
            this.configuration = configuration;
            address = this.configuration.AddressOverride;
        }

        public override bool Connected => connected;

        public override async ValueTask Connect()
        {
            // simply check connection and cache address in case of JsonRpc
            address = await GetAddress();
            connected = true;
        }

        public override async Task<string> GetAddress()
        {
            if (address != null)
            {
                return address;
            }

            var accounts = await Provider.Perform<string[]>("eth_accounts");
            if (accounts.Length <= configuration.AccountIndex)
            {
                throw new Web3Exception($"No account with index #{configuration.AccountIndex} available");
            }

            return accounts[configuration.AccountIndex];
        }

        public override async Task<string> SignMessage(byte[] message)
        {
            return await SignMessageImpl(message.ToHex());
        }

        public override async Task<string> SignMessage(string message)
        {
            return await SignMessageImpl(message.ToHexUTF8());
        }

        public override async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var hash = await SendUncheckedTransaction(transaction);
            transaction.From = await GetAddress();

            try
            {
                var tx = await Provider.GetTransaction(hash);
                return Provider.WrapTransaction(tx, hash);
            }
            catch (Exception e)
            {
                throw new Exception($"failed to get transaction {hash}", e);
            }
        }

        public async Task<string> SendUncheckedTransaction(TransactionRequest transaction)
        {
            var fromAddress = (await GetAddress()).ToLower();

            if (transaction.GasLimit == null)
            {
                var estimate = (TransactionRequest)transaction.Clone();
                estimate.From = fromAddress;
                transaction.GasLimit = await Provider.EstimateGas(transaction);
                transaction.GasLimit = new HexBigInteger(transaction.GasLimit.Value * 2);
            }

            if (transaction.From != null)
            {
                if (transaction.From.ToLower() != fromAddress)
                {
                    throw new Exception("from address mismatch");
                }
            }
            else
            {
                transaction.From = fromAddress;
            }

            var rpcTxParams = transaction.ToRPCParam();
            return await Provider.Perform<string>("eth_sendTransaction", rpcTxParams);
        }

        private async Task<string> SignMessageImpl(string hexMessage)
        {
            var address = await GetAddress();
            return await Provider.Perform<string>("personal_sign", hexMessage, address.ToLower());
        }

        public async Task<string> LegacySignMessage(byte[] message)
        {
            return await LegacySignMessageImpl(message.ToHex(true));
        }

        public async Task<string> LegacySignMessage(string message)
        {
            return await LegacySignMessageImpl(message.ToHexUTF8());
        }

        private async Task<string> LegacySignMessageImpl(string hexMessage)
        {
            var address = await GetAddress();
            return await Provider.Perform<string>("eth_sign", address.ToLower(), hexMessage);
        }
    }
}