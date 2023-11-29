using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using MetaMask.IO;
using MetaMask.Logging;

using Newtonsoft.Json;

namespace MetaMask
{

    /// <summary>
    /// Manages persistent data for MetaMask, allows custom storages to be used, the serialization is based on JSON.
    /// </summary>
    public class MetaMaskDataManager
    {

        #region Fields

        protected bool encrypt = true;
        protected string password;
        protected IMetaMaskPersistentStorage storage;
        protected int saltSize = 12;
        protected int iterations = 10000;

        protected string algorithmName = "aes";

        protected PaddingMode paddingMode = PaddingMode.PKCS7;
        protected CipherMode cipherMode = CipherMode.CBC;

        protected byte[] iv;
        protected byte[] key;

        protected SymmetricAlgorithm algorithm;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether to encrypt the data.
        /// </summary>
        public bool Encrypt
        {
            get => this.encrypt;
            set => this.encrypt = value;
        }

        /// <summary>
        /// Gets or sets the password for encryption
        /// </summary>
        public string Password
        {
            get => this.password;
            set => this.password = value;
        }

        /// <summary>
        /// Gets the storage provider.
        /// </summary>
        public IMetaMaskPersistentStorage Storage => this.storage;

        /// <summary>
        /// Gets the symmetric cryptography algorithm.
        /// </summary>
        public virtual SymmetricAlgorithm Algorithm
        {
            get
            {
                if (this.algorithm == null)
                {
                    if (string.IsNullOrEmpty(this.algorithmName))
                    {
                        this.algorithmName = "aes";
                    }

                    this.algorithm = SymmetricAlgorithm.Create(this.algorithmName);
                    this.algorithm.Mode = this.cipherMode;
                    this.algorithm.Padding = this.paddingMode;
                }
                return this.algorithm;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskDataManager"/>.
        /// </summary>
        /// <param name="storage">The storage provider to use</param>
        public MetaMaskDataManager(IMetaMaskPersistentStorage storage, bool encrypt, string password = null)
        {
            this.storage = storage;
            this.encrypt = encrypt;
            if (string.IsNullOrEmpty(password))
            {
                this.password = RandomString(12);
            }
            else
            {
                this.password = password;
            }
        }

        #endregion

        #region Public Methods

        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.storage.Delete(key);
        }

        /// <summary>
        /// Saves the <paramref name="value"/> by serializing it and then storing it to the <paramref name="key"/> inside the storage.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value to serialize and save</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> or <paramref name="value"/> is null or empty</exception>
        public void Save(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Serialize
            var json = JsonConvert.SerializeObject(value);
            string data = json;

            // Encrypt
            if (this.encrypt)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                using (var stream = new MemoryStream())
                {
                    using (var cryptoStream = GetCryptoStream(stream, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }
                    data = Convert.ToBase64String(stream.ToArray());
                }
            }
            this.storage.Write(key, data);
        }

        /// <summary>
        /// Loads the value from the <paramref name="key"/> from the storage if it exists.
        /// </summary>
        /// <typeparam name="T">The type of the value to load</typeparam>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value to return if the value or key does not exists</param>
        /// <returns>Returns the loaded value, otherwise <paramref name="defaultValue"/> if it is set, otherwise it can be null for nullable types or default(T) for value types.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is null or empty</exception>
        public T Load<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (this.storage.Exists(key))
            {
                var loadedData = this.storage.Read(key);
                if (string.IsNullOrEmpty(loadedData))
                {
                    return defaultValue;
                }

                string data = loadedData;

                // Decrypt
                try
                {
                    var encryptedBytes = Convert.FromBase64String(loadedData);
                    var dataBytes = new byte[encryptedBytes.Length];
                    using var stream = new MemoryStream(encryptedBytes);
                    using var cryptoStream = GetCryptoStream(stream, CryptoStreamMode.Read);
                    cryptoStream.Read(dataBytes, 0, dataBytes.Length);
                    data = Encoding.UTF8.GetString(dataBytes);
                }
                catch (Exception e)
                {
                    MetaMaskDebug.LogException(e);
                    data = loadedData;
                }

                // Deserialize
                T result;
                try
                {
                    result = JsonConvert.DeserializeObject<T>(data);
                }
                catch (Exception ex)
                {
                    MetaMaskDebug.LogWarning("The decryption has failed, probably the password is incorrect, or the data is not encrypted, trying to load without decryption now, this can happen whenever you change the encryption password or enable/disable encryption.");
                    MetaMaskDebug.LogException(ex);
                    return defaultValue;
                }
                if (result == null)
                {
                    return defaultValue;
                }

                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Loads the value from the <paramref name="key"/> from the storage if it exists to the given object <paramref name="instance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object instance</typeparam>
        /// <param name="key">The key</param>
        /// <param name="instance">The object's instance to load the data into</param>
        /// <returns>Returns the loaded object instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is null or empty</exception>
        public T LoadInto<T>(string key, T instance)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (this.storage.Exists(key))
            {
                var loadedData = this.storage.Read(key);
                if (string.IsNullOrEmpty(loadedData))
                {
                    MetaMaskDebug.Log("Storage reported empty data for key " + key);
                    return instance;
                }

                string data = loadedData;

                // Decrypt
                try
                {
                    var encryptedBytes = Convert.FromBase64String(loadedData);
                    var dataBytes = new byte[encryptedBytes.Length];
                    using var stream = new MemoryStream(encryptedBytes);
                    using var cryptoStream = GetCryptoStream(stream, CryptoStreamMode.Read);
                    cryptoStream.Read(dataBytes, 0, dataBytes.Length);
                    data = Encoding.UTF8.GetString(dataBytes);
                }
                catch (Exception ex)
                {
                    MetaMaskDebug.LogWarning("The decryption has failed, probably the password is incorrect, or the data is not encrypted, trying to load without decryption now, this can happen whenever you change the encryption password or enable/disable encryption.");
                    MetaMaskDebug.LogException(ex);
                    data = loadedData;
                }

                // Deserialize
                try
                {
                    JsonConvert.PopulateObject(data, instance);
                }
                catch (Exception e)
                {
                    MetaMaskDebug.LogException(e);
                }

                return instance;
            }
            else
            {
                MetaMaskDebug.Log("Storage reported no key " + key);
            }
            return instance;
        }

        #endregion

        #region Cryptography Methods

        /// <summary>
        /// Prepares the encryption Key and IV.
        /// </summary>
        /// <returns>Returns the salt calculated during the creation of Key and IV</returns>
        public virtual byte[] PrepareEncryptPassword()
        {
            using (var rfc = new Rfc2898DeriveBytes(this.password, this.saltSize, this.iterations))
            {
                this.key = rfc.GetBytes(Algorithm.KeySize / 8);
                this.iv = rfc.GetBytes(Algorithm.BlockSize / 8);
                return rfc.Salt;
            }
        }

        /// <summary>
        /// Prepares the decryption Key and IV.
        /// </summary>
        /// <param name="salt">The salt</param>
        public virtual void PrepareDecryptPassword(byte[] salt)
        {
            using (var rfc = new Rfc2898DeriveBytes(this.password, salt, this.iterations))
            {
                this.key = rfc.GetBytes(Algorithm.KeySize / 8);
                this.iv = rfc.GetBytes(Algorithm.BlockSize / 8);
            }
        }

        /// <summary>
        /// Gets a <see cref="CryptoStream"/> for the <paramref name="stream"/> using the specified <paramref name="mode"/> with symmetric encryption algorithm.
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <param name="mode">The read or write mode</param>
        /// <returns>Returns a <see cref="CryptoStream"/> for the given <paramref name="stream"/></returns>
        public virtual CryptoStream GetCryptoStream(Stream stream, CryptoStreamMode mode)
        {
            this.algorithm = null;
            CryptoStream cryptoStream;
            if (mode == CryptoStreamMode.Read)
            {
                var salt = new byte[this.saltSize];
                stream.Read(salt, 0, salt.Length);
                PrepareDecryptPassword(salt);
                this.algorithm.Key = this.key;
                this.algorithm.IV = this.iv;
                cryptoStream = new CryptoStream(stream, Algorithm.CreateDecryptor(this.key, this.iv), CryptoStreamMode.Read);
            }
            else
            {
                var salt = PrepareEncryptPassword();
                stream.Write(salt, 0, salt.Length);
                this.algorithm.Key = this.key;
                this.algorithm.IV = this.iv;
                cryptoStream = new CryptoStream(stream, Algorithm.CreateEncryptor(this.key, this.iv), CryptoStreamMode.Write);
            }
            return cryptoStream;
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>Returns a randomly generated string</returns>
        public static string RandomString(int length)
        {
            var random = new System.Random();
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[random.Next(valid.Length)]);
            }
            return res.ToString();
        }

        #endregion

    }
}