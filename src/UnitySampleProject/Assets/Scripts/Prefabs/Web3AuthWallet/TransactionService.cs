using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using Nethereum.Signer;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public class TransactionService : ITransactionService
{
    ProjectConfigScriptableObject projectConfigSo = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));

    // used to obtain a users wallet address from their private key stored in memory
    public string GetAddressW3A(string _privateKey) => new EthECKey(_privateKey).GetPublicAddress();

    public async Task<string> CreateTransaction(string _account, TransactionRequest txRequest,  string _gasPrice = "", string _gasLimit = "", string _rpc = "", string _nonce = "")
    {
        WWWForm form = new WWWForm();
        Debug.Log("ProjectID: " + projectConfigSo.ProjectId);
        form.AddField("projectId",  projectConfigSo.ProjectId);
        form.AddField("chain",  projectConfigSo.Chain);
        form.AddField("network",  projectConfigSo.Network);
        form.AddField("account", _account);
        form.AddField("to", txRequest.To);
        form.AddField("value", txRequest.Value.ToString());
        form.AddField("data", txRequest.Data);
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

    public async Task<string> BroadcastTransaction(TransactionRequest txRequest, string _account, string _signature, string _gasPrice = "", string _gasLimit = "", string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", projectConfigSo.ProjectId);
        form.AddField("chain", projectConfigSo.Chain);
        form.AddField("network", projectConfigSo.Network);
        form.AddField("account", _account);
        form.AddField("to", txRequest.To);
        form.AddField("value", txRequest.Value.HexValue);
        form.AddField("data", txRequest.Data);
        form.AddField("signature", _signature);
        form.AddField("gasPrice", _gasPrice);
        form.AddField("gasLimit", _gasLimit);
        form.AddField("rpc", projectConfigSo.Rpc);
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

    public async Task<TransactionResponse?> SendTransaction(TransactionRequest txRequest)
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", projectConfigSo.Chain);
        form.AddField("network", projectConfigSo.Network);
        form.AddField("account", txRequest.From);
        form.AddField("to", txRequest.To);
        form.AddField("value", txRequest.Value.ToString());
        form.AddField("data", txRequest.Data);
        form.AddField("signature", W3AUtils.SignedTxResponse);
        form.AddField("gasPrice", txRequest.GasPrice.ToString());
        form.AddField("gasLimit", txRequest.GasLimit.ToString());
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
