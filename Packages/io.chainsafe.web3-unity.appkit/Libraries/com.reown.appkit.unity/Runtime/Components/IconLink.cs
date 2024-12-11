using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class IconLink : VisualElement
    {
        public const string Name = "icon-link";
        public static readonly string NameImage = $"{Name}__image";
        public static readonly string ClassNameVariantNeutral = $"{Name}--variant-neutral";
        public static readonly string ClassNameVariantShade = $"{Name}--variant-shade";
        public static readonly string ClassNameVariantMain = $"{Name}--variant-main";

        public IconLinkVariant Variant
        {
            get => _variant;
            set
            {
                _variant = value;
                switch (value)
                {
                    case IconLinkVariant.Main:
                        AddToClassList(ClassNameVariantMain);
                        break;
                    case IconLinkVariant.Neutral:
                        AddToClassList(ClassNameVariantNeutral);
                        break;
                    case IconLinkVariant.Shade:
                        AddToClassList(ClassNameVariantShade);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public Clickable Clickable
        {
            get => _clickable;
            set
            {
                _clickable = value;
                this.AddManipulator(value);
            }
        }

        public event Action Clicked
        {
            add
            {
                if (Clickable == null)
                    Clickable = new Clickable(value);
                else
                    Clickable.clicked += value;
            }
            remove
            {
                if (Clickable == null)
                    return;
                Clickable.clicked -= value;
            }
        }

        public readonly Image image;

        private Clickable _clickable;
        private IconLinkVariant _variant;

        public new class UxmlFactory : UxmlFactory<IconLink, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlEnumAttributeDescription<IconLinkVariant> tVariant = new()
            {
                name = "variant"
            };

            private UxmlStringAttributeDescription tIcon = new()
            {
                name = "icon"
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var link = ve as IconLink;
                link.Variant = tVariant.GetValueFromBag(bag, cc);

                var iconUrl = tIcon.GetValueFromBag(bag, cc);
                if (!string.IsNullOrEmpty(iconUrl))
                    link.image.vectorImage = Resources.Load<VectorImage>(iconUrl);
            }
        }

        public IconLink() : this(null, (Action)null)
        {
        }

        public IconLink(VectorImage icon, Action clickEvent)
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/IconLink/IconLink");
            asset.CloneTree(this);

            name = Name;

            image = this.Q<Image>();
            if (icon == null)
                icon = Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_regular_info");
            image.vectorImage = icon;

            Clickable = new Clickable(clickEvent);
            focusable = true;
        }
    }

    public enum IconLinkVariant
    {
        Main,
        Neutral,
        Shade
    }
}