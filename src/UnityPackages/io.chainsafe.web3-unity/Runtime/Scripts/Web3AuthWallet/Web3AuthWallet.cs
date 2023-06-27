using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.Web3AuthWallet
{
    public class Web3AuthWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly Web3AuthWalletConfig configuration;
        private readonly IRpcProvider provider;
        
        public class ResponseObject<T>
        {
            public TransactionResponse? Response;
        }

        public Web3AuthWallet(
            IRpcProvider provider,
            Web3AuthWalletConfig configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public ValueTask Connect()
        {
            if (Connected)
            {
                throw new Web3Exception("Signer already connected.");
            }

            Connected = true;
            return default;
        }

        public Task<string> GetAddress()
        {
            if (!Connected)
            {
                throw new Web3Exception(
                    $"Can't retrieve public address. {nameof(Web3AuthWallet)} is not connected yet.");
            }

            string address = new EthECKey(configuration?.PrivateKey).GetPublicAddress();
            return Task.FromResult(address);
        }

        public Task<string> SignMessage(string message) =>
            Task.FromResult(new EthereumMessageSigner().HashAndSign(
                message,
                configuration?.PrivateKey));

        public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
        {
            throw new NotImplementedException();
        }
        
        public async Task<string> SignTransaction(TransactionRequest transaction)
        {
            int int32 = Convert.ToInt32(transaction.ChainId);
            EthECKey ethEcKey = new EthECKey(configuration?.PrivateKey);
            byte[] byteArray = W3AUtils.Transaction.HexToByteArray();
            BigInteger chainId = transaction.ChainId;
            return await Task.FromResult(int32 == 137 || int32 == 80001 || int32 == 1666600000 || int32 == 1666700000 || int32 == 25 || int32 == 338 || int32 == 250 || int32 == 4002 || int32 == 43114 || int32 == 43113 ? EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateYParityV(byteArray)) : EthECDSASignature.CreateStringSignature(ethEcKey.SignAndCalculateV(byteArray, chainId)));
        }

        // Returns an object that i can't parse into a string, transaction works on the explorer
        public async Task<TransactionResponse?> SendTransaction(TransactionRequest transaction)
        {
            var projectConfigSo = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", projectConfigSo.Chain);
            form.AddField("network", projectConfigSo.Network);
            form.AddField("account", transaction.From);
            form.AddField("to", transaction.To);
            form.AddField("value", transaction.Value.ToString());
            form.AddField("data", transaction.Data);
            form.AddField("signature", W3AUtils.SignedTxResponse);
            form.AddField("gasPrice", transaction.GasPrice.ToString());
            form.AddField("gasLimit", transaction.GasLimit.ToString());
            form.AddField("rpc", projectConfigSo.Rpc);
            string url = configuration.Host + "/broadcastTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                ResponseObject<TransactionResponse> data = JsonUtility.FromJson<ResponseObject<TransactionResponse>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                if (data.Response != null)
                {
                    return data.Response;
                }
            }
            return null;
        }
        
        public static async Task<string?> CreateTransaction(TransactionRequest transaction)
        {
            var projectConfigSo = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", projectConfigSo.Chain);
            form.AddField("network", projectConfigSo.Network);
            form.AddField("account", transaction.From);
            form.AddField("to", transaction.To);
            form.AddField("value", transaction.Value.ToString());
            form.AddField("data", transaction.Data);
            form.AddField("gasPrice", transaction.GasPrice.ToString());
            form.AddField("gasLimit", transaction.GasLimit.ToString());
            form.AddField("rpc", projectConfigSo.Rpc);
            string url =  "https://api.gaming.chainsafe.io/evm/createTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                ResponseObject<string> data = JsonUtility.FromJson<ResponseObject<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                if (data.Response != null)
                {
                    return data.Response.Hash;
                }
            }

            return null;
        }

        public ValueTask WillStartAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask WillStopAsync()
        {
            throw new NotImplementedException();
        }
    }
}