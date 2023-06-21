using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NBitcoin;
using Nethereum.Hex.HexConvertors.Extensions;
using Web3Unity.Scripts.Library.Ethers.HDNode.Wordlists;
using Wordlist = Web3Unity.Scripts.Library.Ethers.HDNode.Wordlists.Wordlist;

namespace Web3Unity.Scripts.Library.Ethers.HDNode
{
    public class HDNode
    {
        // private const uint HardenedBit = 0x80000000;
        private static readonly object ConstructorGuard = new();

        // private readonly string _privateKey;
        // private readonly string _publicKey;
        // private readonly string _fingerprint;
        // private readonly string _parentFingerprint;
        // private readonly string _address;
        private readonly string mnemonic;

        // private readonly string _path;
        // private readonly string _chainCode;
        // private readonly int _index;
        // private readonly int _depth;
        private readonly ExtKey extKey;

        public HDNode(object constructorGuard, ExtKey key, string mnemonic, string path)
        {
            if (constructorGuard != HDNode.ConstructorGuard)
            {
                throw new Exception("HDNode constructor cannot be called directly");
            }

            this.mnemonic = mnemonic;

            // _path = path;
            extKey = key;
        }

        public Key PrivateKey => extKey.PrivateKey;

        public static HDNode FromMnemonic(string mnemonic, string password = null, string locale = "en")
        {
            return FromSeed(MnemonicToSeed(mnemonic, password), mnemonic, "m/44'/60'/0'/0", locale);
        }

        private static HDNode FromSeed(byte[] seed, string mnemonic, string path, string locale)
        {
            var keyPath = new KeyPath(path);
            var key = new ExtKey(seed.ToHex()).Derive(keyPath);

            return new HDNode(ConstructorGuard, key, mnemonic, null);
        }

        public static byte[] MnemonicToSeed(string mnemonic, string password)
        {
            password ??= string.Empty;
            var salt = Encoding.UTF8.GetBytes("mnemonic" + password.Normalize(NormalizationForm.FormKD));
            var pass = Encoding.UTF8.GetBytes(password.Normalize(NormalizationForm.FormKD));
            return new Rfc2898DeriveBytes(pass, salt, 1000).GetBytes(64);
        }

        public static string EntropyToMnemonic(byte[] entropy, string locale = "en")
        {
            var wordlist = GetWordlist(locale);

            if (entropy.Length % 4 != 0 || entropy.Length is < 16 or > 32)
            {
                throw new Exception("entropy must be between 16 and 32 byte bytes and a multiple of 4");
            }

            // var indices = new int[] {0};
            var indices = new List<int> { 0 };

            var remainingBits = 11;
            foreach (var e in entropy)
            {
                // Consume the whole byte (will still more to go)
                if (remainingBits > 8)
                {
                    indices[indices.Count - 1] <<= 8;
                    indices[indices.Count] |= e >> (8 - remainingBits);
                    remainingBits -= 8;
                }
                else
                {
                    // This byte will complete an 11-bit index
                    indices[indices.Count] <<= remainingBits;
                    indices[indices.Count] |= e >> (8 - remainingBits);

                    // Start the next word
                    indices.Add(e & ((1 << (8 - remainingBits)) - 1));

                    remainingBits += 3;
                }
            }

            // Compute the checksum bits
            var sha256 = SHA256.Create();
            var checksumBits = entropy.Length / 4;
            var checksum = sha256.ComputeHash(entropy)[0] & (((1 << checksumBits) - 1) << (8 - checksumBits));

            // Shift the checksum into the word indices
            indices[indices.Count] <<= checksumBits;
            indices[indices.Count] |= checksum >> (8 - checksumBits);

            return string.Join(" ", indices.Select(index => wordlist.GetWord(index)));
        }

        private static Wordlist GetWordlist(string locale)
        {
            return locale switch
            {
                "en" => new WordlistEnglish(),

                // "ja" => JapaneseWordlist,
                // "zh" => ChineseWordlist,
                _ => throw new Exception("invalid locale")
            };
        }

        public HDNode DerivePath(string path)
        {
            var keyPath = new KeyPath(path);
            return new HDNode(ConstructorGuard, extKey.Derive(keyPath), mnemonic, path);
        }
    }
}