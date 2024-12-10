using System;
using System.Threading.Tasks;

namespace Reown.AppKit.Unity
{
    public abstract class ModalController
    {
        public bool IsOpen { get; private set; }

        public event EventHandler<ModalOpenStateChangedEventArgs> OpenStateChanged;

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                throw new Exception("Already initialized"); // TODO: use custom ex type

            OpenStateChanged += OpenStateChangedHandler;
            await InitializeAsyncCore();

            _isInitialized = true;
        }

        public void Open(ViewType view)
        {
            OpenCore(view);
        }

        public void Close()
        {
            CloseCore();
        }

        protected virtual void OnOpenStateChanged(ModalOpenStateChangedEventArgs e)
        {
            OpenStateChanged?.Invoke(this, e);
        }

        private void OpenStateChangedHandler(object sender, ModalOpenStateChangedEventArgs e)
        {
            IsOpen = e.IsOpen;
        }

        protected abstract Task InitializeAsyncCore();

        protected abstract void OpenCore(ViewType view);

        protected abstract void CloseCore();
    }

    public class ModalOpenStateChangedEventArgs : EventArgs
    {
        public bool IsOpen { get; }

        public ModalOpenStateChangedEventArgs(bool isOpen)
        {
            IsOpen = isOpen;
        }
    }

    public enum ViewType
    {
        None,
        Connect,
        QrCode,
        Wallet,
        WalletSearch,
        Account,
        NetworkSearch,
        NetworkLoading,
        Siwe
    }
}