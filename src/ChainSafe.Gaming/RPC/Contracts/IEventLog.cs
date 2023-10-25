using Nethereum.RPC.Eth.DTOs;

namespace ChainSafe.Gaming.Evm.Contracts
{
    /// <summary>
    ///  Defining interface for Event logs.
    /// </summary>
    public interface IEventLog
    {
        /// <summary>
        ///  Represents a property of FilterLog.
        /// </summary>
        /// <returns>
        ///  A FilterLog object.
        /// </returns>
        FilterLog Log { get; }
    }
}