namespace ChainSafe.Gaming.Web3.Core.Operations
{
    /// <summary>
    /// A service which allows to wrap chosen async code, so that user gets notified when a long-lasting operation is executing.
    /// </summary>
    public interface IOperationTracker
    {
        IOperationTrackingProcess TrackOperation(string message);
    }
}