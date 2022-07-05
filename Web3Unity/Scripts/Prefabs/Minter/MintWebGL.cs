using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEditor;
using UnityEngine;

#if UNITY_WEBGL
public class MintWebGL : MonoBehaviour
{
    // Start is called before the first frame update
    public string chain = "ethereum";
    public string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
    public string account;
    public string to = "0x148dC439Ffe10DF915f1DA14AA780A47A577709E";
    public string cid = "QmXjWjjMU8r39UCEZ8483aNedwNRFRLvvV9kwq1GpCgthj";


    public void Start()
    {
        account = PlayerPrefs.GetString("Account");
    }

    public async void MintNFT()
    {
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid);
        // connects to user's browser wallet (metamask) to send a transaction
        try
        {   
               string response = await Web3GL.SendTransactionData(nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.gasPrice,nftResponse.tx.gasLimit, nftResponse.tx.data);
            print("Response: " + response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
#endif