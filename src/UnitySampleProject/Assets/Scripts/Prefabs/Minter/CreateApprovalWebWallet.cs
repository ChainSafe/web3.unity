using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.Network;
using Web3Unity.Scripts.Library.Ethers.Providers;
// using Web3Unity.Scripts.Library.Web3Wallet;
using Network = Web3Unity.Scripts.Library.Ethers.Network.Network;

public class CreateApprovalWebWallet : MonoBehaviour
{
    // Start is called before the first frame update
    public string chain = "ethereum";
    public string network = "goerli";
    public string account;
    public string tokenType = "1155";
    string chainID = "5";

    private void Awake()
    {
        account = PlayerPrefs.GetString("Account");
    }

    public async void ApproveTransaction()
    {
        throw new NotImplementedException(
            "Example scripts are in the process of migration to the new API. This function has not yet been migrated.");

        // var response = await EVM.CreateApproveTransaction(chain, network, account, tokenType);
        // Debug.Log("Response: " + response.connection.chain);
        // var web3 = new Web3Builder().Configure(services =>
        // {
        //     services.UseUnityEnvironment(new UnityEnvironmentConfiguration());
        //     services.UseJsonRpcProvider(new JsonRpcProviderConfiguration { Network = new Network(){ Name = network, ChainId = chainID}});
        //     services.UseMetaMaskBrowserSigner(new MetaMaskBrowserSignerConfiguration())
        // })
        //
        // try
        // {
        //
        //     string responseNft = await Web3Wallet.SendTransaction(chainID, response.tx.to, "0",
        //         response.tx.data, response.tx.gasLimit, response.tx.gasPrice);
        //     if (responseNft == null)
        //     {
        //         Debug.Log("Empty Response Object:");
        //     }
        //     print(responseNft);
        //     Debug.Log(responseNft);
        // }
        // catch (Exception e)
        // {
        //     Debug.LogException(e, this);
        // }
    }
}
