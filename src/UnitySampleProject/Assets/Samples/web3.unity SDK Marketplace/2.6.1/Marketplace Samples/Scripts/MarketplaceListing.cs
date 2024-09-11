
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EvmMarketplace = Scripts.EVM.Marketplace.Marketplace;
using Vector2 = UnityEngine.Vector2;


public class MarketplaceListing : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text nftId;
    [SerializeField] private Image nftImage;
    [SerializeField] private TMP_Text nftPrice;
    [SerializeField] private TMP_Text nftType;

    private static readonly Dictionary<string, Sprite> _cachedSprites = new ();
    private MarketplaceModel.Item _item;
    private string _marketplaceContract;
    
    public async Task Init(string marketplaceContract, MarketplaceModel.Item item, MarketplaceModel.MarketplaceItemMetaData metaData)
    {
        _marketplaceContract = marketplaceContract;
        nftId.text = item.id;
        var ethValue = (decimal)BigInteger.Parse(item.price) / (decimal)BigInteger.Pow(10, 18);
        nftPrice.text = ethValue.ToString("0.##################") + " " + Web3Accessor.Web3.ChainConfig.Symbol.ToUpper();
        nftType.text = item.token.token_type;
        _item = item;
        
        if (!_cachedSprites.TryGetValue(metaData.image, out var sprite))
        {
            Texture2D texture = await ImportTexture(metaData.image);
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            _cachedSprites[metaData.image] = sprite = newSprite;
        }

        nftImage.sprite = sprite;
    }

    private async Task<Texture2D> ImportTexture(string uri)
    {
        var textureRequest = UnityWebRequestTexture.GetTexture(uri);
        await textureRequest.SendWebRequest();
        if (textureRequest.result != UnityWebRequest.Result.Success)
        {
            throw new Web3Exception($"Texture request failure: {textureRequest.error}");
        }
        var texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
        return texture;
    }

    public async void PurchaseNFT()
    {
        await EvmMarketplace.PurchaseNft(_marketplaceContract, _item.id, _item.price);
    }
}
