using System.Collections;
using System.Collections.Generic;
using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Utils;
using Reown.Sign.Unity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public class ModalHeaderPresenter : Presenter<ModalHeader>
    {
        private readonly Label _title;
        private readonly Dictionary<ViewType, VisualElement> _leftSlotItems = new();

        private Coroutine _snackbarCoroutine;
        private bool _disposed;

        public ModalHeaderPresenter(RouterController routerController, Modal parent) : base(routerController, parent)
        {
            View.style.display = DisplayStyle.Flex;

            Router.ViewChanged += ViewChangedHandler;
            AppKit.NotificationController.Notification += NotificationHandler;
            AppKit.ModalController.OpenStateChanged += ModalOpenStateChangedHandler;

            // Create title label
            _title = new Label();
            _title.AddToClassList("text-paragraph");
            View.body.Add(_title);

            // Create Back button and add it to the left slot
            var goBackIconLink = new IconLink(
                Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_medium_chevronleft"),
                routerController.GoBack)
            {
                style =
                {
                    display = DisplayStyle.None
                }
            };
            View.leftSlot.Add(goBackIconLink);

            // Assign buttons to the corresponding view types
            _leftSlotItems.Add(ViewType.QrCode, goBackIconLink);
            _leftSlotItems.Add(ViewType.Wallet, goBackIconLink);
            _leftSlotItems.Add(ViewType.WalletSearch, goBackIconLink);
            _leftSlotItems.Add(ViewType.NetworkSearch, goBackIconLink);
            _leftSlotItems.Add(ViewType.NetworkLoading, goBackIconLink);

            // Close button
            View.rightSlot.Add(new IconLink(
                Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_bold_xmark"),
                AppKit.CloseModal
            ));
        }

        protected override ModalHeader CreateViewInstance()
        {
            return (Parent as Modal)?.header ?? Parent.Q<ModalHeader>();
        }

        private void ModalOpenStateChangedHandler(object _, ModalOpenStateChangedEventArgs e)
        {
            if (!e.IsOpen)
            {
                View.leftSlot.style.visibility = Visibility.Hidden;
                View.rightSlot.style.visibility = Visibility.Hidden;
            }
        }

        private void NotificationHandler(object sender, NotificationEventArgs notification)
        {
            if (_snackbarCoroutine != null)
                UnityEventsDispatcher.Instance.StopCoroutine(_snackbarCoroutine);

            _snackbarCoroutine = UnityEventsDispatcher.Instance.StartCoroutine(ShowSnackbarCoroutine(notification));
        }

        private IEnumerator ShowSnackbarCoroutine(NotificationEventArgs notification)
        {
            var snackbarIconColor = notification.type switch
            {
                NotificationType.Error => Snackbar.IconColor.Error,
                NotificationType.Success => Snackbar.IconColor.Success,
                NotificationType.Info => Snackbar.IconColor.Info,
                _ => Snackbar.IconColor.Info
            };

            var icon = notification.type switch
            {
                NotificationType.Error => Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_bold_warningcircle"),
                NotificationType.Success => Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_bold_checkmark"),
                NotificationType.Info => Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_bold_info"),
                _ => Resources.Load<VectorImage>("Reown/AppKit/Icons/icon_bold_warningcircle")
            };

            View.ShowSnackbar(snackbarIconColor, icon, notification.message);

            yield return new WaitForSeconds(2);
            View.HideSnackbar();

            _snackbarCoroutine = null;
        }

        private void ViewChangedHandler(object _, ViewChangedEventArgs args)
        {
            _title.text = args.newViewType == ViewType.None
                ? string.Empty
                : args.newPresenter.Title.FontWeight600();

            if (_leftSlotItems.TryGetValue(args.oldViewType, out var oldItem))
                oldItem.style.display = DisplayStyle.None;

            if (args.newPresenter == null)
                return;

            // Right slot
            View.rightSlot.style.visibility = !args.newPresenter.EnableCloseButton
                ? Visibility.Hidden
                : Visibility.Visible;

            // Left slot
            if (_leftSlotItems.TryGetValue(args.newViewType, out var newItem))
            {
                newItem.style.display = DisplayStyle.Flex;
                View.leftSlot.style.visibility = Visibility.Visible;
            }
            else
            {
                View.leftSlot.style.visibility = Visibility.Hidden;
            }

            // Header border
            View.style.borderBottomWidth = args.newPresenter.HeaderBorder
                ? 1
                : 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Router.ViewChanged -= ViewChangedHandler;
                AppKit.NotificationController.Notification -= NotificationHandler;
                AppKit.ModalController.OpenStateChanged -= ModalOpenStateChangedHandler;
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}