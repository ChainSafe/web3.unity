using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;

public class MintIPFS : MonoBehaviour
{

    // Start is called before the first frame update
    public string chain = "ethereum";
    public string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
    public string account = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
    public string to = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
    public string type = "721"; // for 1155 assets update type.
    public string cid = "QmbnT8LsBCShSaeSSXyrmX1rHdWZdbj45Whyioomqekwr4";
    
    
    async public void MintButtonIPFS()
    {
        var data = System.Text.Encoding.UTF8.GetBytes("YOUR_DATA");
        
        IPFS ipfs = new IPFS("YOUR_CHAINSAFE_STORE_API_KEY");
        string cid  = await ipfs.Upload("BUCKET_ID", "/PATH", "FILENAME.ext", data, "application/octet-stream");

        Debug.Log("IPFS CID: " + cid);
        
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid, type);
        if (nftResponse != null)
        {
            Debug.Log("CID: " + nftResponse.cid);
            Debug.Log("Connection: " + nftResponse.connection);
            Debug.Log("TC Account: " + nftResponse.tx.account);
            Debug.Log("TX Data: " + nftResponse.tx.data);
            Debug.Log("TX Value: " + nftResponse.tx.value);
            Debug.Log("TX Gas Limit: " + nftResponse.tx.gasLimit);
            Debug.Log("TX Gas Price: " + nftResponse.tx.gasPrice);
            Debug.Log("Hashed Unsigned TX: " + nftResponse.hashedUnsignedTx);
            string chainId = await EVM.ChainId(chain, network, "");
            Debug.Log("Chain Id: " + chainId);
            string gasPrice1 = await EVM.GasPrice(chain, network, "");
            Debug.Log("Gas Price: " + gasPrice1);

            // private key of account
            string privateKey = "ADD_YOUR_PRIVATE_KEY";
            Debug.Log("Account: " + account);
            string transaction = await EVM.CreateTransaction(chain, network, nftResponse.tx.account,
                nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.data,
                nftResponse.tx.gasPrice, nftResponse.tx.gasLimit);
            Debug.Log("Transaction: " + transaction);
            string signature = Web3PrivateKey.SignTransaction(privateKey, transaction, chainId);
            print("Signature: " + signature);
            //string rpc = "";
            string responseBroadcast = await EVM.BroadcastTransaction(chain, network, nftResponse.tx.account,
                nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.data, signature,
                nftResponse.tx.gasPrice, nftResponse.tx.gasLimit, "");
            print("Response: " + responseBroadcast);
        }
    }
}