using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Core.Operations
{
    public static class OperationTrackerExtensions
    {
        /// <summary>
        /// Use <see cref="IOperationTracker"/> to notify user when there is a long-lasting operation currently executing.
        /// </summary>
        /// <param name="web3">The Web3 client.</param>
        /// <returns>The <see cref="IOperationTracker"/> service instance.</returns>
        public static IOperationTracker OperationTracker(this Web3 web3)
        {
            return web3.ServiceProvider.GetRequiredService<IOperationTracker>();
        }
    }
}