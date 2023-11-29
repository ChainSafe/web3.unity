using System;
using System.Runtime.InteropServices;

namespace MetaMask.Cryptography
{
    public class UnityEciesProvider : IEciesProvider
    {
        #region Constants

#if (UNITY_IOS || UNITY_TVOS || UNITY_WEBGL) && !UNITY_EDITOR
        private const string libraryName = "__Internal";
#else
        private const string libraryName = "ecies";
#endif

        #endregion

        #region Fields

        protected static UnityEciesProvider instance;

        #endregion

        #region Properties

        /// <summary>Gets the singleton instance of the <see cref="UnityEciesProvider"/>.</summary>
        /// <returns>The singleton instance of the <see cref="UnityEciesProvider"/>.</returns>
        public static UnityEciesProvider Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnityEciesProvider();
                }

                return instance;
            }
        }

        #endregion

        #region Externals

        //
        // Imports from ECIES Go library
        //

#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>Initializes the ECIES library.</summary>
        [DllImport(libraryName)]
        private static extern void EciesInitialize();

        /// <summary>Generates a private key for ECIES.</summary>
        /// <returns>A private key for ECIES.</returns>
        [DllImport(libraryName)]
        private static extern string EciesGeneratePrivateKey();

        /// <summary>Gets the public key from a private key.</summary>
        /// <param name="privkey">The private key.</param>
        /// <returns>The public key.</returns>
        [DllImport(libraryName)]
        private static extern string EciesGetPublicKey(string privkey);

        /// <summary>Encrypts a message using ECIES.</summary>
        /// <param name="pubkey">The public key of the recipient.</param>
        /// <param name="message">The message to encrypt.</param>
        /// <returns>The encrypted message.</returns>
        [DllImport(libraryName)]
        private static extern string EciesEncrypt(string pubkey, string message);

        /// <summary>Decrypts a message using the ECIES scheme.</summary>
        /// <param name="privkey">The private key.</param>
        /// <param name="messageB64">The message, as a base64-encoded string.</param>
        /// <returns>The decrypted message.</returns>
        [DllImport(libraryName)]
        private static extern string EciesDecrypt(string privkey, string messageB64);
#else

        /// <summary>Generates a new ECIES private key.</summary>
        /// <returns>A pointer to the generated private key.</returns>
        [DllImport(libraryName)]
        private static extern IntPtr EciesGeneratePrivateKey();

        /// <summary>Gets the public key from a private key.</summary>
        /// <param name="privkey">The private key.</param>
        /// <returns>The public key.</returns>
        [DllImport(libraryName)]
        private static extern IntPtr EciesGetPublicKey(string privkey);

        /// <summary>Encrypts a message using ECDH-ES.</summary>
        /// <param name="pubkey">The public key of the recipient.</param>
        /// <param name="message">The message to encrypt.</param>
        /// <returns>The encrypted message.</returns>
        [DllImport(libraryName)]
        private static extern IntPtr EciesEncrypt(string pubkey, string message);

        /// <summary>Decrypts a message using the ECDH-ES scheme.</summary>
        /// <param name="privkey">The private key.</param>
        /// <param name="messageB64">The message, as a base64-encoded string.</param>
        /// <returns>The decrypted message, as a base64-encoded string.</returns>
        [DllImport(libraryName)]
        private static extern IntPtr EciesDecrypt(string privkey, string messageB64);
#endif

        #endregion

        #region Constructors

        /// <summary>Initializes the ECCIES provider.</summary>
        protected UnityEciesProvider()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            EciesInitialize();
#endif
        }

        #endregion

        #region Public Methods

        /// <summary>Decrypts a string using the specified private key.</summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string encryptedText, string privateKey)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return EciesDecrypt(privateKey, encryptedText);
#else
            return Marshal.PtrToStringAnsi(EciesDecrypt(privateKey, encryptedText));
#endif
        }

        /// <summary>Encrypts a string using the specified public key.</summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <param name="publickey">The public key to use for encryption.</param>
        /// <returns>The encrypted string.</returns>
        public string Encrypt(string plainText, string publickey)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return EciesEncrypt(publickey, plainText);
#else
            return Marshal.PtrToStringAnsi(EciesEncrypt(publickey, plainText));
#endif
        }

        /// <summary>Generates a private key for use with the ECIES algorithm.</summary>
        /// <returns>A private key.</returns>
        public string GeneratePrivateKey()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return EciesGeneratePrivateKey();
#else
            return Marshal.PtrToStringAnsi(EciesGeneratePrivateKey());
#endif
        }

        /// <summary>Gets the public key from a private key.</summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The public key.</returns>
        public string GetPublicKey(string privateKey)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return EciesGetPublicKey(privateKey);
#else
            return Marshal.PtrToStringAnsi(EciesGetPublicKey(privateKey));
#endif
        }

        #endregion
    }
}