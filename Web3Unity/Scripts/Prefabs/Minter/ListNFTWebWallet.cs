using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Web3Unity.Scripts.Prefabs.Minter
{
    public class ListNftWebWallet : MonoBehaviour
    {
        private string chain = "ethereum";
        private string network = "goerli";
        private string chainID = "5";
        private string _itemPrice = "0.001";
        private string _tokenType = "";
        private string _itemID = "";
        private string account;
        
        public Renderer textureObject;
        public Text description;
        public Text tokenURI;
        public Text contractAddr;
        public Text isApproved;
        public InputField itemPrice;
        public Text noListedItems;
        public Text playerAccount;
   
        public void Awake()
        {
            account = PlayerPrefs.GetString("Account");
            description.text = "";
            tokenURI.text = "";
            isApproved.text = "";
            contractAddr.text = "";
        }

        // Start is called before the first frame update
        async void Start()
        {
            playerAccount.text = account;
            try
            {
                List<MintedNFT.Response> response = await EVM.GetMintedNFT(chain, network,account);
            
                if (response[1].uri == null)
                {
                    Debug.Log("Not Listed Items");
                    return;
                }
                if (response[1].uri.StartsWith("ipfs://"))
                {
                    response[1].uri = response[1].uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
                }
            
                UnityWebRequest webRequest = UnityWebRequest.Get(response[1].uri);
                await webRequest.SendWebRequest();
                RootGetNFT data =
                    JsonConvert.DeserializeObject<RootGetNFT>(
                        System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data)); 
                description.text = data.description;
                // parse json to get image uri
                string imageUri = data.image;
                if (imageUri.StartsWith("ipfs://"))
                {
                    imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
                    StartCoroutine(DownloadImage(imageUri));
                }

                tokenURI.text = response[1].uri;
                Debug.Log(response[1].uri);
                contractAddr.text = response[1].nftContract;
                Debug.Log("NFT Contract: " + response[1].nftContract);
                isApproved.text = response[1].isApproved.ToString();
                _itemID = response[1].id;
                _itemPrice = itemPrice.text;
                Debug.Log("Token Type: " + response[1].tokenType);
                _tokenType = response[1].tokenType;
            }
            catch (Exception e)
            {
                noListedItems.text = "NO LISTED ITEM for " + account;
                Debug.Log("No Listed Items" + e);
            }
           
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator DownloadImage(string MediaUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else
            {
                Texture2D webTexture = ((DownloadHandlerTexture) request.downloadHandler).texture as Texture2D;
                Sprite webSprite = SpriteFromTexture2D(webTexture);
                textureObject.GetComponent<Image>().sprite = webSprite;
            }
        }

        Sprite SpriteFromTexture2D(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
                100.0f);
        }

        public async void ListItem()
        {

            float eth = float.Parse(_itemPrice);
            float decimals = 1000000000000000000; // 18 decimals
            float wei = eth * decimals;
            Debug.Log("ItemID: " + _itemID);
            ListNFT.Response response =
                await EVM.CreateListNftTransaction(chain, network, account, _itemID, Convert.ToDecimal(wei).ToString(), _tokenType);
            int value = Convert.ToInt32(response.tx.value.hex, 16);
            Debug.Log("Response: " + response);
            try
            {
                string responseNft = await Web3Wallet.SendTransaction(chainID, response.tx.to, value.ToString(),
                    response.tx.data, response.tx.gasLimit, response.tx.gasPrice);
                if (responseNft == null)
                {
                    Debug.Log("Empty Response Object:");
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }
    }
}

