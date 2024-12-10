using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class QrCode : VisualElement
    {
        public const string Name = "qrcode";

        private static readonly CustomStyleProperty<Color> BgColorProperty = new("--ro-qrcode-bg-color");
        private static readonly CustomStyleProperty<Color> FgColorProperty = new("--ro-qrcode-fg-color");

        private readonly Image _qrcodeImage;

        private Color _bgColor = Color.white;
        private Color _fgColor = Color.black;
        private string _data;

        public QrCode() : this(string.Empty)
        {
        }

        public QrCode(string data)
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/QrCode/QrCode");
            asset.CloneTree(this);

            AddToClassList(Name);

            _qrcodeImage = this.Q<Image>("qrcode-image");

            Data = data;

            RegisterCallback(new EventCallback<CustomStyleResolvedEvent>(CustomStyleResolvedHandler));
        }

        public string Data
        {
            get => _data;
            set
            {
#if UNITY_EDITOR
                // To bypass UI Builder errors
                if (LoadingAnimator.Instance == null)
                    return;
#endif

                if (string.IsNullOrWhiteSpace(value))
                {
                    LoadingAnimator.Instance.Subscribe(_qrcodeImage);
                    _data = value;
                }
                else
                {
                    LoadingAnimator.Instance.Unsubscribe(_qrcodeImage);
                    _qrcodeImage.image = QRCode.EncodeTexture(value, _fgColor, _bgColor);
                    _data = value;
                }
            }
        }

        private void CustomStyleResolvedHandler(CustomStyleResolvedEvent evt)
        {
            _ = evt.customStyle.TryGetValue(BgColorProperty, out _bgColor);
            _ = evt.customStyle.TryGetValue(FgColorProperty, out _fgColor);
        }

        public new class UxmlFactory : UxmlFactory<QrCode>
        {
        }
    }
}