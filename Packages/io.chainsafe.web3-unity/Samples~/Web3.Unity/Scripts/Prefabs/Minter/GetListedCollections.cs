using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Scripts.EVM.Remote;
// using Web3Unity.Scripts.Library.Web3Wallet;

public class GetListedCollections : MonoBehaviour
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
    public string collectionSlug;
    private string nftContract = "0x2c1867bc3026178a47a677513746dcc6822a137a";

    void Start()
    {
        // load the nft data
        LoadNftDataBuyPage();
    }
    async void LoadNftDataBuyPage()
    {
        account = await Web3Accessor.Web3.Signer.GetAddress();
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        // create a reference to a list and iterate through it to gain token id
        List<string> tokenIdList = new();
        // checks if filter should be applied
        if (collectionSlug == "")
        {
            Debug.Log("Please set collection ID");
            return;
        }
        else
        {
            string collections = await CSServer.GetNftCollectionBySlug(Web3Accessor.Web3, collectionSlug);
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

        List<GetNftListModel.Response> listResponse = await CSServer.GetNftMarket(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network);

        foreach (string tokenId in tokenIdList)
        {
            for (int i = 0; i < listResponse.Count; i++)
            {
                if (listResponse[i].tokenId == tokenId)
                {
                    string nftResponseStr = await CSServer.GetNft(Web3Accessor.Web3, account, chainConfig.Chain, chainConfig.Network, nftContract, tokenId);
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
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        Debug.Log("Buying Nft");
        BuyNFT.Response response = await CSServer.CreatePurchaseNftTransaction(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network,
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
            var txRequest = new TransactionRequest
            {
                ChainId = HexBigIntUtil.ParseHexBigInt(chainConfig.ChainId),
                To = response.tx.to,
                Value = HexBigIntUtil.ParseHexBigInt(response.tx.value),
                Data = response.tx.data,
                GasLimit = HexBigIntUtil.ParseHexBigInt(response.tx.gasLimit),
                GasPrice = HexBigIntUtil.ParseHexBigInt(response.tx.gasPrice),
            };
            var responseNft = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(txRequest);
            Debug.Log(JsonConvert.SerializeObject(responseNft));
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
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
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new UnityEngine.Vector2(0.5f, 0.5f),
            100.0f);
    }
    // parses the json response, as it has lists within the object
    public static CollectionModel.Collection ParseCollections(string json)
    {
        CollectionModel.Root root = JsonConvert.DeserializeObject<CollectionModel.Root>(json);
        return root.response.collection;
    }
    // parses the json response, as it has lists within the object same as above
    public static GetNftModel.Response ParseNft(string json)
    {
        GetNftModel.Root root = JsonConvert.DeserializeObject<GetNftModel.Root>(json);
        return root.response;
    }
}