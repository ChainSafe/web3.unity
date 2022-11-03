using System;
using Models;
using UnityEngine;

namespace Web3Unity.Scripts.Prefabs.Minter
{
    public class MintWeb3Wallet721 : MonoBehaviour
    {

        public string chain = "ethereum";
        public string network = "goerli"; // mainnet ropsten kovan rinkeby goerli
        public string account;
        public string to = "0x7259E32e35cf880aEACfbD412E7F4Baa8606e04c";
        public string cid721 = "f01559ae4021a47e26bc773587278f62a833f2a6117411afbc5a7855661936d1c";
        public string chainId = "5";
        public string type721 = "721";

        public void Awake()
        {
            account = PlayerPrefs.GetString("Account");
        }

        // Start is called before the first frame update
        public async void MintNft721()
        {
            CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid721, type721);
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

        public async void VoucherMintNft721()
        {
            // validates the account that sent the voucher, you can change this if you like to fit your system
            if (PlayerPrefs.GetString("Web3Voucher721") == "0x1372199B632bd6090581A0588b2f4F08985ba2d4"){
                CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid721, type721);
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
}
