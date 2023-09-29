using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;

namespace ChainSafe.Gaming.InProcessSigner
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

        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var primaryType = typeof(TStructType).Name;
            if (StructAttribute.IsStructType(message))
            {
                primaryType = StructAttribute.GetAttribute(message).Name;
            }

            var typedData = new TypedData<SerializableDomain>
            {
                PrimaryType = primaryType,
                Domain = domain,
                Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(SerializableDomain), typeof(TStructType)),
                Message = MemberValueFactory.CreateFromMessage(message),
            };
            return Task.FromResult(Eip712TypedDataSigner.Current.SignTypedDataV4(typedData, privateKey));
        }

        public EthECKey GetKey() => privateKey;
    }
}
