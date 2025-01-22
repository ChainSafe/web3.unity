using System;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public interface IEmbeddedWalletRequest
    {
        public DateTime Timestamp { get; }

        public void Confirm();
    }
}