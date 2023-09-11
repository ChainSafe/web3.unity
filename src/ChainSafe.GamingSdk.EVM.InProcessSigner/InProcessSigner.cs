using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.EVM.InProcessSigner
{
    public class InProcessSigner : ISigner
    {
        private EthECKey privateKey;
        private EthereumMessageSigner messageSigner;

        public InProcessSigner(InProcessSignerConfig config)
        {
            privateKey = config.PrivateKey ??
                throw new Web3Exception($"{nameof(InProcessSignerConfig)}.{nameof(InProcessSignerConfig.PrivateKey)} must be set");
            messageSigner = new();
        }

        public Task<string> GetAddress() => Task.FromResult(privateKey.GetPublicAddress());

        public Task<string> SignMessage(string message) =>
            Task.FromResult(messageSigner.EncodeUTF8AndSign(message, privateKey));

        // TODO: test this with Gelato
        public Task<string> SignTypedData<TStructType>(
            SerializableDomain domain, Dictionary<string, MemberDescription[]> types, string primaryType, TStructType message)
        {
            var typedData = new TypedData<SerializableDomain>
            {
                PrimaryType = primaryType,
                Domain = domain,
                Types = types,
                Message = MemberValueFactory.CreateFromMessage(message),
            };

            if (!typedData.Types.ContainsKey(DomainSeparator.DomainName))
            {
                typedData.Types.Add(DomainSeparator.DomainName, DomainSeparator.Eip712Domain);
            }

            return Task.FromResult(
                Eip712TypedDataSigner.Current.SignTypedData(typedData, privateKey));
        }

        public EthECKey GetKey() => privateKey;
    }
}
