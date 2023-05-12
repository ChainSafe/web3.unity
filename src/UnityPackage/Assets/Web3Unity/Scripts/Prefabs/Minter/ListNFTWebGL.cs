using System;
using System.Collections;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;

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
    private async void Start()
    {
        playerAccount.text = account;
        var response = await EVM.GetMintedNFT(chain, network, account);

        if (response[1].uri.StartsWith("ipfs://"))
            response[1].uri = response[1].uri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");

        var webRequest = UnityWebRequest.Get(response[1].uri);
        await webRequest.SendWebRequest();
        var data =
            JsonConvert.DeserializeObject<RootGetNFT>(
                System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        description.text = data.description;
        // parse json to get image uri
        var imageUri = data.image;
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
            StartCoroutine(DownloadImage(imageUri));
        }

        tokenURI.text = response[1].uri;
        Debug.Log(response[1].uri);
        contractAddr.text = response[1].nftContract;
        isApproved.text = response[1].isApproved.ToString();
        _itemID = response[1].id;
        _itemPrice = itemPrice.text;
        _tokenType = response[1].tokenType;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DownloadImage(string MediaUrl)
    {
        var request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var webTexture = ((DownloadHandlerTexture) request.downloadHandler).texture as Texture2D;
            var webSprite = SpriteFromTexture2D(webTexture);
            textureObject.GetComponent<Image>().sprite = webSprite;
        }
    }

    private Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
            100.0f);
    }

    public async void ListItem()
    {
        var eth = float.Parse(_itemPrice);
        float decimals = 1000000000000000000; // 18 decimals
        var wei = eth * decimals;
        var response =
            await EVM.CreateListNftTransaction(chain, network, account, _itemID, Convert.ToDecimal(wei).ToString(),
                _tokenType);
        var value = Convert.ToInt32(response.tx.value.hex, 16);
        try
        {
            var responseNft = await Web3GL.SendTransactionData(response.tx.to, value.ToString(),
                response.tx.gasPrice, response.tx.gasLimit, response.tx.data);
            if (responseNft == null) Debug.Log("Empty Response Object:");
        }
        catch (Exception e)
        {
            Debug.Log("Revoked Transaction" + e);
        }
    }
}
#endif