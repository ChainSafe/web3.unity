using System.Threading.Tasks;
using UnityEngine;
using Reown.AppKit.Unity.WebGl.Modal;
using NativeViewType = Reown.AppKit.Unity.ViewType;
using WebGlViewType = Reown.AppKit.Unity.WebGl.Modal.ViewType;

namespace Reown.AppKit.Unity.WebGl
{
    /// <summary>
    /// Modal Controller for the web implementation of the AppKit that uses Wagmi.
    /// </summary>
    public class ModalControllerWebGl : ModalController
    {
        protected override Task InitializeAsyncCore()
        {
            ModalInterop.StateChanged += StateChangedHandler;
            return Task.CompletedTask;
        }

        private void StateChangedHandler(ModalState modalState)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLInput.captureAllKeyboardInput = !modalState.open;
#endif
            OnOpenStateChanged(new ModalOpenStateChangedEventArgs(modalState.open));
        }

        protected override void OpenCore(NativeViewType view)
        {
            var viewType = ConvertViewType(view);
            ModalInterop.Open(new OpenModalParameters(viewType));
        }

        protected override void CloseCore()
        {
            ModalInterop.Close();
        }

        private static WebGlViewType ConvertViewType(NativeViewType viewType)
        {
            return viewType switch
            {
                NativeViewType.Connect => WebGlViewType.Connect,
                NativeViewType.None => WebGlViewType.Connect,
                NativeViewType.Account => WebGlViewType.Account,
                NativeViewType.WalletSearch => WebGlViewType.AllWallets,
                NativeViewType.NetworkSearch => WebGlViewType.Networks,
                NativeViewType.QrCode => WebGlViewType.ConnectingWalletConnect,
                NativeViewType.Wallet => WebGlViewType.ConnectWallets,
                NativeViewType.NetworkLoading => WebGlViewType.Networks,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}