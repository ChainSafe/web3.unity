using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Web3Unity.Scripts.Library.Ethers.Providers;

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

        private async Task<string> _signMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await provider.Send<string>("personal_sign", new object[] {hexMessage, address.ToLower()});
        }

        public async Task<string> _LegacySignMessage(byte[] message)
        {
            return await _legacySignMessage(message.ToHex());
        }

        public async Task<string> _LegacySignMessage(string message)
        {
            return await _legacySignMessage(message.ToHexUTF8());
        }

        private async Task<string> _legacySignMessage(string hexMessage)
        {
            var address = await GetAddress();
            return await provider.Send<string>("eth_sign", new object[] {address.ToLower(), hexMessage});
        }
    }
}