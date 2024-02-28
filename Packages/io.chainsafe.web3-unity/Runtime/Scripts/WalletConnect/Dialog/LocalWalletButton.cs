using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public class LocalWalletButton : MonoBehaviour
    {
        public GameObject IconStub;
        public Image Icon;
        public TMP_Text Text;
        public Button Button;

        public async void Set(WalletModel data, Action onClick)
        {
            // Text
            Text.text = data.Name;

            // Click
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => onClick());

            // Icon
            IconStub.gameObject.SetActive(true);
            Icon.gameObject.SetActive(false);

            if (data.Images == null || string.IsNullOrEmpty(data.Images.SmallUrl))
            {
                return;
            }

            var icon = await DownloadIcon(data.Images.SmallUrl);
            if (icon == null)
            {
                return;
            }

            Icon.sprite = icon;
            Icon.gameObject.SetActive(true);
            IconStub.gameObject.SetActive(false);
        }

        private static async Task<Sprite> DownloadIcon(string url)
        {
            try
            {
                var iconRequest = UnityWebRequest.Get(url);
                var downloadHandler = new DownloadHandlerTexture();
                iconRequest.downloadHandler = downloadHandler;
                await iconRequest.SendWebRequest();
                var iconTexture = downloadHandler.texture;
                var sprite = Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height),
                    Vector2.zero);
                return sprite;
            }
            catch
            {
                return null;
            }
        }
    }
}