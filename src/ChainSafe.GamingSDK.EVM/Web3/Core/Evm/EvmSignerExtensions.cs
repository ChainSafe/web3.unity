using System;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public static class EvmSignerExtensions
    {
        public const int TimestampExpirationSeconds = 60;

        /// <summary>
        /// Signs timestamp, verifies signature, checks expiration time.
        /// </summary>
        /// <returns>User's public address.</returns>
        public static async Task<string> VerifyUserOwnsAccount(this IEvmSigner signer)
        {
            // sign current timestamp
            var timestamp = GetCurrentTimestamp();
            var expirationTime = timestamp + TimestampExpirationSeconds;
            var message = expirationTime.ToString();
            var signature = await signer.SignMessage(message);
            var publicAddress = VerifySignature(signature, message);

            if (publicAddress.Length != 42)
            {
                throw new Web3Exception(
                    $"Public address recovered from signature has length {publicAddress.Length}. Should be 42.");
            }

            var now = GetCurrentTimestamp();
            if (now > expirationTime)
            {
                throw new Web3Exception("Signature has already expired. Try signing again.");
            }

            return publicAddress;

            int GetCurrentTimestamp()
            {
                return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            }
        }

        /// <summary>
        /// Asserts that signer was not yet connected.
        /// </summary>
        /// <exception cref="Web3Exception">Thrown when signer is already connected.</exception>
        public static void AssertNotConnected(this IEvmSigner signer)
        {
            if (signer.Connected)
            {
                throw new Web3Exception("Signer already connected.");
            }
        }

        private static string VerifySignature(string signature, string originalMessage)
        {
            var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
            var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
            var ecdsaSignature = MessageSigner.ExtractEcdsaSignature(signature);
            var key = EthECKey.RecoverFromSignature(ecdsaSignature, msgHash);
            return key.GetPublicAddress();
        }
    }
}