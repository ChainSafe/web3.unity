using System;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core
{
    /// <summary>
    /// Utility class for delay operations.
    /// </summary>
    public static class DelayUtil
    {
        /// <summary>
        /// Replicates Task.Delay behaviour. Works on a single-threaded platforms like Unity WebGL.
        /// </summary>
        /// <param name="delay">The time to wait.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SafeDelay(TimeSpan delay) // TODO: use default method if platform supports multithreading
        {
            var start = DateTime.UtcNow;

            while (DateTime.UtcNow - start < delay)
            {
                await Task.Yield();
            }
        }
    }
}