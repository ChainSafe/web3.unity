using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class Link : VisualElement
    {
        public const string Name = "link";
        public static readonly string NameIcon = $"{Name}__icon";
        public static readonly string ClassNameSizeSmall = $"{Name}--size-small";
        public static readonly string ClassNameSizeMedium = $"{Name}--size-medium";
        public static readonly string ClassNameVariantMain = $"{Name}--variant-main";
        public static readonly string ClassNameVariantGray = $"{Name}--variant-gray";
        public static readonly string ClassNameVariantIcon = $"{Name}--variant-icon";

        public string Text
        {
            get => _label.text ?? string.Empty;
            set => _label.text = value;
        }

        public LinkSize Size
        {
            get => _size;
            set
            {
                _size = value;
                switch (value)
                {
                    case LinkSize.Small:
                        AddToClassList(ClassNameSizeSmall);
                        break;
                    case LinkSize.Medium:
                        AddToClassList(ClassNameSizeMedium);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public LinkVariant Variant
        {
            get => _variant;
            set
            {
                _variant = value;
                switch (value)
                {
                    case LinkVariant.Main:
                        AddToClassList(ClassNameVariantMain);
                        break;
                    case LinkVariant.Gray:
                        AddToClassList(ClassNameVariantGray);
                        break;
                    case LinkVariant.Icon:
                        AddToClassList(ClassNameVariantIcon);
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

        public readonly Image icon;
        private readonly Label _label;
        private LinkSize _size = LinkSize.Small;
        private LinkVariant _variant = LinkVariant.Main;
        private Clickable _clickable;

        public new class UxmlFactory : UxmlFactory<Link, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription tText = new()
            {
                name = "text"
            };

            private UxmlEnumAttributeDescription<LinkSize> tSize = new()
            {
                name = "size"
            };

            private UxmlEnumAttributeDescription<LinkVariant> tVariant = new()
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

                var link = ve as Link;
                link.Text = tText.GetValueFromBag(bag, cc);
                link.Size = tSize.GetValueFromBag(bag, cc);
                link.Variant = tVariant.GetValueFromBag(bag, cc);

                var iconUrl = tIcon.GetValueFromBag(bag, cc);
                if (!string.IsNullOrEmpty(iconUrl))
                    link.icon.vectorImage = Resources.Load<VectorImage>(iconUrl);
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

        public Link()
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/Link/Link");
            asset.CloneTree(this);

            name = Name;

            icon = this.Q<Image>(NameIcon);
            _label = this.Q<Label>();
            focusable = true;
        }

        public Link(string text, Action clickEvent) : this()
        {
            Text = text;
            Clickable = new Clickable(clickEvent);
        }
    }

    public enum LinkVariant
    {
        Main,
        Gray,
        Icon
    }

    public enum LinkSize
    {
        Small,
        Medium
    }
}