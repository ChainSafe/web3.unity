namespace ChainSafe.Gaming.Web3.Core.Operations
{
    internal class OperationTrackingProcess : IOperationTrackingProcess
    {
        private readonly OperationTracker tracker;
        private readonly OperationHandle handle;

        public OperationTrackingProcess(OperationTracker tracker, OperationHandle handle)
        {
            this.handle = handle;
            this.tracker = tracker;
        }

        public void Dispose()
        {
            tracker.NotifyFinished(handle);
        }
    }
}