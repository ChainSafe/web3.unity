namespace MetaMask.Cryptography
{

    /// <summary>
    /// An interface for ECIES providers.
    /// </summary>
    public interface IEciesProvider
    {

        /// <summary>
        /// Generates key-pair and return the asymmetric private key
        /// </summary>
        /// <returns>Return the asymmetric private key</returns>
        string GeneratePrivateKey();

        /// <summary>
        /// Returns public key in base64 from key-pair
        /// </summary>
        /// <param name="privateKey">The asymmetric private key</param>
        /// <returns>Returns public key in base64 from key-pair</returns>
        string GetPublicKey(string privateKey);

        /// <summary>
        /// Encrypts message using a public key and returns base64 encrypted string
        /// </summary>
        /// <param name="plainText">The plain text to encrypt</param>
        /// <param name="publickey">The public key</param>
        /// <returns>Returns base64 encrypted string</returns>
        string Encrypt(string plainText, string publickey);

        /// <summary>
        /// Decrypts base64 message using private key and returns plain text message
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt</param>
        /// <param name="privateKey">The private key</param>
        /// <returns>Returns plain text message</returns>
        string Decrypt(string encryptedText, string privateKey);

    }
}
