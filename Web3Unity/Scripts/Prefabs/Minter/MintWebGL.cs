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
    public string account = "0x148dC439Ffe10DF915f1DA14AA780A47A577709E";
    public string to = "0x148dC439Ffe10DF915f1DA14AA780A47A577709E";
    
    
    public async void MintNFT()
    {
        var data = System.Text.Encoding.UTF8.GetBytes("YOUR_DATA");
        
        IPFS ipfs = new IPFS("YOUR_CHAINSAFE_STORAGE_API_KEY");
        string cid  = await ipfs.Upload("BUCKET_ID", "/PATH", "FILENAME.ext", data, "application/octet-stream");

        Debug.Log("IPFS CID: " + cid);

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