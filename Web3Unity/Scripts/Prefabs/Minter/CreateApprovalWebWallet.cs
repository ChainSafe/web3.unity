using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var response = await EVM.CreateApproveTransaction(chain, network, account, tokenType);
        Debug.Log("Response: " + response.connection.chain);
        
        try
        {
            
            string responseNft = await Web3Wallet.SendTransaction(chainID, response.tx.to, "0",
                response.tx.data, response.tx.gasLimit, response.tx.gasPrice);
            if (responseNft == null)
            {
                Debug.Log("Empty Response Object:");
            }
            print(responseNft);
            Debug.Log(responseNft);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}
