using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Web3Unity.Scripts.Library.Ethers.Web3AuthWallet
{
    // class contents copied from the dll in plugins so we can modify
    public class W3AWalletUtils
    {
        public static string account { get; set; }
        public static string pk { get; set; }
        public static string amount { get; set; }
        public static string outgoingContract { get; set; }
        public static string incomingAction { get; set; }
        public static string incomingTxData { get; set; }
        public static string incomingMessageData { get; set; }
        public static string signedTxResponse { get; set; }
        public static bool incomingTx { get; set; }

        private static readonly string host = "https://api.gaming.chainsafe.io/evm";

        // used to obtain a users wallet address from their private key stored in memory
        public static string GetAddressW3A(string _privateKey) => new EthECKey(_privateKey).GetPublicAddress();

        // used to sign a message with a users private key stored in memory
        public static string SignMsgW3A(string _privateKey, string _message) => new EthereumMessageSigner().HashAndSign(_message, _privateKey);

        // creates a transaction for the wallet
        
        public interface ITransactionService
        {
            Task<string> CreateTransaction(string chain, string network, string account, string to, string value, string data, string gasPrice = "", string gasLimit = "", string rpc = "", string nonce = "");
            Task<string> BroadcastTransaction(string chain, string network, string account, string to, string value, string data, string signature, string gasPrice, string gasLimit, string rpc);
        }

        public interface ISignatureService
        {
            string SignTransaction(string privateKey, string transaction, string chainId);
        }

        public class TransactionService : ITransactionService
        {
            public async Task<string> CreateTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _gasPrice = "", string _gasLimit = "", string _rpc = "", string _nonce = "")
            {
                WWWForm form = new WWWForm();
                form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
                form.AddField("chain", _chain);
                form.AddField("network", _network);
                form.AddField("account", _account);
                form.AddField("to", _to);
                form.AddField("value", _value);
                form.AddField("data", _data);
                form.AddField("gasPrice", _gasPrice);
                form.AddField("gasLimit", _gasLimit);
                form.AddField("rpc", _rpc);
                form.AddField("nonce", _nonce);
                string url = host + "/createTransaction";
                using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
                {
                    await webRequest.SendWebRequest();
                    EVM.Response<string> data = JsonUtility.FromJson<EVM.Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                    return data.response;
                }
            }

            public async Task<string> BroadcastTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _signature, string _gasPrice, string _gasLimit, string _rpc)
            {
                WWWForm form = new WWWForm();
                form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
                form.AddField("chain", _chain);
                form.AddField("network", _network);
                form.AddField("account", _account);
                form.AddField("to", _to);
                form.AddField("value", _value);
                form.AddField("data", _data);
                form.AddField("signature", _signature);
                form.AddField("gasPrice", _gasPrice);
                form.AddField("gasLimit", _gasLimit);
                form.AddField("rpc", _rpc);
                string url = host + "/broadcastTransaction";
                using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
                {
                    await webRequest.SendWebRequest();
                    EVM.Response<string> data = JsonUtility.FromJson<EVM.Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                    return data.response;
                }
            }
        }

        public class SignatureService : ISignatureService
        {
            public string SignTransaction(string _privateKey, string _transaction, string _chainId)
            {
                int int32 = Convert.ToInt32(_chainId);
                EthECKey ethEcKey = new EthECKey(_privateKey);
                byte[] byteArray = _transaction.HexToByteArray();
                BigInteger chainId = BigInteger.Parse(_chainId);
                return int32 == 137 || int32 == 80001 || int32 == 1666600000 || int32 == 1666700000 || int32 == 25 || int32 == 338 || int32 == 250 || int32 == 4002 || int32 == 43114 || int32 == 43113 ? EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateYParityV(byteArray)) : EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateV(byteArray, chainId));
            }
        }
    }
}