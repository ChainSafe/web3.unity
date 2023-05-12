using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public class JsonRpcSigner : BaseSigner
    {
        private readonly JsonRpcProvider provider;
        private int _index;
        private string _address;

        public JsonRpcSigner(JsonRpcProvider provider, string address) : base(provider)
        {
            this.provider = provider;
            _address = address;
        }

        public JsonRpcSigner(JsonRpcProvider provider, int index) : base(provider)
        {
            this.provider = provider;
            _index = index;
        }

        public override ISigner Connect(IProvider provider)
        {
            throw new Exception("cannot alter JSON-RPC Signer connection");
        }

        public override async Task<string> GetAddress()
        {
            if (_address != null) return await Task.Run(() => _address);

            var accounts = await provider.Send<string[]>("eth_accounts", null);
            if (accounts.Length <= _index) throw new Exception($"unknown account #{_index}");
            return accounts[_index];
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
                var tx = await _provider.GetTransaction(hash);
                return this.provider._wrapTransaction(tx, hash);
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
                transaction.GasLimit = (await _provider.EstimateGas(transaction));
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
            return await provider.Send<string>("eth_sendTransaction", new object[] { rpcTxParams });
        }

        private async Task<string> _signMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await provider.Send<string>("personal_sign", new object[] { hexMessage, address.ToLower() });
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
            return await provider.Send<string>("eth_sign", new object[] { address.ToLower(), hexMessage });
        }
    }
}