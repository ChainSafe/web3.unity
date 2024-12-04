using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core.Operations
{
    internal class OperationTracker : IOperationTracker, ILifecycleParticipant
    {
        private readonly IOperationNotificationHandler handler;

        private readonly List<(OperationHandle, string)> activeOperations = new();
        private OperationHandle indexer;

        public OperationTracker()
        {
            // empty
        }

        public OperationTracker(IOperationNotificationHandler handler)
        {
            this.handler = handler;
        }

        public IOperationTrackingProcess TrackOperation(string message)
        {
            var handle = NotifyStarted(message);
            var tracker = new OperationTrackingProcess(this, handle);

            return tracker;
        }

        internal OperationHandle NotifyStarted(string message)
        {
            indexer = OperationHandle.Next(indexer);

            activeOperations.Add((indexer, message));

            if (handler != null)
            {
                if (activeOperations.Count == 1)
                {
                    handler.OnNotificationsAvailable();
                }

                handler.SetCurrentOperation(message);
            }

            return indexer;
        }

        internal void NotifyFinished(OperationHandle handle)
        {
            var index = activeOperations.FindIndex(tuple => tuple.Item1 == handle);

            if (index < 0)
            {
                throw new Web3Exception("Tried finishing operation, but it was not started.");
            }

            activeOperations.RemoveAt(index);

            if (handler == null)
            {
                return;
            }

            if (activeOperations.Count == 0)
            {
                handler.OnNotificationsOver();
                return;
            }

            var (_, currentOperationMsg) = activeOperations.Last();

            handler.SetCurrentOperation(currentOperationMsg);
        }

        public ValueTask WillStartAsync() => default;

        public ValueTask WillStopAsync()
        {
            if (activeOperations.Count > 0)
            {
                handler.OnNotificationsOver(); // force terminate all notifications
            }

            return default;
        }
    }
}