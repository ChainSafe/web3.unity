using System;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Models;
using ChainSafe.Gaming.Web3.Environment.Http;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Reown.Dialog
{
    public class LocalWalletButton : MonoBehaviour
    {
        public GameObject IconStub;
        public Image Icon;
        public TMP_Text Text;
        public Button Button;

        private readonly Vector3[] corners = new Vector3[4];

        private new RectTransform transform;
        private HttpHeader[] httpHeaders;
        private bool iconLoadingInitiated;
        private Rect canvasRect;
        private string walletIconEndpoint;
        private WalletModel walletData;

        private void Awake()
        {
            transform = GetComponent<RectTransform>();
            
            // Get canvas rect
            var canvas = GetComponentInParent<Canvas>();
            var canvasTransform = (RectTransform)canvas.transform;
            var canvasCorners = new Vector3[4];
            canvasTransform.GetWorldCorners(canvasCorners);
            canvasRect = new Rect(
                canvasCorners[0].x, 
                canvasCorners[0].y, 
                canvasCorners[2].x - canvasCorners[0].x, 
                canvasCorners[2].y - canvasCorners[0].y
            );
        }

        public async void Set(WalletModel data, string walletIconEndpoint, HttpHeader[] httpHeaders, Action onClick)
        {
            walletData = data;
            this.walletIconEndpoint = walletIconEndpoint;
            this.httpHeaders = httpHeaders;
            
            // Text
            Text.text = data.Name;

            // Click
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => onClick());

            // Enable stub for the icon while icon is still not loaded
            IconStub.gameObject.SetActive(true);
            Icon.gameObject.SetActive(false);
        }

        private async void Update()
        {
            if (iconLoadingInitiated)
            {
                return;
            }
            
            transform.GetWorldCorners(corners);

            if (corners.Any(c => canvasRect.Contains(c)))
            {
                iconLoadingInitiated = true;
                var icon = await DownloadIcon();

                if (icon == null)
                {
                    return;
                }
                
                Icon.sprite = icon;
                Icon.gameObject.SetActive(true);
                IconStub.gameObject.SetActive(false);
            }
        }

        private async Task<Sprite> DownloadIcon()
        {
            try
            {
                var url = $"{walletIconEndpoint}{walletData.ImageId}";
                using (var request = UnityWebRequestTexture.GetTexture(url))
                {
                    
                    request.SetRequestHeader("accept", "image/jpeg,image/png");

                    foreach (var httpHeader in httpHeaders)
                    {
                        request.SetRequestHeader(httpHeader.Name, httpHeader.Value);
                    }

                    await request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        throw new Exception("Download failed.");
                    }
                    
                    var iconTexture = DownloadHandlerTexture.GetContent(request);
                    var sprite = Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height),
                        Vector2.zero);
                    return sprite;
                }
            }
            catch
            {
                Debug.LogWarning($"Failed to download icon for the {walletData.Name} wallet.");
                return null;
            }
        }
    }
}