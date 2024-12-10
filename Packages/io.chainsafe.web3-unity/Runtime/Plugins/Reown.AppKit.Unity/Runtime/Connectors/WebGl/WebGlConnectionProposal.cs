using UnityEngine;
using Reown.AppKit.Unity.WebGl.Wagmi;

namespace Reown.AppKit.Unity
{
#if UNITY_WEBGL
    public class WebGlConnectionProposal : ConnectionProposal
    {
        private bool _disposed;
        
        public WebGlConnectionProposal(Connector connector) : base(connector)
        {
            WagmiInterop.WatchAccountTriggered += WatchAccountTriggeredHandler;
        }

        private void WatchAccountTriggeredHandler(GetAccountReturnType arg)
        {
            if (arg.isConnected)
            {
                connected?.Invoke(this);
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    WagmiInterop.WatchAccountTriggered -= WatchAccountTriggeredHandler;

                _disposed = true;
                base.Dispose(disposing);
            }
        }
    }
#endif
}