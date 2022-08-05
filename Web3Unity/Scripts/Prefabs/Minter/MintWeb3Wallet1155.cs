using System;
using Models;
using UnityEngine;


public class MintWeb3Wallet1155 : MonoBehaviour
{

    private string chain = "ethereum";
    private string network = "goerli"; // mainnet ropsten kovan rinkeby goerli
    private string account;
    private string to = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
    public string cid1155 = "f01559ae4021a47e26bc773587278f62a833f2a6117411afbc5a7855661936d1c";
    private string chainId = "5";
    public string type1155 = "1155";

    public void Awake()
    {
        account = PlayerPrefs.GetString("Account");
    }
    
    
    public async void MintNFT1155()
    {
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid1155, type1155);
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

    public async void VoucherMintNFT1155()
    {
        // validates the account that sent the voucher, you can change this if you like to fit your system
        if (PlayerPrefs.GetString("Web3Voucher1155") == "0x1372199B632bd6090581A0588b2f4F08985ba2d4"){
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid1155, type1155);
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
    else
    {
        Debug.Log("Voucher Invalid");
    }
    }
}
