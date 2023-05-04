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
    public class JsonRpcSigner : BaseSigner, IEvmWallet
    {
        private string _address;
        private readonly JsonRpcSignerConfiguration _configuration;

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

        public bool Connected { get; private set; }
    
        public async ValueTask Connect()
        {
            // simply cache address in this case
            _address = await GetAddress();
            Connected = true;
        }
    }
}