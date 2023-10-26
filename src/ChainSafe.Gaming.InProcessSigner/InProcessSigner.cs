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
    /// <summary>
    /// Concrete Implementation of <see cref="ISigner"/> that uses a Private Key to sign messages and typed data.
    /// </summary>
    public class InProcessSigner : ISigner
    {
        private EthECKey privateKey;
        private EthereumMessageSigner messageSigner;

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessSigner"/> class.
        /// </summary>
        /// <param name="config">Injected Config for signer containing a private key.</param>
        /// <exception cref="Web3Exception">Throws Exception if initializing instance fails.</exception>
        public InProcessSigner(InProcessSignerConfig config)
        {
            privateKey = config.PrivateKey ??
                throw new Web3Exception($"{nameof(InProcessSignerConfig)}.{nameof(InProcessSignerConfig.PrivateKey)} must be set");
            messageSigner = new();
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.GetAddress"/> using In Process.
        /// </summary>
        /// <returns>Public address of signer.</returns>
        public Task<string> GetAddress() => Task.FromResult(privateKey.GetPublicAddress());

        /// <summary>
        /// Implementation of <see cref="ISigner.SignMessage"/> using In Process.
        /// </summary>
        /// <param name="message">Message to be signed.</param>
        /// <returns>Hash response of a successfully signed message.</returns>
        public Task<string> SignMessage(string message) =>
            Task.FromResult(messageSigner.EncodeUTF8AndSign(message, privateKey));

        /// <summary>
        /// Implementation of <see cref="ISigner.SignTypedData{TStructType}"/> using In Process.
        /// </summary>
        /// <param name="domain">Serializable domain for signing typed data.</param>
        /// <param name="message">Message/Data to be signed.</param>
        /// <typeparam name="TStructType">Type of Data to be signed.</typeparam>
        /// <returns>Hash response of a successfully signed typed data.</returns>
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

        /// <summary>
        /// Get private key of <see cref="ISigner"/>.
        /// </summary>
        /// <returns>Private key of <see cref="ISigner"/>.</returns>
        public EthECKey GetKey() => privateKey;
    }
}
