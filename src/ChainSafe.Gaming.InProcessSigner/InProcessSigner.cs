using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.RPC.Accounts;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.InProcessSigner
{
    /// <summary>
    /// Concrete Implementation of <see cref="ISigner"/> that uses a Private Key to sign messages and typed data.
    /// </summary>
    public class InProcessSigner : ISigner
    {
        private readonly IAccountProvider accountProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessSigner"/> class.
        /// </summary>
        /// <exception cref="Web3Exception">Throws Exception if initializing instance fails.</exception>
        public InProcessSigner(IAccountProvider accountProvider)
        {
            this.accountProvider = accountProvider;
        }

        private IAccount Account => accountProvider.Account;

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
            string hash = await Account.AccountSigningService.PersonalSign.SendRequestAsync(Encoding.UTF8.GetBytes(message));

            return hash.AssertSignatureValid(message, PublicAddress);
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
            SerializableTypedData<TStructType> typedData = new SerializableTypedData<TStructType>(domain, message);

            string hash = await Account.AccountSigningService.SignTypedDataV4.SendRequestAsync(JsonConvert.SerializeObject(typedData));

            return hash.AssertTypedDataSignatureValid(typedData, PublicAddress);
        }
    }
}
