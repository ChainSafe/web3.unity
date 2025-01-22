using System;

namespace ChainSafe.Gaming.Web3.Core.Operations
{
    /// <summary>
    /// Miscellaneous interface for an operation tracking process, that cancels the tracking process when disposed.
    /// </summary>
    public interface IOperationTrackingProcess : IDisposable
    {
    }
}