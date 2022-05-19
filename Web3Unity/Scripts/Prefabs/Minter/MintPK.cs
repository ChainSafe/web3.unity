using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;

public class MintPK : MonoBehaviour
{

    // Start is called before the first frame update
    public string chain = "ethereum";
    public string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
    public string account = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
    public string to = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
    public string cid = "QmXjWjjMU8r39UCEZ8483aNedwNRFRLvvV9kwq1GpCgthj";

    async public void MintButtonPK()
    {
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid);
        if (nftResponse != null)
        {
            print("CID: " + nftResponse.cid);
            print("Connection: " + nftResponse.connection);
            print("TC Account: " + nftResponse.tx.account);
            print("TX Data: " + nftResponse.tx.data);
            print("TX Value: " + nftResponse.tx.value);
            print("TX Gas Limit: " + nftResponse.tx.gasLimit);
            print("TX Gas Price: " + nftResponse.tx.gasPrice);
            print("Hashed Unsigned TX: " + nftResponse.hashedUnsignedTx);
            string chainId = await EVM.ChainId(chain, network, "");
            Debug.Log("Chain Id: " + chainId);
            string gasPrice1 = await EVM.GasPrice(chain, network, "");
            Debug.Log("Gas Price: " + gasPrice1);

            // private key of account
            string privateKey = "ADD_PRIVATE_KEY";
            Debug.Log("Account: " + account);
            string transaction = await EVM.CreateTransaction(chain, network, nftResponse.tx.account,
                nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.data,
                nftResponse.tx.gasPrice, nftResponse.tx.gasLimit);
            Debug.Log("Transaction: " + transaction);
            string signature = Web3PrivateKey.SignTransaction(privateKey, transaction, chainId);
            print("Signature: " + signature);
            //string rpc = "";
            string response1 = await EVM.BroadcastTransaction(chain, network, nftResponse.tx.account,
                nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.data, signature,
                nftResponse.tx.gasPrice, nftResponse.tx.gasLimit, "");
            print("Response: " + response1);
        }
    }
}