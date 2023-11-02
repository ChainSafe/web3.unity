using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core;
using Nethereum.Contracts.QueryHandlers.MultiCall;

namespace ChainSafe.Gaming.MultiCall
{
    /// <summary>
    /// Represents an interface for making batched Ethereum function calls using the MultiCall service.
    /// </summary>
    public interface IMultiCall : ILifecycleParticipant
    {
        /// <summary>
        /// Executes a batch of Ethereum function calls using the MultiCall service asynchronously.
        /// </summary>
        /// <param name="multiCalls">An array of function calls to execute in a batch.</param>
        /// <param name="pageSize">The maximum number of calls per batch request (default is 3000).</param>
        /// <returns>A list of results from executing the batched calls.</returns>
        public Task<List<Result>> MultiCallAsync(Call3Value[] multiCalls, int pageSize = 3000);
    }
}