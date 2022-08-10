using System;
using Models;
using UnityEngine;

namespace Web3Unity.Scripts.Prefabs.Minter
{
    public class MintWeb3Wallet1155 : MonoBehaviour
    {

        private string chain = "ethereum";
        private string network = "goerli"; // mainnet ropsten kovan rinkeby goerli
        private string account;
        private string to;
        public string cid1155 = "bafkzvzacdlxhaqsig3fboo3kjzshfb6rltxivrbnrqwy2euje7sq";
        private string chainId = "5";
        public string type1155 = "1155";

        public void Awake()
        {
            account = PlayerPrefs.GetString("Account");
            to = PlayerPrefs.GetString("Account");
            Debug.Log("To" + to);
        }
    
    
        public async void MintNft1155()
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

        public async void VoucherMintNft1155()
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
}
