using UnityEngine.UIElements;
using UnityEngine;

namespace Reown.AppKit.Unity.Components
{
    public class ModalHeader : VisualElement
    {
        public const string Name = "modal-header";
        public static readonly string NameContainer = $"{Name}__container";
        public static readonly string NameBody = $"{Name}__body";
        public static readonly string NameLeftSlot = $"{Name}__left-slot";
        public static readonly string NameRightSlot = $"{Name}__right-slot";

        public static readonly string NameSnackbar = $"{Name}__snackbar";
        public static readonly string NameSnackbarContainerHorizontal = $"{Name}__snackbar-container-horizontal";
        public static readonly string NameSnackbarContainerVertical = $"{Name}__snackbar-container-vertical";

        public readonly VisualElement container;
        public readonly VisualElement body;
        public readonly VisualElement leftSlot;
        public readonly VisualElement rightSlot;
        public readonly Snackbar snackbar;

        public new class UxmlFactory : UxmlFactory<ModalHeader>
        {
        }

        public ModalHeader()
        {
            var asset = Resources.Load<VisualTreeAsset>($"Reown/AppKit/Components/ModalHeader/ModalHeader");
            asset.CloneTree(this);

            name = Name;

            container = this.Q<VisualElement>(NameContainer);
            body = this.Q<VisualElement>(NameBody);
            leftSlot = this.Q<VisualElement>(NameLeftSlot);
            rightSlot = this.Q<VisualElement>(NameRightSlot);
            snackbar = this.Q<Snackbar>(NameSnackbar);

            HideSnackbar();
        }

        public void ShowSnackbar(Snackbar.IconColor color, VectorImage icon, string message)
        {
            snackbar.CurrentIconColor = color;
            snackbar.Icon.vectorImage = icon;
            snackbar.Message = message;

            snackbar.style.opacity = 1;
        }

        public void HideSnackbar()
        {
            snackbar.style.opacity = 0;
        }
    }
}