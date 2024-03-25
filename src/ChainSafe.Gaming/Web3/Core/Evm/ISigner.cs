using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.Evm.Signers
{
    /// <summary>
    /// Represents a component responsible for signing transactions, messages, and providing the player's public address.
    /// </summary>
    /// <remarks>
    /// Signer serves as a secure and essential element that encapsulates a user's
    /// private key, enabling the signing of messages and transactions. Additionally, it provides
    /// the user's public address for identity verification on the blockchain network.
    /// </remarks>
    public interface ISigner
    {
        /// <summary>
        /// Retrieves the wallet address associated with the signer.
        /// </summary>
        /// <value>
        /// The wallet address associated with the signer as a string.
        /// </value>
        string PublicAddress { get; }

        /// <summary>
        /// Asynchronously signs a given message.
        /// </summary>
        /// <param name="message">The message to sign as a string.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// the signature of the message as a string.
        /// </returns>
        Task<string> SignMessage(string message);

        /// <summary>
        /// Asynchronously signs a structured message using the provided domain parameters.
        /// </summary>
        /// <typeparam name="TStructType">The type of the structured message to sign.</typeparam>
        /// <param name="domain">The domain parameters for signing.</param>
        /// <param name="message">The structured message to sign.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// the signature of the structured message as a string.
        /// </returns>
        Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message);

        // TODO: is this the right thing to do?
        // Task<string> SignMessage(byte[] message) => SignMessage(message.ToHex());
        // This needs to be refactored byte[] should be default data type and message
        // string should be adopted to the byte[]
    }
}