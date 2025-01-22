using System;
using System.Collections.Generic;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public class EmbeddedWalletRequestHandler
    {
        private readonly Queue<IEmbeddedWalletRequest> requests = new Queue<IEmbeddedWalletRequest>();

        public event Action<IEmbeddedWalletRequest> RequestQueued;

        public event Action<IEmbeddedWalletRequest> RequestApproved;

        public event Action<IEmbeddedWalletRequest> RequestDeclined;

        public event Action<IEmbeddedWalletRequest> RequestConfirmed;

        public void Enqueue(IEmbeddedWalletRequest request)
        {
            requests.Enqueue(request);

            RequestQueued?.Invoke(request);
        }

        public IEmbeddedWalletRequest Dequeue()
        {
            return requests.Dequeue();
        }

        public IEmbeddedWalletRequest Peek()
        {
            return requests.Peek();
        }

        public void Approve()
        {
            var request = Dequeue();

            RequestApproved?.Invoke(request);
        }

        public void Decline()
        {
            var request = Dequeue();

            RequestDeclined?.Invoke(request);
        }

        public void Confirm(IEmbeddedWalletRequest request)
        {
            RequestConfirmed?.Invoke(request);
        }
    }
}