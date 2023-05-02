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
        private readonly int index;
        private readonly string address;

        public JsonRpcSigner(JsonRpcProvider provider, string address)
            : base(provider)
        {
            this.provider = provider;
            this.address = address;
        }

        public JsonRpcSigner(JsonRpcProvider provider, int index)
            : base(provider)
        {
            this.provider = provider;
            this.index = index;
        }

        public override ISigner Connect(IProvider provider)
        {
            throw new Exception("cannot alter JSON-RPC Signer connection");
        }

        public override async Task<string> GetAddress()
        {
            if (address != null)
            {
                return await Task.Run(() => address);
            }

            var accounts = await provider.Send<string[]>("eth_accounts", null);
            if (accounts.Length <= index)
            {
                throw new Exception($"unknown account #{index}");
            }

            return accounts[index];
        }

        public override async Task<string> SignMessage(byte[] message)
        {
            return await SignHexMessage(message.ToHex());
        }

        public override async Task<string> SignMessage(string message)
        {
            return await SignHexMessage(message.ToHexUTF8());
        }

        public override async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var hash = await SendUncheckedTransaction(transaction);
            transaction.From = await GetAddress();

            try
            {
                var tx = await Provider.GetTransaction(hash);
                return provider.WrapTransaction(tx, hash);
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
            return await provider.Send<string>("eth_sendTransaction", new object[] { rpcTxParams });
        }

        private async Task<string> SignHexMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await provider.Send<string>("personal_sign", new object[] { hexMessage, address.ToLower() });
        }

        public async Task<string> LegacySignMessage(byte[] message)
        {
            return await LegacySignHexMessage(message.ToHex(true));
        }

        public async Task<string> LegacySignMessage(string message)
        {
            return await LegacySignHexMessage(message.ToHexUTF8());
        }

        private async Task<string> LegacySignHexMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await provider.Send<string>("eth_sign", new object[] { address.ToLower(), hexMessage });
        }
    }
}