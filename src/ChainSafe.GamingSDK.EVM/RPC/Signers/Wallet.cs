// TODO: Finish implementation

/*
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using Org.BouncyCastle.Math;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Random = System.Random;

//
namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public class Wallet : BaseSigner
    {
        private readonly Key _signingKey;

        public Wallet(string privateKey, BaseProvider baseProvider = null) : base(null)
        {
            if (baseProvider != null) BaseProvider = baseProvider;
            var key = new Key();


            //throw new NotImplementedException();
        }

        public Wallet(HDNode.HDNode hdNode, BaseProvider baseProvider = null) : base(null)
        {
            if (baseProvider != null) BaseProvider = baseProvider;

            _signingKey = hdNode.PrivateKey;
        }

        public static Wallet CreateRandom(string path = "m/44'/60'/0'/0/0", string locale = "en")
        {
            // Generate a random seed using the RNGCryptoServiceProvider
            byte[] randomBytes = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // TODO: extra entropy
            // hash keccak-256 of random bytes
            // var hash = Keccak.Keccak256(random);
            var mnemonic = new Mnemonic(Wordlist.English, randomBytes).ToString();
            Console.WriteLine("Mnemonic: " + mnemonic);
            return FromMnemonic(mnemonic, path, locale);
        }

        public static void CreateEthWallet()
        {
            // Generate a random seed using the RNGCryptoServiceProvider
            byte[] randomBytes = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            // Convert the random bytes to a mnemonic seed phrase
            string seedPhrase = new Mnemonic(Wordlist.English, randomBytes).ToString();

            Console.WriteLine("Seed phrase: " + seedPhrase);

            // Convert the seed phrase to a 64-byte seed using PBKDF2
            byte[] salt = Encoding.UTF8.GetBytes("mnemonic");
            using (var pbkdf2 = new Rfc2898DeriveBytes(seedPhrase, salt, 2048))
            {
                byte[] seed = pbkdf2.GetBytes(32);

                // Keep generating a private key until a valid one is obtained
                EthECKey privateKey;
                do
                {
                    privateKey = new EthECKey(seed, true);
                    seed = Increment(seed);
                } while (!IsValidPrivateKey(privateKey.GetPrivateKeyAsBytes()));

                // Print the private key
                Console.WriteLine("Private key: " + privateKey.GetPrivateKeyAsBytes().ToHex());

                // Generate the Ethereum public address from the private key
                string address = privateKey.GetPublicAddress().ToLower();


                // Print the Ethereum public address
                Console.WriteLine("Public address: " + address);
            }
        }
        static bool IsValidPrivateKey(byte[] privateKeyBytes)
        {
            // Convert the private key bytes to a BigInteger
            BigInteger privateKey = new BigInteger(1, privateKeyBytes);

            // Check if the private key is within the range of valid private keys for the secp256k1 curve
            BigInteger n = new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", 16);
            return privateKey.CompareTo(BigInteger.One) >= 0 && privateKey.CompareTo(n) < 0;
        }

        static byte[] Increment(byte[] bytes)
        {
            // Increment the least significant byte by 1
            byte[] result = new byte[bytes.Length];
            Array.Copy(bytes, result, bytes.Length);
            for (int i = result.Length - 1; i >= 0; i--)
            {
                if (result[i] < 255)
                {
                    result[i]++;
                    break;
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }

        public static Wallet FromMnemonic(string mnemonic, string path = "m/44'/60'/0'/0/0",
            string locale = "en")
        {
            return new Wallet(HDNode.HDNode.FromMnemonic(mnemonic, null, locale).DerivePath(path));
        }

        public string Address { get; }

        public BaseProvider BaseProvider { get; }

        public string PublicKey { get; }

        public override Task<string> GetAddress()
        {
            return base.GetAddress(); // TODO:
        }

        public override Task<string> SignMessage(byte[] message)
        {
            var hash = new Sha3Keccack().CalculateHash(message);
            return Task.FromResult(_signingKey.Sign(new uint256(hash)).ToCompact().ToHex());
        }

        public override Task<string> SignMessage(string message)
        {
            var hash = new Sha3Keccack().CalculateHash(message);
            Console.WriteLine("Hash: " + hash);
            return Task.FromResult(_signingKey.Sign(new uint256(hash)).ToCompact().ToHex());
        }

        public override Task<string> SignTransaction(TransactionRequest transaction)
        {
            // TODO: serialize transaction and get keccak-256 hash to sign
            // return Task.FromResult(_signingKey.Sign(transaction.ToRPCParam()));
            return base.SignTransaction(transaction);
        }
    }
}
*/