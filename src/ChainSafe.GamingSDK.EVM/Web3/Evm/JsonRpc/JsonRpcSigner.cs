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
        private readonly JsonRpcSignerConfiguration _configuration;

        private string _address;
        private bool _connected;

        public JsonRpcSigner(JsonRpcSignerConfiguration configuration, IEvmProvider provider) : base(provider)
        {
            _configuration = configuration;
            _address = _configuration.AddressOverride;
        }

        public override async Task<string> GetAddress()
        {
            if (_address != null) return _address;

            var accounts = await Provider.Perform<string[]>("eth_accounts");
            if (accounts.Length <= _configuration.AccountIndex)
            {
                throw new Web3Exception($"No account with index #{_configuration.AccountIndex} available");
            }

            return accounts[_configuration.AccountIndex];
        }

        public override async Task<string> SignMessage(byte[] message)
        {
            return await _signMessage(message.ToHex());
        }

        public override async Task<string> SignMessage(string message)
        {
            return await _signMessage(message.ToHexUTF8());
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
                transaction.GasLimit = (await Provider.EstimateGas(transaction));
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

        private async Task<string> _signMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await Provider.Perform<string>("personal_sign", hexMessage, address.ToLower());
        }

        public async Task<string> _LegacySignMessage(byte[] message)
        {
            return await _legacySignMessage(message.ToHex(true));
        }

        public async Task<string> _LegacySignMessage(string message)
        {
            return await _legacySignMessage(message.ToHexUTF8());
        }

        private async Task<string> _legacySignMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await Provider.Perform<string>("eth_sign", address.ToLower(), hexMessage);
        }

        public override bool Connected => _connected;

        public override async ValueTask Connect()
        {
            // simply check connection and cache address in case of JsonRpc
            _address = await GetAddress();
            _connected = true;
        }
    }
}