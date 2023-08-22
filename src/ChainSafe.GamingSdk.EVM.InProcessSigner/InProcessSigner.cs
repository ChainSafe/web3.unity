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
        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, Dictionary<string, MemberDescription[]> types, TStructType message)
        {
            var typedData = new TypedData<SerializableDomain>
            {
                PrimaryType = domain.Name,
                Domain = domain,
                Types = types,
                Message = MemberValueFactory.CreateFromMessage(message),
            };

            if (!typedData.Types.ContainsKey("EIP712Domain"))
            {
                var domain712 = new[]
                {
                    new MemberDescription { Name = "name", Type = "string" },
                    new MemberDescription { Name = "chainId", Type = "uint256" },
                };
                typedData.Types.Add("EIP712Domain", domain712);
            }

            return Task.FromResult(
                Eip712TypedDataSigner.Current.SignTypedData(typedData, privateKey));
        }

        public EthECKey GetKey() => privateKey;
    }
}
