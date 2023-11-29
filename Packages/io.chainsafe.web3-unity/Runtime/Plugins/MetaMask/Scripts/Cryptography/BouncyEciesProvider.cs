namespace MetaMask.Cryptography
{

    public class BouncyEciesProvider : IEciesProvider
    {
        /// <summary>Gets the singleton instance of the BouncyEciesProvider class.</summary>
        protected static BouncyEciesProvider instance;

        /// <summary>Gets the singleton instance of the BouncyEciesProvider class.</summary>
        /// <returns>The singleton instance of the BouncyEciesProvider class.</returns>
        public static BouncyEciesProvider Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new BouncyEciesProvider();
                }
                return instance;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="BouncyEciesProvider"/> class.</summary>
        protected BouncyEciesProvider() { }

        /// <summary>Decrypts a string.</summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="NotImplementedException">Thrown when the function hasn't been implemented yet.</exception>
        public string Decrypt(string encryptedText, string privateKey)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Encrypts a string using the specified public key.</summary>        
        /// <param name="plainText">The string to encrypt.</param>        
        /// <param name="publickey">The public key to use for encryption.</param>        
        /// <returns>The encrypted string.</returns>        
        /// <exception cref="System.NotImplementedException">Thrown when the function is not implemented.</exception>        
        public string Encrypt(string plainText, string publickey)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Generates a private key.</summary>
        /// <returns>A private key.</returns>
        public string GeneratePrivateKey()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Gets the public key from a private key.</summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The public key.</returns>
        public string GetPublicKey(string privateKey)
        {
            throw new System.NotImplementedException();
        }

    }

}