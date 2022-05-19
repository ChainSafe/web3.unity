using System;
using Models;
using UnityEngine;

public class MintWeb3Wallet : MonoBehaviour
{

    public string chain = "ethereum";
    public string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
    public string account = "0xAd51aeAFB332719be31cb8F8bDF036Eff1478827";
    public string to = "0xAd51aeAFB332719be31cb8F8bDF036Eff1478827";
    public string cid = "QmXjWjjMU8r39UCEZ8483aNedwNRFRLvvV9kwq1GpCgthj";
    public string chainId = "4";

    
    // Start is called before the first frame update
    public async void MintNFT()
    {
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid);
        Debug.Log("NFT Response: " + nftResponse);
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
