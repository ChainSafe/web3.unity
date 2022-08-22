// using System;
// using System.Threading.Tasks;
// using NBitcoin;
// using Nethereum.Hex.HexConvertors.Extensions;
// using Nethereum.Util;
// using UnityEngine;
// using Web3Unity.Scripts.Library.Ethers.Providers;
// using Web3Unity.Scripts.Library.Ethers.Transactions;
// using Random = System.Random;
//
// namespace Web3Unity.Scripts.Library.Ethers.Signers
// {
//     public class Wallet : BaseSigner
//     {
//         private readonly Key _signingKey;
//         
//         public Wallet(string privateKey, BaseProvider baseProvider = null) : base(null)
//         {
//             if (baseProvider != null)
//             {
//                 BaseProvider = baseProvider;
//             }
//             
//             // new Key()
//
//             throw new NotImplementedException();
//         }
//         
//         public Wallet(HDNode.HDNode hdNode, BaseProvider baseProvider = null) : base(null)
//         {
//             if (baseProvider != null)
//             {
//                 BaseProvider = baseProvider;
//             }
//             
//             _signingKey = hdNode.PrivateKey;
//         }
//
//         public static Wallet CreateRandom(string path = "m/44'/60'/0'/0/0", string locale = "en")
//         {
//             var entropy = new byte[16];
//             new Random().NextBytes(entropy);
//
//             // TODO: extra entropy
//             // hash keccak-256 of random bytes
//             // var hash = Keccak.Keccak256(random);
//
//             var mnemonic = HDNode.HDNode.EntropyToMnemonic(entropy, locale);
//             return FromMnemonic(mnemonic, path, locale);
//         }
//
//         public static Wallet FromMnemonic(string mnemonic, string path = "m/44'/60'/0'/0/0",
//             string locale = "en")
//         {
//             return new Wallet(HDNode.HDNode.FromMnemonic(mnemonic, null, locale).DerivePath(path));
//         }
//
//         public string Address { get; }
//
//         public BaseProvider BaseProvider { get; }
//
//         public string PublicKey { get; }
//
//         public override Task<string> GetAddress()
//         {
//             return base.GetAddress(); // TODO:
//         }
//
//         public override Task<string> SignMessage(byte[] message)
//         {
//             var hash = new Sha3Keccack().CalculateHash(message);
//             return Task.FromResult(_signingKey.Sign(new uint256(hash)).ToCompact().ToHex());
//         }
//
//         public override Task<string> SignMessage(string message)
//         {
//             var hash = new Sha3Keccack().CalculateHash(message);
//             return Task.FromResult(_signingKey.Sign(new uint256(hash)).ToCompact().ToHex());
//         }
//
//         public override Task<string> SignTransaction(TransactionRequest transaction)
//         {
//             // TODO: serialize transaction and get keccak-256 hash to sign
//             // return Task.FromResult(_signingKey.Sign(transaction.ToRPCParam()));
//             return base.SignTransaction(transaction);
//         }
//     }
// }