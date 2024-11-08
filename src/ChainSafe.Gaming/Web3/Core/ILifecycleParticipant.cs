using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core
{
    /// <summary>
    /// Services and components that implement this interface will receive callbacks on <see cref="Web3"/>
    /// initialization and termination.
    /// </summary>
    public interface ILifecycleParticipant // TODO: split this into two separate interfaces?
    {
        /// <summary>
        /// ExecutionOrder for <see cref="WillStartAsync"/> and <see cref="WillStopAsync"/>.
        /// </summary>
        public int ExecutionOrder => 0;

        /// <summary>
        /// Called on <see cref="Web3"/> initialization.
        /// </summary>
        /// <returns>Task handle for the asynchronous process.</returns>
        ValueTask WillStartAsync();

        /// <summary>
        /// Called on <see cref="Web3"/> termination.
        /// </summary>
        /// <returns>Task handle for the asynchronous process.</returns>
        ValueTask WillStopAsync();
    }
}
