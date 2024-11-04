using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Marketplace.Extensions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace ChainSafe.Gaming.Marketplace.Samples
{
    public class UI_MarketplaceItem : MonoBehaviour
    {
        [SerializeField] private Image marketplaceItemImage;
        [SerializeField] private TMP_Text type, itemId, itemPrice, itemStatus;
        [SerializeField] private Button button;

        private MarketplaceItem _marketplaceItemModel;

        private static Dictionary<string, Sprite> _spritesDict = new();

        public async Task Initialize(MarketplaceItem model)
        {
            _marketplaceItemModel = model;
            button.interactable = model.Status == MarketplaceItemStatus.Listed;
            itemStatus.text = model.Status == MarketplaceItemStatus.Listed ? "Purchase" : model.Status.ToString();
            marketplaceItemImage.sprite = await GetSprite(model);
            type.text = model.Token.Type;
            itemId.text = "ID " + model.Token.Id;
            itemPrice.text =
                ((decimal)BigInteger.Parse(model.Price) / (decimal)BigInteger.Pow(10, 18)).ToString("0.############",
                    CultureInfo.InvariantCulture) + Web3Unity.Web3.ChainConfig.Symbol;
            button.onClick.AddListener(Purchase);
        }

        private async Task<Sprite> GetSprite(MarketplaceItem model)
        {
            Sprite sprite = null;
            string imageUrl = (string)model.Token.Metadata["image"];
            if (_spritesDict.TryGetValue(imageUrl, out sprite)) return sprite;

            var unityWebRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            await unityWebRequest.SendWebRequest();
            if (unityWebRequest.error != null)
            {
                Debug.LogError("There was an error getting the texture " + unityWebRequest.error);
                return null;
            }

            var myTexture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;

            sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), Vector2.one * 0.5f);

            return sprite;
        }

        private async void Purchase()
        {
            try
            {
                await Web3Unity.Web3.Marketplace().Purchase(_marketplaceItemModel.Id, _marketplaceItemModel.Price);
                button.interactable = false;
                itemStatus.text = "Sold";
            }
            catch (ServiceNotBoundWeb3Exception<ISigner> _)
            {
                Debug.LogError("You wanted to purchase an item and you don't have a wallet. Please connect the wallet to make a purchase");
                Web3Unity.ConnectModal.Open();
            }
        }
    }
}