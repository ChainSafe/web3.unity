using System;
using Models;
using UnityEngine;


public class MintWeb3Wallet : MonoBehaviour
{

    public string chain = "ethereum";
    public string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
    public string account;
    public string to = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
    public string cid = "QmXjWjjMU8r39UCEZ8483aNedwNRFRLvvV9kwq1GpCgthj";
    public string chainId = "4";

    public void Start()
    {
        account = PlayerPrefs.GetString("Account");
    }

    // Start is called before the first frame update
    public async void MintNFT()
    {
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid);
        Debug.Log("NFT Response: " + nftResponse);
        account = PlayerPrefs.GetString("Account");
        // connects to user's browser wallet (metamask) to send a transaction
        try
        {
            string response = await Web3Wallet.SendTransaction(chainId, nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.data, nftResponse.tx.gasLimit, nftResponse.tx.gasPrice);
            print(response);
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
