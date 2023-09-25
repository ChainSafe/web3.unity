using System;
using System.Collections;
using System.Globalization;
using System.Text;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Scripts.EVM.Remote;

namespace Web3Unity.Scripts.Prefabs.Minter
{
    public class ListNft : MonoBehaviour
    {
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

        public async void Awake()
        {
            description.text = "";
            tokenURI.text = "";
            isApproved.text = "";
            contractAddr.text = "";
            account = await Web3Accessor.Web3.Signer.GetAddress();
        }

        // Start is called before the first frame update
        private async void Start()
        {
            var chainConfig = Web3Accessor.Web3.ChainConfig;
            playerAccount.text = account;
            try
            {
                var response = await CSServer.GetMintedNFT(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network, account);

                if (response[1].uri == null)
                {
                    Debug.Log("Not Listed Items");
                    return;
                }

                if (response[1].uri.StartsWith("ipfs://"))
                {
                    response[1].uri = response[1].uri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
                }

                var webRequest = UnityWebRequest.Get(response[1].uri);
                await webRequest.SendWebRequest();
                var data = JsonConvert.DeserializeObject<RootGetNFT>(Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                description.text = data.description;
                // parse json to get image uri
                var imageUri = data.image;
                if (imageUri.StartsWith("ipfs://"))
                {
                    imageUri = imageUri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
                    StartCoroutine(DownloadImage(imageUri));
                }
                else
                {
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
        private IEnumerator DownloadImage(string mediaUrl)
        {
            var request = UnityWebRequestTexture.GetTexture(mediaUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                var webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                var webSprite = SpriteFromTexture2D(webTexture);
                textureObject.GetComponent<Image>().sprite = webSprite;
            }
        }

        private Sprite SpriteFromTexture2D(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new UnityEngine.Vector2(0.5f, 0.5f), 100.0f);
        }

        public async void ListItem()
        {
            var chainConfig = Web3Accessor.Web3.ChainConfig;
            var eth = float.Parse(_itemPrice);
            float decimals = 1000000000000000000; // 18 decimals
            var wei = eth * decimals;
            Debug.Log("ItemID: " + _itemID);
            var response =
                await CSServer.CreateListNftTransaction(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network, account, _itemID, Convert.ToDecimal(wei).ToString(CultureInfo.InvariantCulture),
                    _tokenType);
            var value = Convert.ToInt32(response.tx.value.hex, 16);
            Debug.Log("Response: " + response);
            try
            {
                var txRequest = new TransactionRequest
                {
                    ChainId = HexBigIntUtil.ParseHexBigInt(chainConfig.ChainId),
                    To = response.tx.to,
                    Value = new HexBigInteger(value),
                    Data = response.tx.data,
                    GasLimit = HexBigIntUtil.ParseHexBigInt(response.tx.gasLimit),
                    GasPrice = HexBigIntUtil.ParseHexBigInt(response.tx.gasPrice),
                };
                var responseNft = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(txRequest);
                Debug.Log(JsonConvert.SerializeObject(responseNft));
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }
    }
}