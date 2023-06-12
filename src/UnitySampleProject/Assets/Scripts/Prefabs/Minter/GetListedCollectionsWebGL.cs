using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;

#if UNITY_WEBGL
public class GetListedCollectionsWebGL : MonoBehaviour
{
    // vars and objects
    public Text[] idsBuy;
    public Text[] descriptionsBuy;
    public Text[] prices;
    public Text[] sellers;
    public Text[] tokenTypesBuy;
    public Renderer[] textureObjects;
    public GameObject C1b;
    public GameObject C2b;
    public GameObject C3b;
    private int nftListAmount;
    private int nftCount;
    private string account;
    public string chain = "ethereum";
    public string network = "goerli";
    public string chainID = "5";
    public string collectionSlug;
    private string nftContract = "0x2c1867bc3026178a47a677513746dcc6822a137a";

    void Start()
    {
        // load the nft data
        LoadNftDataBuyPage();
    }
    
    async void LoadNftDataBuyPage()
    {
        account = PlayerPrefs.GetString("Account");
        
        // create a reference to a list and iterate through it to gain tokenids
        List<string> tokenIdList = new List<String>();
        
        // checks if filter should be applied
        if (collectionSlug == "")
        {
            Debug.Log("Please set collection ID");
            return;
        }
        else
        {
            string collections = await EVM.GetNftCollectionBySlug(collectionSlug);
            CollectionModel.Collection response = ParseCollections(collections);
            nftListAmount = response.items.Count;
            for (int i = 0; i < nftListAmount; i++)
            {
                tokenIdList.Add(response.items[i].tokenId);
            }
        }

        // display check based on nft count fetched, only activates object with data
        if (nftListAmount == 1)
        {
            C1b.SetActive(true);
        }
        else if (nftListAmount == 2)
        {
            C1b.SetActive(true);
            C2b.SetActive(true);
        }
        else if (nftListAmount >= 3)
        {
            C1b.SetActive(true);
            C2b.SetActive(true);
            C3b.SetActive(true);
        }

        List<GetNftListModel.Response> listResponse = await EVM.GetNftMarket(chain, network);

        foreach (string tokenId in tokenIdList)
        {
            for (int i = 0; i < listResponse.Count; i++)
            {
                if (listResponse[i].tokenId == tokenId)
                {
                    string nftResponseStr = await EVM.GetNft(account, chain, network, nftContract, tokenId);
                GetNftModel.Response nftResponse = ParseNft(nftResponseStr);
                // breaks out of loop and continues on if an error case is found for some reason
                if (nftResponseStr == "{}")
                {
                    continue;
                }
                
                if (nftResponse.uri.StartsWith("ipfs://"))
                {
                    nftResponse.uri = nftResponse.uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
                }
                UnityWebRequest webRequest = UnityWebRequest.Get(nftResponse.uri);
                await webRequest.SendWebRequest();
                RootGetNFT data =
                    JsonConvert.DeserializeObject<RootGetNFT>(
                        System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                // populate text objects with data
                idsBuy[nftCount].text = listResponse[i].itemId;
                float decimals = 18;
                double price = float.Parse(listResponse[i].price) / Math.Pow(10, decimals);
                prices[nftCount].text = listResponse[i].price;
                sellers[nftCount].text = nftResponse.owner;
                tokenTypesBuy[nftCount].text = nftResponse.tokenType;
                if (data.description == null)
                {
                    descriptionsBuy[nftCount].text = "";
                }
                else
                {
                    descriptionsBuy[nftCount].text = data.description;
                }
                // parse json to get image uri and download
                string imageUri = data.image;
                if (imageUri.StartsWith("ipfs://"))
                {
                    imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
                    StartCoroutine(DownloadImage(imageUri, nftCount));
                }
                else
                {
                    StartCoroutine(DownloadImage(imageUri, nftCount));
                }

                if (data.properties != null)
                {
                    foreach (var prop in data.properties.additionalFiles)
                    {
                        if (prop.StartsWith("ipfs://"))
                        {
                            var additionalURi = prop.Replace("ipfs://", "https://ipfs.io/ipfs/");
                        }
                    }
                }
                // increase nft count so we can keep track of the local objects
                nftCount++;
                }
            }   
        }
    }

    // buy nft function
    public async void BuyNFT(int nftNumber)
    {
        Debug.Log("Buying Nft");
        BuyNFT.Response response = await EVM.CreatePurchaseNftTransaction(chain, network,
            account, idsBuy[nftNumber].text, prices[nftNumber].text, tokenTypesBuy[nftNumber].text);
        Debug.Log(account);
        Debug.Log(idsBuy[nftNumber].text);
        Debug.Log(prices[nftNumber].text);
        Debug.Log(tokenTypesBuy[nftNumber].text);
        Debug.Log("Account: " + response.tx.account);
        Debug.Log("To : " + response.tx.to);
        Debug.Log("Value : " + response.tx.value);
        Debug.Log("Data : " + response.tx.data);
        Debug.Log("Gas Price : " + response.tx.gasPrice);
        Debug.Log("Gas Limit : " + response.tx.gasLimit);

        try
        {
            string responseNft = await Web3GL.SendTransaction(response.tx.to, response.tx.value, response.tx.gasLimit, response.tx.gasLimit);
            if (responseNft == null)
            {
                Debug.Log("Empty Response Object:");
            }
            print(responseNft);
            Debug.Log(responseNft);
        }
        catch (Exception e)
        {
            Debug.LogError(e, this);
        }
    }
    
    // downloads the nft image
    IEnumerator DownloadImage(string MediaUrl, int nftCount)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
        {
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
            textureObjects[nftCount].GetComponent<Image>().sprite = webSprite;
        }
    }

    // render the sprite into the image object
    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
            100.0f);
    }
    
    // parses the json response, as it has lists within the object it needs extra care
    public static CollectionModel.Collection ParseCollections(string json)
    {
        CollectionModel.Root root = JsonConvert.DeserializeObject<CollectionModel.Root>(json);
        return root.response.collection;
    }
    
    // parses the json response, as it has lists within the object it needs extra care
    public static GetNftModel.Response ParseNft(string json)
    {
        GetNftModel.Root root = JsonConvert.DeserializeObject<GetNftModel.Root>(json);
        return root.response;
    }
}
#endif