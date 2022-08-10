using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_WEBGL
public class CreateApprovalWebGL : MonoBehaviour
{
    // Start is called before the first frame update
    public string chain = "ethereum";
    public string network = "goerli";
    public string account;
    public string tokenType = "1155";

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
            
            string responseNft = await Web3GL.SendTransactionData(response.tx.to, "0", response.tx.gasPrice, response.tx.gasLimit,response.tx.data);
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
#endif