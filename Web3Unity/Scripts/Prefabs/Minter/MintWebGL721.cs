using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEditor;
using UnityEngine;

#if UNITY_WEBGL
public class MintWebGL721 : MonoBehaviour
{
    // Start is called before the first frame update
    public string chain = "ethereum";
    public string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
    public string account;
    public string to;
    // update cid per mint
    public string cid = "QmXjWjjMU8r39UCEZ8483aNedwNRFRLvvV9kwq1GpCgthj";
    public string type721 = "721";


    public void Awake()
    {
        account = PlayerPrefs.GetString("Account");
        to = PlayerPrefs.GetString("Account");
    }

    public async void MintNFT()
    {
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid, type721);
        // connects to user's browser wallet (metamask) to send a transaction
        Debug.Log("NFT Response: " + nftResponse);
        try
        {   
            string response = await Web3GL.SendTransactionData(nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.gasPrice,nftResponse.tx.gasLimit, nftResponse.tx.data);
            print("Response: " + response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }

    public async void VoucherMintNFT()
    {
        // validates the account that sent the voucher, you can change this if you like to fit your system
        if (PlayerPrefs.GetString("WebGLVoucher721") == "0x1372199B632bd6090581A0588b2f4F08985ba2d4"){
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid, type721);
        // connects to user's browser wallet (metamask) to send a transaction
        try
        {   
            string response = await Web3GL.SendTransactionData(nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.gasPrice,nftResponse.tx.gasLimit, nftResponse.tx.data);
            print("Response: " + response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
    else
    {
        Debug.Log("Voucher Invalid");
    }
    }
}
#endif