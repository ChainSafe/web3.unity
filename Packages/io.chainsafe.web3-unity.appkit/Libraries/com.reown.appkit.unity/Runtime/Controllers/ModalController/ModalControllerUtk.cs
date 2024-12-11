using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Utils;
using Reown.Sign.Unity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    /// <summary>
    ///     ModalController for Unity UI Toolkit
    /// </summary>
    public class ModalControllerUtk : ModalController
    {
        private readonly ModalOpenStateChangedEventArgs _openStateChangedEventArgsTrueOnClose = new(false);

        private readonly ModalOpenStateChangedEventArgs _openStateChangedEventArgsTrueOnOpen = new(true);
        public UIDocument UIDocument { get; private set; }

        public Modal Modal { get; private set; }

        public VisualElement AppKitModalElement { get; private set; }

        public RouterController RouterController { get; private set; }

        protected ModalHeaderPresenter ModalHeaderPresenter { get; private set; }

        protected override Task InitializeAsyncCore()
        {
            var web3Modal = AppKit.Instance;
            UIDocument = web3Modal.GetComponentInChildren<UIDocument>(true);

            AppKitModalElement = UIDocument.rootVisualElement.Children().First();

            Modal = AppKitModalElement.Q<Modal>();

            RouterController = new RouterController(Modal.body);
            RouterController.ViewChanged += ViewChangedHandler;

            ModalHeaderPresenter = new ModalHeaderPresenter(RouterController, Modal);

            LoadingAnimator.Instance.PauseAnimation();

            UnityEventsDispatcher.Instance.Tick += TickHandler;

            return Task.CompletedTask;
        }

        private void ViewChangedHandler(object _, ViewChangedEventArgs args)
        {
            if (args.newViewType == ViewType.None)
                CloseCore();
        }

        protected override void OpenCore(ViewType view)
        {
            AppKitModalElement.visible = true;
            RouterController.OpenView(view);
            LoadingAnimator.Instance.ResumeAnimation();
            OnOpenStateChanged(_openStateChangedEventArgsTrueOnOpen);

            AppKit.EventsController.SendEvent(new Event
            {
                name = "MODAL_OPEN",
                properties = new Dictionary<string, object>
                {
                    { "connected", AppKit.IsAccountConnected }
                }
            });
        }

        protected override void CloseCore()
        {
            AppKitModalElement.visible = false;
            LoadingAnimator.Instance.PauseAnimation();
            RouterController.CloseAllViews();
            OnOpenStateChanged(_openStateChangedEventArgsTrueOnClose);

            AppKit.EventsController.SendEvent(new Event
            {
                name = "MODAL_CLOSE",
                properties = new Dictionary<string, object>
                {
                    { "connected", AppKit.IsAccountConnected }
                }
            });
        }

        private void TickHandler()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                RouterController.GoBack();
        }
    }
}