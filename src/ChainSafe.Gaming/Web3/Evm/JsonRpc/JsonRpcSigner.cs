using System.Threading.Tasks;
using System.Transactions;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using NBitcoin;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer.EIP712;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    public class JsonRpcSigner : ISigner, ILifecycleParticipant
    {
        private readonly JsonRpcSignerConfig config;
        private readonly IRpcProvider provider;
        private string address;

        public JsonRpcSigner(JsonRpcSignerConfig config, IRpcProvider provider)
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

        public async Task<string> SignTransaction(TransactionRequest tx)
        {
            return await provider.Perform<string>("eth_signTransaction", tx.ToRPCParam());
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