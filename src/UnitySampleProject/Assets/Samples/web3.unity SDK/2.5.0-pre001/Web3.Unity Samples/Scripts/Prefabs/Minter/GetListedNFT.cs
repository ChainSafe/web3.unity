using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Scripts.EVM.Remote;
// using Web3Unity.Scripts.Library.Web3Wallet;

public class GetListedNFT : MonoBehaviour
{
    public Renderer textureObject;
    public Text price;
    public Text seller;
    public Text description;
    public Text listPercentage;
    public Text contractAddr;
    public Text tokenId;
    public Text itemId;
    private string _itemPrice = "";
    private string _tokenType = "";

    private string _itemID = "";


    public void Awake()
    {

        price.text = "";
        seller.text = "";
        description.text = "";
        listPercentage.text = "";
        tokenId.text = "";
        itemId.text = "";
        contractAddr.text = "";
    }

    // Start is called before the first frame update
    async void Start()
    {
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        List<GetNftListModel.Response> response = await CSServer.GetNftMarket(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network);
        price.text = response[0].price;
        seller.text = response[0].seller;
        Debug.Log("Seller: " + response[0].seller);
        if (response[0].uri.StartsWith("ipfs://"))
        {
            response[0].uri = response[0].uri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
            Debug.Log("Response URI" + response[0].uri);
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(response[0].uri);
        await webRequest.SendWebRequest();
        RootGetNFT data =
            JsonConvert.DeserializeObject<RootGetNFT>(
                System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        if (data.description == null)
        {
            description.text = "";
        }
        else
        {
            description.text = data.description;
        }

        // parse json to get image uri
        string imageUri = data.image;
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
            StartCoroutine(DownloadImage(imageUri));
        }
        else
        {
            StartCoroutine(DownloadImage(imageUri));
        }

        if (data.properties != null)
        {
            foreach (var prop in data.properties.additionalFiles)
            {
                if (prop.StartsWith("ipfs://"))
                {
                    var additionalURi = prop.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
                }
            }
        }
        listPercentage.text = response[0].listedPercentage;
        Debug.Log(response[0].listedPercentage);
        contractAddr.text = response[0].nftContract;
        itemId.text = response[0].itemId;
        _itemID = response[0].itemId;
        _itemPrice = response[0].price;
        _tokenType = response[0].tokenType;
        tokenId.text = response[0].tokenId;
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
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
            textureObject.GetComponent<Image>().sprite = webSprite;
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new UnityEngine.Vector2(0.5f, 0.5f),
            100.0f);
    }

    public async void PurchaseItem()
    {
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        BuyNFT.Response response = await CSServer.CreatePurchaseNftTransaction(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network,
            await Web3Accessor.Web3.Signer.GetAddress(), _itemID, _itemPrice, _tokenType);
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
}

