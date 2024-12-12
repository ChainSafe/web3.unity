using UnityEngine.UIElements;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.Components
{
    public class WalletImage : VisualElement
    {
        public const string Name = "wallet-image";
        public static readonly string ClassNameSmall = $"{Name}--size-small";
        public static readonly string ClassNameMedium = $"{Name}--size-medium";
        public static readonly string ClassNameLarge = $"{Name}--size-large";

        private VisualElementSize _size;

        public WalletImage()
        {
            AddToClassList(Name);
        }
        
        public new class UxmlFactory : UxmlFactory<WalletImage, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlEnumAttributeDescription<VisualElementSize> _tSize = new()
            {
                name = "size"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var image = ve as WalletImage;
                image.Size = _tSize.GetValueFromBag(bag, cc);

                image.AddToClassList(Name);
            }
        }

        public VisualElementSize Size
        {
            get => _size;
            set
            {
                _size = value;
                switch (value)
                {
                    case VisualElementSize.Small:
                        AddToClassList(ClassNameSmall);
                        break;
                    case VisualElementSize.Medium:
                        AddToClassList(ClassNameMedium);
                        break;
                    case VisualElementSize.Large:
                        AddToClassList(ClassNameLarge);
                        break;
                }
            }
        }
    }
}