using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class StatusIcon : VisualElement
    {
        public const string ClassName = "status-icon";
        public static readonly string ClassNameIcon = $"{ClassName}__icon";
        public static readonly string ClassNameIconContainer = $"{ClassName}__icon-container";

        public static readonly string ClassNameIconColorError = $"{ClassName}__icon-container--color-error";
        public static readonly string ClassNameIconColorSuccess = $"{ClassName}__icon-container--color-success";

        public const string SuccessIconPath = "Reown/AppKit/Icons/icon_medium_checkmark";

        public readonly Image icon;
        public readonly VisualElement iconContainer;

        public new class UxmlFactory : UxmlFactory<StatusIcon>
        {
        }

        public StatusIcon() : this(IconType.Success, IconColor.Success)
        {
        }

        public StatusIcon(IconType type, IconColor color)
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/StatusIcon/StatusIcon");
            asset.CloneTree(this);

            AddToClassList(ClassName);

            icon = this.Q<Image>(ClassNameIcon);
            iconContainer = this.Q<VisualElement>(ClassNameIconContainer);

            switch (color)
            {
                case IconColor.Error:
                    iconContainer.AddToClassList(ClassNameIconColorError);
                    break;
                case IconColor.Success:
                    iconContainer.AddToClassList(ClassNameIconColorSuccess);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }

            switch (type)
            {
                case IconType.Success:
                    icon.vectorImage = Resources.Load<VectorImage>(SuccessIconPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public enum IconType
        {
            Success
        }

        public enum IconColor
        {
            Error,
            Success
        }
    }

    public enum StatusIconType
    {
        None,
        Success
    }
}