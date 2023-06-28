using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using Nethereum.Signer;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public class TransactionService : ITransactionService
{
    // used to obtain a users wallet address from their private key stored in memory
    public string GetAddressW3A(string _privateKey) => new EthECKey(_privateKey).GetPublicAddress();

    public async Task<string> CreateTransaction(string _chain, string _network, string _account, string _to,
        string _value, string _data, string _gasPrice = "", string _gasLimit = "", string _rpc = "", string _nonce = "")
    {
        WWWForm form = new WWWForm();
        Debug.Log("ProjectID: " + PlayerPrefs.GetString("ProjectID"));
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
        string url = "https://api.gaming.chainsafe.io/evm/createTransaction";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            EVM.Response<string> data = JsonUtility.FromJson<EVM.Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public async Task<string> BroadcastTransaction(string _chain, string _network, string _account, string _to,
        string _value, string _data, string _signature, string _gasPrice, string _gasLimit, string _rpc)
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
        string url = "https://api.gaming.chainsafe.io/evm/broadcastTransaction";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            EVM.Response<string> data =
                JsonUtility.FromJson<EVM.Response<string>>(
                    System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

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
        string url = "https://api.gaming.chainsafe.io/evm/broadcastTransaction";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Web3AuthWallet.ResponseObject<TransactionResponse> data = JsonUtility.FromJson<Web3AuthWallet.ResponseObject<TransactionResponse>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            if (data.Response != null)
            {
                return data.Response;
            }
        }

        return null;
    }
}
