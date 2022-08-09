using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#if UNITY_WEBGL
public class ListNFTWebGL : MonoBehaviour
{
    private string chain = "ethereum";
    private string network = "goerli";
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
        List<MintedNFT.Response> response = await EVM.GetMintedNFT(chain, network, account);

        if (response[0].uri.StartsWith("ipfs://"))
        {
            response[0].uri = response[0].uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(response[0].uri);
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

        tokenURI.text = response[0].uri;
        Debug.Log(response[0].uri);
        contractAddr.text = response[0].nftContract;
        isApproved.text = response[0].isApproved.ToString();
        _itemID = response[0].id;
        _itemPrice = itemPrice.text;
        _tokenType = response[0].tokenType;
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
        ListNFT.Response response =
            await EVM.CreateListNftTransaction(chain, network, account, _itemID, Convert.ToDecimal(wei).ToString(), _tokenType);
        int value = Convert.ToInt32(response.tx.value.hex, 16);
        try
        {
            string responseNft = await Web3GL.SendTransactionData(response.tx.to, value.ToString(),
                response.tx.gasPrice, response.tx.gasLimit, response.tx.data);
            if (responseNft == null)
            {
                Debug.Log("Empty Response Object:");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Revoked Transaction" + e);
        }
    }
}
#endif

