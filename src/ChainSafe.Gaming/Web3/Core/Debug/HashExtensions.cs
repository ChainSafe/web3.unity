using System.Text;
using System.Text.RegularExpressions;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Nethereum.Util;

namespace ChainSafe.Gaming.Web3.Core.Debug
{
    public static class HashExtensions
    {
        public static bool IsSignatureHashValid(this string hash)
        {
            string regexPattern = @"^0x[a-fA-F0-9]{130}$";

            return !string.IsNullOrEmpty(hash) && Regex.IsMatch(hash, regexPattern);
        }

        public static bool IsSignatureValid(this string hash, string message, string account)
        {
            return IsSignatureHashValid(hash) && IsSignatureAuthentic(hash, message, account);
        }

        public static bool IsTypedDataSignatureValid<TMessage>(this string hash, SerializableTypedData<TMessage> typedData, string account)
        {
            return IsSignatureHashValid(hash) && IsTypedDataSignatureAuthentic(hash, typedData, account);
        }

        public static string AssertSignatureValid(this string hash, string message, string account)
        {
            if (!IsSignatureValid(hash, message, account))
            {
                throw new Web3AssertionException("Signature is not valid.");
            }

            return hash;
        }

        public static string AssertTypedDataSignatureValid<TMessage>(this string hash, SerializableTypedData<TMessage> typedData, string account)
        {
            if (!IsTypedDataSignatureValid(hash, typedData, account))
            {
                throw new Web3AssertionException("Typed Data Signature is not valid.");
            }

            return hash;
        }

        /// <summary>
        /// Check if signer account signed the message.
        /// </summary>
        /// <param name="hash">Hash of signed message.</param>
        /// <param name="message">Signed message.</param>
        /// <param name="account">Signer public account.</param>
        /// <returns>Did signer sign message.</returns>
        private static bool IsSignatureAuthentic(string hash, string message, string account)
        {
            if (!AddressExtensions.IsPublicAddress(account))
            {
                return false;
            }

            string messageToHash = "\x19" + "Ethereum Signed Message:\n" + message.Length + message;

            return IsSignatureBytesAuthentic(Encoding.UTF8.GetBytes(messageToHash), hash, account);
        }

        private static bool IsTypedDataSignatureAuthentic<TMessage>(string hash, SerializableTypedData<TMessage> typedData, string account)
        {
            if (!AddressExtensions.IsPublicAddress(account))
            {
                return false;
            }

            var rawTypedData = new TypedDataRaw
            {
                Types = typedData.Types,
                PrimaryType = typedData.PrimaryType,
                Message = MemberValueFactory.CreateFromMessage(typedData.Message),
                DomainRawValues = MemberValueFactory.CreateFromMessage(typedData.Domain),
            };

            return IsSignatureBytesAuthentic(Eip712TypedDataEncoder.Current.EncodeTypedDataRaw(rawTypedData), hash, account);
        }

        private static bool IsSignatureBytesAuthentic(byte[] bytes, string hash, string account)
        {
            EthECDSASignature signature = MessageSigner.ExtractEcdsaSignature(hash);

            byte[] messageHash = new Sha3Keccack().CalculateHash(bytes);

            var key = EthECKey.RecoverFromSignature(signature, messageHash);

            return key.GetPublicAddress().ToLower().Trim() == account.ToLower().Trim();
        }
    }
}