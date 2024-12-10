using System;

namespace Reown.AppKit.Unity
{
    public class ConnectionProposal : IDisposable
    {
        public bool IsConnected { get; protected set; }
        
        public bool IsSignarureRequested { get; protected set; }

        public event Action<ConnectionProposal> ConnectionUpdated
        {
            add => connectionUpdated += value;
            remove => connectionUpdated -= value;
        }

        public event Action<ConnectionProposal> Connected
        {
            add => connected += value;
            remove => connected -= value;
        }
        
        public readonly Connector connector;

        protected Action<ConnectionProposal> connectionUpdated;
        protected Action<ConnectionProposal> connected;

        private bool _disposed;

        public ConnectionProposal(Connector connector)
        {
            this.connector = connector;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                connectionUpdated = null;
                connected = null;
            }

            _disposed = true;
        }
    }
}