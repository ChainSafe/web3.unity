using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Prefabs.Web3AuthWallet.UI;
using UnityEngine.UI;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class SignW3A : MonoBehaviour
{
    public Text responseText;
    public string message = "This is a test message to sign";
    private GameObject CSWallet = null;
    private Web3AuthWalletConfig _web3AuthWalletConfig;
    private Web3AuthWallet _web3AuthWallet;
    private Web3 _web3;


    async private void Awake()
    {
        var projectConfig = ProjectConfigUtilities.Load();
        _web3 = await new Web3Builder(projectConfig)
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseJsonRpcProvider();
                services.UseWeb3AuthWallet();
            })
            .BuildAsync();
        _web3AuthWallet = new Web3AuthWallet(_web3.RpcProvider, _web3AuthWalletConfig);
        _web3AuthWalletConfig = new Web3AuthWalletConfig();
    }

    public void OnEnable()
    {
        // resets response text
        responseText.text = string.Empty;
    }

    public void OnSignMessage()
    {
        // finds the wallet, sets sign and incoming tx conditions to true and opens
        CSWallet = GameObject.FindGameObjectWithTag("CSWallet");
        W3AWalletUtils.IncomingTx = true;
        W3AWalletUtils.IncomingAction = "Sign";
        W3AWalletUtils.IncomingMessageData = message;
        CSWallet.GetComponent<Web3AuthWalletUI>().OpenButton();
    }

    void Update()
    {
        if (W3AWalletUtils.SignedTxResponse != string.Empty)
        {
            // display signed tx response from wallet
            responseText.text = W3AWalletUtils.SignedTxResponse;
            W3AWalletUtils.SignedTxResponse = string.Empty;
        }
    }
}