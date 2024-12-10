using System;
using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class CardSelect : VisualElement
    {
        public const string Name = "card-select";
        public static readonly string NameLabel = $"{Name}__label";
        public static readonly string NameContainer = $"{Name}__container";
        public static readonly string NameImageContainer = $"{Name}__icon-image-container";
        public static readonly string NameIconImageBorder = $"{Name}__icon-image-border";
        public static readonly string NameIconImage = $"{Name}__icon-image";
        public static readonly string NameStatusIconContainer = $"{Name}__status-icon-container";
        public static readonly string ClassNameActivated = $"{Name}--activated";

        public new class UxmlFactory : UxmlFactory<CardSelect, UxmlTraits>
        {
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

        public Clickable Clickable
        {
            get => _clickable;
            set
            {
                _clickable = value;
                this.AddManipulator(value);
            }
        }

        public string LabelText
        {
            get => LabelElement.text;
            set => LabelElement.text = value;
        }

        public RemoteSprite<Image> Icon
        {
            get => _icon;
            set
            {
                _icon?.UnsubscribeImage(IconImage);

                value.SubscribeImage(IconImage);
                _icon = value;
            }
        }

        public VisualElement Container { get; }
        public Label LabelElement { get; }
        public VisualElement StatusIconContainer { get; }
        private Image IconImage { get; }

        private Clickable _clickable;
        private RemoteSprite<Image> _icon;

        public CardSelect() : this(StatusIconType.None)
        {
        }

        public CardSelect(StatusIconType statusIconType)
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/CardSelect/CardSelect");
            asset.CloneTree(this);

            name = Name;

            LabelElement = this.Q<Label>(NameLabel);
            Container = this.Q<VisualElement>(NameContainer);
            StatusIconContainer = this.Q<VisualElement>(NameStatusIconContainer);
            IconImage = this.Q<Image>(NameIconImage);

            RegisterCallback<DetachFromPanelEvent, (RemoteSprite<Image>, Image)>(
                (_, data) => data.Item1?.UnsubscribeImage(data.Item2), (_icon, IconImage));

// TODO: enable hover effect only on desktop and webgl
#if UNITY_STANDALONE || UNITY_WEBGL
#endif
            switch (statusIconType)
            {
                case StatusIconType.None:
                    StatusIconContainer.style.display = DisplayStyle.None;
                    break;
                case StatusIconType.Success:
                    StatusIconContainer.Add(new StatusIcon(StatusIcon.IconType.Success, StatusIcon.IconColor.Success));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusIconType), statusIconType, null);
            }
        }

        public void SetActivated(bool activated)
        {
            if (activated)
            {
                AddToClassList(ClassNameActivated);
            }
            else
            {
                if (ClassListContains(ClassNameActivated))
                    RemoveFromClassList(ClassNameActivated);
            }
        }
    }
}