using System.IO;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Newtonsoft.Json;
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
                Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(Domain), typeof(TStructType)),
                Message = MemberValueFactory.CreateFromMessage(message),
            };

            var sb = new StringBuilder();
            sb.Append($"{JsonConvert.SerializeObject(typedData.Domain)}\n");
            sb.Append($"{JsonConvert.SerializeObject(typedData.Types)}\n");
            sb.Append($"{JsonConvert.SerializeObject(typedData.Message)}\n");
            sb.AppendLine($"PrimaryType: {primaryType}");
            File.AppendAllText("gaming-signature.json", sb.ToString());
            sb.Clear();

            var sig = Eip712TypedDataSigner.Current.SignTypedDataV4(typedData, privateKey);

            return Task.FromResult(sig);
        }

        public EthECKey GetKey() => privateKey;
    }
}
