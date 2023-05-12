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
using Web3Unity.Scripts.Library.Web3Wallet;

public class ListCollectionsWeb3Wallet : MonoBehaviour
{
    // vars and objects
    public Text[] idsSell;
    public Text[] descriptionsSell;
    public Text[] tokenTypesSell;
    public InputField[] PriceInputs;
    public Renderer[] textureObjects;
    public GameObject C1s;
    public GameObject C2s;
    public GameObject C3s;
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
        LoadNftDataSellPage();
    }
    // load sell page data function
    async void LoadNftDataSellPage()
    {
        account = PlayerPrefs.GetString("Account");
        // create a reference to a list and iterate through it to gain token id
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
            C1s.SetActive(true);
        }
        else if (nftListAmount == 2)
        {
            C1s.SetActive(true);
            C2s.SetActive(true);
        }
        else if (nftListAmount >= 3)
        {
            C1s.SetActive(true);
            C2s.SetActive(true);
            C3s.SetActive(true);
        }

        // get nft data for each tokenId paired with nft count for local data population
        foreach (string tokenId in tokenIdList)
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
            idsSell[nftCount].text = nftResponse.id;
            tokenTypesSell[nftCount].text = nftResponse.tokenType;
            if (data.description == null)
            {
                descriptionsSell[nftCount].text = "";
            }
            else
            {
                descriptionsSell[nftCount].text = data.description;
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

    // sell nft function
    public async void SellNFT(int nftNumber)
    {
        Debug.Log("Selling Nft");
        var eth = float.Parse(PriceInputs[nftNumber].text);
        float decimals = 1000000000000000000; // 18 decimals
        var wei = eth * decimals;
        Debug.Log("ItemID: " + idsSell[nftNumber].text);
        var response =
            await EVM.CreateListNftTransaction(chain, network, account, idsSell[nftNumber].text, Convert.ToDecimal(wei).ToString(CultureInfo.InvariantCulture),
                tokenTypesSell[nftNumber].text);
        var value = Convert.ToInt32(response.tx.value.hex, 16);
        Debug.Log("Response: " + response);
        try
        {
            var responseNft = await Web3Wallet.SendTransaction(chainID, response.tx.to, value.ToString(),
                response.tx.data, response.tx.gasLimit, response.tx.gasPrice);
            if (responseNft == null) Debug.Log("Empty Response Object:");
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e);
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