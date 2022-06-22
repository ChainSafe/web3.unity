using System;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public class Wallet : BaseSigner
    {
        public Wallet(string privateKey, BaseProvider baseProvider = null) : base(null)
        {
            if (baseProvider != null)
            {
                BaseProvider = baseProvider;
            }
        }

        public static Wallet CreateRandom()
        {
            throw new NotImplementedException();
        }

        public static Wallet FromMnemonic(string mnemonic, string path = "m/44'/60'/0'/0/0",
            string wordlist = "english")
        {
            throw new NotImplementedException();
        }

        public string Address { get; }

        public BaseProvider BaseProvider { get; }

        public string PublicKey { get; }
    }
}