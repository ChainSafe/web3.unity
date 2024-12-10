using Reown.AppKit.Unity.Components;
using Reown.AppKit.Unity.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public class NetworkLoadingPresenter : Presenter<NetworkLoadingView>
    {
        public NetworkLoadingPresenter(RouterController router, VisualElement parent) : base(router, parent)
        {
            AppKit.NetworkController.ChainChanged += ChainChangedHandler;
        }

        protected override void OnVisibleCore()
        {
            base.OnVisibleCore();

            var chainId = PlayerPrefs.GetString("WC_SELECTED_CHAIN_ID");
            var chain = AppKit.NetworkController.Chains[chainId];

            Title = chain.Name;
            var remoteSprite = RemoteSpriteFactory.GetRemoteSprite<Image>(chain.ImageUrl);
            View.SetNetworkIcon(remoteSprite);
        }

        private void ChainChangedHandler(object sender, NetworkController.ChainChangedEventArgs e)
        {
            if (!IsVisible)
                return;

            if (e.NewChain == null)
                Router.GoBack();
            else
                AppKit.CloseModal();
        }
    }
}