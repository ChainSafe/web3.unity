using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.InProcessSigner
{
    /// <summary>
    /// Concrete Implementation of <see cref="ISigner"/> that uses a Private Key to sign messages and typed data.
    /// </summary>
    public class InProcessSigner : ISigner
    {
        private readonly AccountProvider accountProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessSigner"/> class.
        /// </summary>
        /// <exception cref="Web3Exception">Throws Exception if initializing instance fails.</exception>
        public InProcessSigner(AccountProvider accountProvider)
        {
            this.accountProvider = accountProvider;
        }

        private Account Account => accountProvider.Account;

        /// <summary>
        /// Public Address.
        /// </summary>
        /// <value>Public address of signer.</value>
        public string PublicAddress => Account.Address;

        /// <summary>
        /// Implementation of <see cref="ISigner.SignMessage"/> using In Process.
        /// </summary>
        /// <param name="message">Message to be signed.</param>
        /// <returns>Hash response of a successfully signed message.</returns>
        public async Task<string> SignMessage(string message)
        {
            var byteList = new List<byte>();
            var bytePrefix = "0x19".HexToByteArray();
            var textBytePrefix = Encoding.UTF8.GetBytes("Ethereum Signed Message:\n" + message.Length);

            byteList.AddRange(bytePrefix);
            byteList.AddRange(textBytePrefix);
            byteList.AddRange(Encoding.UTF8.GetBytes(message));
            var hash = new Sha3Keccack().CalculateHash(byteList.ToArray());

            return await Account.AccountSigningService.PersonalSign.SendRequestAsync(hash);
        }

        /// <summary>
        /// Implementation of <see cref="ISigner.SignTypedData{TStructType}"/> using In Process.
        /// </summary>
        /// <param name="domain">Serializable domain for signing typed data.</param>
        /// <param name="message">Message/Data to be signed.</param>
        /// <typeparam name="TStructType">Type of Data to be signed.</typeparam>
        /// <returns>Hash response of a successfully signed typed data.</returns>
        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
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

            return await Account.AccountSigningService.SignTypedDataV4.SendRequestAsync(JsonConvert.SerializeObject(typedData));
        }
    }
}
